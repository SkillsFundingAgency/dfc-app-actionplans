﻿using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using DFC.APP.ActionPlans.Data;
using DFC.APP.ActionPlans.Data.Common;
using DFC.APP.ActionPlans.Data.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DFC.APP.ActionPlans.CacheContentService
{
    public class WebhooksService : IWebhooksService
    {
        private readonly ILogger<WebhooksService> logger;
        private readonly IWebhookContentProcessor webhookContentProcessor;
        private readonly Guid _sharedContentId;

        public WebhooksService(
            ILogger<WebhooksService> logger,
            IWebhookContentProcessor webhookContentProcessor,
            IConfiguration config)
        {
            this.logger = logger;
            this.webhookContentProcessor = webhookContentProcessor;
            _sharedContentId = config.GetValue<Guid>(Constants.SharedContentGuidConfig);
        }

        public async Task<HttpStatusCode> ProcessMessageAsync(WebhookCacheOperation webhookCacheOperation, Guid eventId, Guid contentId, string apiEndpoint)
        {
            if (_sharedContentId != contentId)
            {
                logger.LogInformation($"Event Id: {eventId}, is not a shared content item we are subscribed to, so no content has been processed");
                return HttpStatusCode.OK;
            }

            switch (webhookCacheOperation)
            {
                case WebhookCacheOperation.Delete:
                    return await webhookContentProcessor.DeleteContentAsync(contentId).ConfigureAwait(false);


                case WebhookCacheOperation.CreateOrUpdate:

                    if (!Uri.TryCreate(apiEndpoint, UriKind.Absolute, out Uri? url))
                    {
                        throw new InvalidDataException($"Invalid Api url '{apiEndpoint}' received for Event Id: {eventId}");
                    }

                    return await webhookContentProcessor.ProcessContentAsync(url, contentId).ConfigureAwait(false);


                default:
                    logger.LogError($"Event Id: {eventId} got unknown cache operation - {webhookCacheOperation}");
                    return HttpStatusCode.BadRequest;
            }
        }
    }
}
