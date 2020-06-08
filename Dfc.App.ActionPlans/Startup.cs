using System.Diagnostics.CodeAnalysis;
using DFC.App.ActionPlans.Services.DSS.Interfaces;
using DFC.App.ActionPlans.Models;
using DFC.App.ActionPlans.Services;
using DFC.App.ActionPlans.Services.DSS.Models;
using DFC.App.ActionPlans.Services.DSS.Services;
using DFC.Personalisation.Common.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


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
            services.AddApplicationInsightsTelemetry();

            services.AddControllersWithViews();
            services.AddScoped<IDssReader, DssService>();
            
            services.Configure<DssSettings>(Configuration.GetSection(nameof(DssSettings)));
            services.Configure<CompositeSettings>(Configuration.GetSection(nameof(CompositeSettings)));

            services.Configure<AuthSettings>(Configuration.GetSection("AuthSettings"));
            var authSettings = new AuthSettings();
            var appPath = Configuration.GetSection("CompositeSettings:Path").Value;

            Configuration.GetSection("AuthSettings").Bind(authSettings);
          /*  services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
                                context.Response.Redirect("/auth/signin");
                            }
                            return Task.CompletedTask;
                            
                            
                        },
                       OnChallenge = context =>
                        {
                            context.Response.Redirect("/auth/signin");
                            context.HandleResponse();
                            return Task.CompletedTask;
                        }
                        

                    };
                });
          */
            services.AddSession();
            services.AddMvc().AddMvcOptions(options =>
            {
                options.Conventions.Add(new RouteTokenTransformerConvention(
                    new HyphenControllerTransformer()));
            }).AddViewOptions(options =>
                options.HtmlHelperOptions.ClientValidationEnabled = false);
        }

        // This method gets called by the run-time. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,  ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var appPath = Configuration.GetSection("CompositeSettings:Path").Value;
            app.UseStaticFiles();
            app.UseSession();  
            app.UseHttpsRedirection();

            app.UseExceptionHandler(errorApp =>
                errorApp.Run(async context =>
                {
                    await ErrorService.LogException(context, logger);
                    context.Response.Redirect(appPath + "/Error");
                }));



            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("yourDetails", appPath + "/your-details", new {controller = "yourDetails", action = "body"});
                
                endpoints.MapControllerRoute("closeAccount", appPath + "/close-your-account", new {controller = "closeYourAccount", action = "body"});
                endpoints.MapControllerRoute("closeAccountBody", "/body/close-your-account", new {controller = "closeYourAccount", action = "body"});
                
                
                endpoints.MapControllerRoute("editDetails", appPath + "/edit-your-details", new {controller = "editYourDetails", action = "body"});
                endpoints.MapControllerRoute("changePassword", appPath + "/change-password", new {controller = "changePassword", action = "body"});
                
                endpoints.MapControllerRoute("deleteAccount", appPath + "/delete-account", new {controller = "deleteAccount", action = "body"});

                endpoints.MapControllerRoute("confirmDelete", appPath + "/confirm-delete", new { controller = "confirmDelete", action = "body" });
                endpoints.MapControllerRoute("confirmDeleteBody", "/body/confirm-delete", new { controller = "confirmDelete", action = "body" });

                endpoints.MapControllerRoute("shcDeleted", appPath + "/shc-deleted", new { controller = "shcDeleted", action = "body" });
                endpoints.MapControllerRoute("shcDeletedBody", "/body/shc-deleted", new { controller = "shcDeleted", action = "body" });
                
                endpoints.MapControllers();
            });

        }
    }
}
