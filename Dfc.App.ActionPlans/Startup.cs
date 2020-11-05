using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;
using DFC.App.ActionPlans.Cosmos.Interfaces;
using DFC.App.ActionPlans.Cosmos.Models;
using DFC.App.ActionPlans.Cosmos.Services;
using DFC.App.ActionPlans.Services.DSS.Interfaces;
using DFC.App.ActionPlans.Models;
using DFC.App.ActionPlans.Services;
using DFC.App.ActionPlans.Services.DSS.Models;
using DFC.App.ActionPlans.Services.DSS.Services;
using DFC.Personalisation.Common.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;


namespace Dfc.App.ActionPlans
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
           
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddApplicationInsightsTelemetry();
            services.AddControllersWithViews();
            services.AddHttpContextAccessor();

            services.AddScoped<IDssReader, DssService>();
            services.AddScoped<IDssWriter, DssService>();
            services.Configure<DssSettings>(Configuration.GetSection(nameof(DssSettings)));
            services.Configure<CompositeSettings>(Configuration.GetSection(nameof(CompositeSettings)));
            services.Configure<CosmosSettings>(Configuration.GetSection(nameof(CosmosSettings)));
            services.AddScoped((x) => new CosmosClient(
                accountEndpoint: Configuration.GetSection("CosmosSettings:ApiUrl").Value, 
                authKeyOrResourceToken: Configuration.GetSection("CosmosSettings:ApiKey").Value));
            services.AddScoped<ICosmosService, CosmosService>();

            services.Configure<AuthSettings>(Configuration.GetSection("AuthSettings"));
            var authSettings = new AuthSettings();
            var appPath = Configuration.GetSection("CompositeSettings:Path").Value;

            Configuration.GetSection("AuthSettings").Bind(authSettings);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(cfg =>
                {
                    cfg.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateIssuerSigningKey = true,
                            ValidateLifetime = true,
                            ClockSkew = TimeSpan.Zero,
                            ValidIssuer = authSettings.Issuer,
                            ValidAudience = authSettings.ClientId,
                            IssuerSigningKey =
                                new SymmetricSecurityKey(
                                    Encoding.ASCII.GetBytes(authSettings.ClientSecret)),
                        };
                    cfg.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.Redirect(appPath + "/session-timeout");
                            }
                            else
                            {
                                context.Response.Redirect(authSettings.SignInUrl);
                            }
                            return Task.CompletedTask;
                            
                            
                        },
                       OnChallenge = context =>
                       {
                            var requestingUrl = context.Request.Path.ToString().Replace("/head",appPath);
                            context.Response.Redirect($"{authSettings.SignInUrl}?redirectUrl={requestingUrl}");
                            context.HandleResponse();
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddMvc().AddMvcOptions(options =>
            {
                options.Conventions.Add(new RouteTokenTransformerConvention(
                    new HyphenControllerTransformer()));
                options.RespectBrowserAcceptHeader = true;
                options.ReturnHttpNotAcceptable = true;
            }).AddViewOptions(options =>
                options.HtmlHelperOptions.ClientValidationEnabled = false);
        }

        // This method gets called by the run-time. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,  ILoggerFactory logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var appPath = Configuration.GetSection("CompositeSettings:Path").Value;
            app.UseStaticFiles();
            app.UseHttpsRedirection();

            //app.UseExceptionHandler(errorApp =>
            //    errorApp.Run(async context =>
            //    {
            //        await ErrorService.LogException(context, logger);
            //        context.Response.Redirect(appPath + "/Error");
            //    }));



            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                /*
                endpoints.MapControllerRoute("action-plans", appPath + "/action-plans", new {controller = "home", action = "body"});
                endpoints.MapControllerRoute("viewGoal", appPath + "/view-goal", new {controller = "viewGoal", action = "body"});
                */
                endpoints.MapControllers();
            });

        }
    }
}
