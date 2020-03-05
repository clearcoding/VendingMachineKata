using System.Collections.Generic;
using FluentAssertions;
using Muscle.Vending.Display;
using Xunit;

namespace Muscle.Vending.Tests.VendingMachineTests
{
    public class ExactChangeFeature : BaseVendingMachineFeature
    {
        [Fact]
        public void GivenThereIsNoChangeInTheMachine_ThenTheDisplayMessageShouldBeExactMoneyOnly()
        {
            _mockUsCurrencyService.SetupGet(s => s.AvailableChange).Returns(new List<ICoin>());
            _vendingMachine.CurrentDisplay.Should().Be(VendingResponse.ExactChangeOnly);
        }
    }
}