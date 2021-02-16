using System;
using System.Net;
using System.Threading.Tasks;

namespace DFC.APP.ActionPlans.Data.Contracts
{
    public interface IWebhooksService
    {
        Task<HttpStatusCode> ProcessMessageAsync(WebhookCacheOperation webhookCacheOperation, Guid eventId, Guid contentId, string apiEndpoint);
    }
}
