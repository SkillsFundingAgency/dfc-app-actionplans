// <copyright file="BasicSteps.cs" company="National Careers Service">
// Copyright (c) National Careers Service. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using DFC.App.ActionPlans.Model;
using DFC.TestAutomation.UI.Extension;
using OpenQA.Selenium;
using System.Globalization;
using System.Linq;
using TechTalk.SpecFlow;

namespace DFC.App.ActionPlans.UI.FunctionalTests.StepDefinitions
{
    [Binding]
    internal class BasicSteps
    {
        public BasicSteps(ScenarioContext context)
        {
            this.Context = context;
        }

        private ScenarioContext Context { get; set; }

        [When(@"I click the (.*) button")]
        public void WhenIClickTheButton(string buttonText)
        {
            var allbuttons = this.Context.GetWebDriver().FindElements(By.ClassName("govuk-button")).ToList();

            foreach (var button in allbuttons)
            {
                if (button.Text.Trim().Equals(buttonText, System.StringComparison.OrdinalIgnoreCase))
                {
                    button.Click();
                    return;
                }
            }

            throw new NotFoundException($"Unable to perform the step: {this.Context.StepContext.StepInfo.Text}. The button could not be found.");
        }

        [When(@"I click the (.*) link to update (.*)")]
        [When(@"I click the (.*) link to view my (.*)")]
        public void WhenIClickTheLink(string linkText, string pageName)
        {
            var allliinks = this.Context.GetWebDriver().FindElements(By.LinkText(linkText)).ToList();

            switch (pageName.ToLower(CultureInfo.CurrentCulture))
            {
                case "action":
                    allliinks[allliinks.Count - 1].Click();
                    break;

                case "action plan":
                case "goal":
                case "due date":
                case "status":
                    allliinks[0].Click();
                    break;

                default:
                    throw new NotFoundException($"Unable to perform the step: {this.Context.StepContext.StepInfo.Text}. View link not found");
            }
        }
    }
}