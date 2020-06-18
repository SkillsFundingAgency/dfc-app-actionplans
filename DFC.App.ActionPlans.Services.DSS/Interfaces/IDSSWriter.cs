using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DFC.App.ActionPlans.Services.DSS.Models;

namespace DFC.App.ActionPlans.Services.DSS.Interfaces
{
    public interface IDssWriter
    {
        Task UpdateActionPlan(UpdateActionPlan updateActionPlan);
    }
}
