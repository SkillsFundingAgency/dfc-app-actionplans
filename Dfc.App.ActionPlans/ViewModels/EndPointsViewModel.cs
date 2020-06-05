using System.Collections.Generic;
using DFC.App.ActionPlans.Models;

namespace DFC.App.ActionPlans.ViewModels
{
    public class EndPointsViewModel
    {
        public List<EndPoint> EndPoints { get; set; }

        public EndPointsViewModel()
        {
            EndPoints = new List<EndPoint>();
        }
    }
}
