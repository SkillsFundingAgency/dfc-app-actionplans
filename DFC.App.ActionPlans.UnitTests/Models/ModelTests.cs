using System;
using DFC.App.ActionPlans.Models;
using DFC.App.ActionPlans.Services.DSS.Enums;
using DFC.App.ActionPlans.Services.DSS.Models;
using DFC.App.ActionPlans.ViewModels;
using NUnit.Framework;

namespace DFC.App.ActionPlans.UnitTests
{
    class ModelTests
    {
        class CompositeSettingsModelTests
        {
            [Test]
            public void CompositeSettingsModel()
            {
                var compositeSettings = new CompositeSettings();
                compositeSettings.Path = "SomePath";
                compositeSettings.Cdn = "SomeCDN";
            }
        }

        class GoalModelTests
        {
            [Test]
            public void GoalModel()
            {
                var goal = new Goal()
                {
                    ActionPlanId = "Id",
                    CustomerId = "cId",
                    DateGoalAchieved = DateTime.Now,
                    DateGoalCaptured = DateTime.Now,
                    DateGoalShouldBeCompletedBy = DateTime.Now,
                    GoalId = "gId",
                    GoalStatus = GoalStatus.Achieved,
                    GoalSummary = "summary",
                    GoalType=GoalType.Learning,
                    LastModifiedBy = "me",
                    LastModifiedDate = DateTime.Now,
                    SubcontractorId = "sId"
                };
                
            }
        }

        class AuthSettingsModelTests
        {
            [Test]
            public void AuthSettingsModel()
            {
                var authSettings = new AuthSettings()
                {
                    ClientId = "id",
                    ClientSecret = "secret",
                    Issuer = "isuser",
                    RegisterUrl = "url",
                    ResetPasswordUrl = "url",
                    SignInUrl = "url",
                    SignOutUrl = "url"
                };

            }
        }

        class ContactDetailsModelTests
        {
            [Test]
            public void ContactDetailsModel()
            {
                var contactDetails = new ContactDetails
                {
                    Phone = null,
                    WebchatLink = null,
                    ContactDaysTime = null
                };
                var phone = contactDetails.Phone;
                var webchatLink = contactDetails.WebchatLink;
                var contactDaysTime = contactDetails.ContactDaysTime;
            }
        }

        class CompositeViewModelTests
        {
            [Test]
            public void CompositeViewModelModel()
            {
                var homeCompositeModel = new HomeCompositeViewModel();
                homeCompositeModel.ToString();
                homeCompositeModel.GetElementId("someElement", "home");
            }
        }
    }
}
