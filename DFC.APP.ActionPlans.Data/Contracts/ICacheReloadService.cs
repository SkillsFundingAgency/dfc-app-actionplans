using DFC.APP.ActionPlans.Data.Models;
using System.Threading;
using System.Threading.Tasks;

namespace DFC.APP.ActionPlans.Data.Contracts
{
    public interface ICacheReloadService
    {
        Task Reload(CancellationToken stoppingToken);

        Task<CmsApiSharedContentModel>? GetSharedContentAsync();

        Task ProcessSummaryListAsync(CmsApiSharedContentModel sharedContent, CancellationToken stoppingToken);

        Task GetAndSaveItemAsync(CmsApiSharedContentModel item, CancellationToken stoppingToken);
    }
}