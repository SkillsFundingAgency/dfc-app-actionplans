using System;
using System.Collections.Generic;
using System.Text;
using DFC.App.ActionPlans.Exceptions;
using FluentAssertions;
using NUnit.Framework;
using DFC.App.ActionPlans.Helpers;
using DFC.App.ActionPlans.Models;

namespace DFC.App.ActionPlans.UnitTests.Helpers
{
    class ValidateTests
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
}
