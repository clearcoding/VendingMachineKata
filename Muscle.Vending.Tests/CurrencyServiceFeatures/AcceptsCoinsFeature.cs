using FluentAssertions;
using Muscle.Vending.Currency;
using Xunit;

namespace Muscle.Vending.Tests.CurrencyServiceFeatures
{
    public class AcceptsCoinsFeature
    {
        private UsCurrencyService _currencyService;

        public AcceptsCoinsFeature()
        {
            _currencyService = new UsCurrencyService();
        }

        [Fact]
        public void GivenACoin_WhenRecognisedAsDime_ThenReturnTrue()
        {
            var dime = new Coin(USCoinTypes.Dime);
            var result = _currencyService.IsAccepted(dime);
            result.Should().BeTrue();
        }

        [Fact]
        public void GivenACoin_WhenNotRecognised_TheReturnFalse()
        {
            var penny = new Coin(USCoinTypes.Penny);
            var result = _currencyService.IsAccepted(penny);
            result.Should().BeFalse();
        }
    }
}