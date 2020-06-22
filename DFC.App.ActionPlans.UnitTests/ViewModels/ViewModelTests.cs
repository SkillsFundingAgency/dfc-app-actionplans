using DFC.App.ActionPlans.Models;
using NUnit.Framework;

namespace DFC.App.ActionPlans.UnitTests.ViewModels
{
    class ViewModelTests
    {
        [Test]
        public void ErrorViewModel()
        {
            var errorViewModel = new ErrorViewModel() {RequestId = "id",};
        }
    }

    class ErrorCompositeViewModelTests
    {
        [Test]
        public void ErrorCompositeViewModelViewModel()
        {
            var errorViewModel = new ErrorCompositeViewModelTests();
        }
    }
    
}
