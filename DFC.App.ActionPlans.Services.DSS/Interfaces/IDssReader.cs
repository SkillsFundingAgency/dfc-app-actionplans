using System.Collections.Generic;
using System.Threading.Tasks;
using DFC.App.ActionPlans.Models;
using DFC.App.ActionPlans.Services.DSS.Models;

namespace DFC.App.ActionPlans.Services.DSS.Interfaces
{
    public interface IDssReader
    {
        Task<Customer> GetCustomerDetails(string customerId);
        Task<IList<Session>> GetSessions(string customerId, string interactionId);
        Task<IList<Goal>> GetGoals(string customerId, string interactionId, string actionPlanId);
        Task<IList<Action>> GetActions(string customerId, string interactionId, string actionPlanId);
    }
}
