using System.Threading.Tasks;
using DFC.App.ActionPlans.Services.DSS.Models;

namespace DFC.App.ActionPlans.Services.DSS.Interfaces
{
    public interface IDssWriter
    {
        Task UpdateActionPlan(UpdateActionPlan updateActionPlan);
        Task UpdateGoal(UpdateGoal updateGoal);
        Task UpdateAction(UpdateAction updateAction);
    }
}
