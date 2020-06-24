using DFC.App.ActionPlans.Models;
using DFC.App.ActionPlans.ViewModels;
using FluentAssertions;
using NUnit.Framework;

namespace DFC.App.ActionPlans.UnitTests.ViewModels
{
    class ViewModelTests
    {
        [Test]
        public void When_RequestIdProvided_Then_ShowRequestIdIsTrue()
        {
            var errorViewModel = new ErrorViewModel() {RequestId = "id"};
            var  showRequestId = errorViewModel.ShowRequestId;
            showRequestId.Should().BeTrue();
        }

        [Test]
        public void When_RequestIdNotProvided_Then_ShowRequestIdIsFalse()
        {
            var errorViewModel = new ErrorViewModel() {RequestId = ""};
            var  showRequestId = errorViewModel.ShowRequestId;
            showRequestId.Should().BeFalse();
        }

        [Test]
        public void When_CompositeViewModelContactDetailsUsed_Return_CotactDetails()
        {
            ViewActionCompositeViewModel model = new ViewActionCompositeViewModel();
            model.ContactDetails = new ContactDetails();
            ContactDetails cd = model.ContactDetails;
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
