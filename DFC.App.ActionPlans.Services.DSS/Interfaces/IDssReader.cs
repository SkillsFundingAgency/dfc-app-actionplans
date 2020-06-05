using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using DFC.App.ActionPlans.Services.DSS.Models;

namespace DFC.App.ActionPlans.Services.DSS.Interfaces
{
    public interface IDssReader
    {
        Task<Customer> GetCustomerDetails(string customerId);

    }
}
