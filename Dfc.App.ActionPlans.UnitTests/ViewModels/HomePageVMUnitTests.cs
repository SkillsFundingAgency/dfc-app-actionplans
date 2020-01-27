using Dfc.App.ActionPlans.ViewModels;
using FluentAssertions;
using NUnit.Framework;

namespace Dfc.App.ActionPlans.UnitTests.ViewModels
{
    [TestFixture()]
    public class HomePageVmUnitTests
    {
        [Test]
        public void When_Created_Then_IdIsSetCorrectly()
        {
            // Arrange.

            // Act.
            var mut = new HomePageVm();

            // Assert.
            mut.Id.Should().Be(PageVm.PageId.Home);
            mut.Id.Value.Should().Be("HOME");
        }
    }
}
