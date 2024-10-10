﻿using DFC.App.ActionPlans.Cosmos.Interfaces;
using DFC.App.ActionPlans.Cosmos.Models;
using DFC.App.ActionPlans.Cosmos.Services;
using DFC.App.ActionPlans.Models;
using DFC.App.ActionPlans.Services.DSS.Interfaces;
using DFC.App.ActionPlans.Services.DSS.Models;
using DFC.App.ActionPlans.Services.DSS.Services;
using DFC.Common.SharedContent.Pkg.Netcore;
using DFC.Common.SharedContent.Pkg.Netcore.Infrastructure;
using DFC.Common.SharedContent.Pkg.Netcore.Infrastructure.Strategy;
using DFC.Common.SharedContent.Pkg.Netcore.Interfaces;
using DFC.Common.SharedContent.Pkg.Netcore.Model.ContentItems.SharedHtml;
using DFC.Common.SharedContent.Pkg.Netcore.RequestHandler;
using DFC.Compui.Cosmos.Contracts;
using DFC.Compui.Subscriptions.Pkg.Netstandard.Extensions;
using DFC.Compui.Telemetry;
using DFC.Content.Pkg.Netcore.Data.Models.ClientOptions;
using DFC.Content.Pkg.Netcore.Data.Models.PollyOptions;
using DFC.Content.Pkg.Netcore.Extensions;
using DFC.Personalisation.Common.Helpers;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading;
using DFC.Common.SharedContent.Pkg.Netcore.Constant;
using Microsoft.Extensions.Caching.Memory;

namespace Dfc.App.ActionPlans
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        private const string RedisCacheConnectionStringAppSettings = "Cms:RedisCacheConnectionString";
        private const string GraphApiUrlAppSettings = "Cms:GraphApiUrl";
        private const string WorkerThreadsConfigAppSettings = "ThreadSettings:WorkerThreads";
        private const string IocpThreadsConfigAppSettings = "ThreadSettings:IocpThreads";
        private const string CosmosDbContentPagesConfigAppSettings = "Configuration:CosmosDbConnections:Account";

        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment env;
        private readonly ILogger<Startup> logger;

        public Startup(IConfiguration configuration, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            Configuration = configuration;
            this.env = env;
            this.logger = logger;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureMinimumThreads();
            var cosmosDbConnectionContentPages = Configuration.GetSection(CosmosDbContentPagesConfigAppSettings).Get<CosmosDbConnection>();
            var cosmosRetryOptions = new RetryOptions { MaxRetryAttemptsOnThrottledRequests = 20, MaxRetryWaitTimeInSeconds = 60 };

            services.AddApplicationInsightsTelemetry();

            services.AddControllersWithViews();

            services.AddScoped<IDssReader, DssService>();
            services.AddScoped<IDssWriter, DssService>();
            services.AddAutoMapper(typeof(Startup).Assembly);

            services.Configure<DssSettings>(Configuration.GetSection(nameof(DssSettings)));
            services.Configure<CompositeSettings>(Configuration.GetSection(nameof(CompositeSettings)));
            services.Configure<CosmosSettings>(Configuration.GetSection(nameof(CosmosSettings)));
            services.Configure<ActionPlanRetirement>(Configuration.GetSection(nameof(ActionPlanRetirement)));

            services.AddStackExchangeRedisCache(options => { options.Configuration = Configuration.GetSection(RedisCacheConnectionStringAppSettings).Get<string>(); });

            services.AddHttpClient();
            services.AddSingleton<IGraphQLClient>(s =>
            {
                var option = new GraphQLHttpClientOptions()
                {
                    EndPoint = new Uri(Configuration[ConfigKeys.GraphApiUrl] ??
                throw new ArgumentNullException($"{nameof(ConfigKeys.GraphApiUrl)} is missing or has an invalid value.")),
                    HttpMessageHandler = new CmsRequestHandler(
                        s.GetService<IHttpClientFactory>(),
                        s.GetService<IConfiguration>(),
                        s.GetService<IHttpContextAccessor>(),
                        s.GetService<IMemoryCache>()),
                };
                var client = new GraphQLHttpClient(option, new NewtonsoftJsonSerializer());
                return client;
            });


            services.AddSingleton<ISharedContentRedisInterfaceStrategyWithRedisExpiry<SharedHtml>, SharedHtmlQueryStrategy>();
            services.AddSingleton<ISharedContentRedisInterfaceStrategyFactory, SharedContentRedisStrategyFactory>();
            services.AddScoped<ISharedContentRedisInterface, SharedContentRedis>();

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
                            var requestingUrl = context.Request.Path.ToString().Replace("/head", appPath);
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

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var appPath = Configuration.GetSection("CompositeSettings:Path").Value;
            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }

        private void ConfigureMinimumThreads()
        {
            var workerThreads = Convert.ToInt32(Configuration[WorkerThreadsConfigAppSettings]);

            var iocpThreads = Convert.ToInt32(Configuration[IocpThreadsConfigAppSettings]);

            if (ThreadPool.SetMinThreads(workerThreads, iocpThreads))
            {
                logger.LogInformation(
                    "ConfigureMinimumThreads: Minimum configuration value set. IOCP = {0} and WORKER threads = {1}",
                    iocpThreads,
                    workerThreads);
            }
            else
            {
                logger.LogWarning("ConfigureMinimumThreads: The minimum number of threads was not changed");
            }
        }
    }
}
