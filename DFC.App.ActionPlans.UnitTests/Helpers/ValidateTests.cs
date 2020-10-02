using System;
using FluentAssertions;
using NUnit.Framework;
using DFC.App.ActionPlans.Helpers;
using DFC.App.ActionPlans.Models;
using DFC.App.ActionPlans.Services.DSS.Enums;

namespace DFC.App.ActionPlans.UnitTests.Helpers
{
    class DateExtension
    {
        [Test]
        public void WhenCheckValidSplitDateCalledWithInvalidDate_Then_ReturnFalse()
        {
            DateTime dateValue;
            var result = Validate.CheckValidSplitDate(new SplitDate()
            {
                Day="50",
                Month = "12", 
                Year = "2000"
            }, out dateValue);
            result.Should().BeFalse();

        }

        [Test]
        public void WhenCheckValidDueDateCalledWithInvalidDate_Then_ReturnFalse()
        {
            DateTime dateValue;
            var result = Validate.CheckValidDueDate(new SplitDate()
            {
                Day="50",
                Month = "12", 
                Year = "2000"
            }, out dateValue);
            result.Should().BeFalse();

        }
    }

    class Utility
    {
        [Test]
        public void When_GetActionHelperTextWithUrl_Then_ReturnAnchor()
        {
            
            var result = ActionPlans.Helpers.Utility.GetActionHelperText(SignpostedToCategory.ApprenticeshipService,"http://www.microsoft.com");
            result.Should().Contain("<a");

        }

        [Test]
        public void When_GetActionHelperTextWithNoUrl_Then_ReturnNoAnchor()
        {
            
            var result = ActionPlans.Helpers.Utility.GetActionHelperText(SignpostedToCategory.ApprenticeshipService,"Help Text");
            result.Should().NotContain("<a");

        }
    }

}
