using DFC.App.ActionPlans.Models;
using NUnit.Framework;

namespace DFC.App.ActionPlans.UnitTests.ViewModels
{
    class ViewModelTests
    {
        class ErrorViewModelTests
        {
            [Test]
            public void ErrorViewModel()
            {
                var errorViewModel= new ErrorViewModel()
                {
                RequestId    = "id",
                };
            }
        }
    }
}
