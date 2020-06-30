﻿using System;
using DFC.App.ActionPlans.Extensions;
using FluentAssertions;
using NUnit.Framework;
using DFC.App.ActionPlans.Helpers;
using DFC.App.ActionPlans.Models;

namespace DFC.App.ActionPlans.UnitTests.Extensions
{
    public static class DateExtension
    {
        [Test]
        public static void When_DateOnly_Then_OnlyDateIsReturned()
        {
            DateTime dateValue = new DateTime();
            var result = dateValue.DateOnly();
            DateTime.Parse(result).TimeOfDay.TotalSeconds.Should().Be(0);
        }

    }
}
