using Dfc.App.ActionPlans.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace Dfc.App.ActionPlans.UnitTests.Controllers
{
    [TestFixture]
    public class HomeControllerUnitTests
    {
        [TestFixture]
        public class Index
        {
            [Test]
            public void When_GET_Then_ReturnView()
            {
                // Arrange
                var cut = new HomeController();

                // Act
                var result = cut.Index();

                // Assert
                result.Should().BeOfType<ViewResult>();
            }
        }
    }
}
