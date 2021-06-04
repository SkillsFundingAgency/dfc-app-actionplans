using AutoMapper;
using DFC.App.ActionPlans.Cosmos.Interfaces;
using DFC.App.ActionPlans.Cosmos.Models;
using DFC.App.ActionPlans.Cosmos.Services;
using DFC.App.ActionPlans.HostedServices;
using DFC.App.ActionPlans.Models;
using DFC.App.ActionPlans.Services.DSS.Interfaces;
using DFC.App.ActionPlans.Services.DSS.Models;
using DFC.App.ActionPlans.Services.DSS.Services;
using DFC.APP.ActionPlans.CacheContentService;
using DFC.APP.ActionPlans.Data.Contracts;
using DFC.APP.ActionPlans.Data.Models;
using DFC.Compui.Cosmos;
using DFC.Compui.Cosmos.Contracts;
using DFC.Compui.Subscriptions.Pkg.Netstandard.Extensions;
using DFC.Compui.Telemetry;
using DFC.Content.Pkg.Netcore.Data.Models.ClientOptions;
using DFC.Content.Pkg.Netcore.Data.Models.PollyOptions;
using DFC.Content.Pkg.Netcore.Extensions;
using DFC.Personalisation.Common.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;
using DFC.APP.Account.CacheContentService;


namespace Dfc.App.ActionPlans
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        private const string CosmosDbContentPagesConfigAppSettings = "Configuration:CosmosDbConnections:ActionPlans";
        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            this.env = env;

        }

        public void ConfigureServices(IServiceCollection services)
        {
            var cosmosDbConnectionContentPages = Configuration.GetSection(CosmosDbContentPagesConfigAppSettings).Get<CosmosDbConnection>();
            services.AddDocumentServices<CmsApiSharedContentModel>(cosmosDbConnectionContentPages, env.IsDevelopment());

            services.AddTransient<IEventMessageService<CmsApiSharedContentModel>, EventMessageService<CmsApiSharedContentModel>>();
            services.AddTransient<ICacheReloadService, CacheReloadService>();

            services.AddApplicationInsightsTelemetry();

            services.AddControllersWithViews();
            
            services.AddScoped<IDssReader, DssService>();
            services.AddScoped<IDssWriter, DssService>();

            services.AddAutoMapper(typeof(Startup).Assembly);
            services.AddTransient<IWebhooksService, WebhooksService>();
            services.AddTransient<IWebhookContentProcessor, WebhookContentProcessor>();
            services.Configure<DssSettings>(Configuration.GetSection(nameof(DssSettings)));
            services.Configure<CompositeSettings>(Configuration.GetSection(nameof(CompositeSettings)));
            services.Configure<CosmosSettings>(Configuration.GetSection(nameof(CosmosSettings)));

            services.AddSingleton((x) => new CosmosClient(
                accountEndpoint: Configuration.GetSection("CosmosSettings:ApiUrl").Value,
                authKeyOrResourceToken: Configuration.GetSection("CosmosSettings:ApiKey").Value));
            services.AddSingleton<ICosmosService, CosmosService>();

            services.Configure<AuthSettings>(Configuration.GetSection("AuthSettings"));
            var authSettings = new AuthSettings();
            var appPath = Configuration.GetSection("CompositeSettings:Path").Value;

            services.AddSingleton(Configuration.GetSection(nameof(CmsApiClientOptions)).Get<CmsApiClientOptions>() ?? new CmsApiClientOptions());
            services.AddHostedServiceTelemetryWrapper();
            services.AddSubscriptionBackgroundService(Configuration);
            services.AddHostedService<CacheReloadBackgroundService>();

            const string AppSettingsPolicies = "Policies";
            var policyOptions = Configuration.GetSection(AppSettingsPolicies).Get<PolicyOptions>() ?? new PolicyOptions();
            var policyRegistry = services.AddPolicyRegistry();
            services.AddApiServices(Configuration, policyRegistry);

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

            services.AddSession();
            services.AddMvc().AddMvcOptions(options =>
            {
                options.Conventions.Add(new RouteTokenTransformerConvention(
                    new HyphenControllerTransformer()));
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
