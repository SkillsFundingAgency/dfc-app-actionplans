using System.Collections.Generic;
using System.Threading.Tasks;
using DFC.App.ActionPlans.Models;
using DFC.App.ActionPlans.Services.DSS.Models;

namespace DFC.App.ActionPlans.Services.DSS.Interfaces
{
    public interface IDssReader
    {
        Task<Customer> GetCustomerDetails(string customerId);
        Task<List<Session>> GetSessions(string customerId, string interactionId);
        Task<List<Goal>> GetGoals(string customerId, string interactionId, string actionPlanId);
        Task<List<Action>> GetActions(string customerId, string interactionId, string actionPlanId);
        Task<Interaction> GetInteractionDetails(string customerId, string interactionId);
        Task<Adviser> GetAdviserDetails(string adviserId);
        Task<ActionPlan> GetActionPlanDetails(string customerId, string interactionId, string actionPlanId);
        Task<Goal> GetGoalDetails(string customerId, string interactionId, string actionPlanId, string goalId);
    }
}
