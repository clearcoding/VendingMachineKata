using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Muscle.Vending.Currency;
using Xunit;

namespace Muscle.Vending.Tests
{
    public class CurrencyServiceTests
    {
        private UsCurrencyService _currencyService;

        public CurrencyServiceTests()
        {
            _currencyService  = new UsCurrencyService();
        }
        [Fact]
        public void GivenACoin_WhenRecognisedAsDime_ThenReturnTrue()
        {
            var dime  = new Coin(USCoinTypes.Dime);
            var result = _currencyService.IsAccepted(dime);
            result.Should().BeTrue();
            
        }

        [Fact]
        public void GivenACoin_WhenNotRecognised_TheReturnFalse()
        {
            var penny  = new Coin(USCoinTypes.Penny);
            var result = _currencyService.IsAccepted(penny);
            result.Should().BeFalse();
        }

        
        [Theory]
        [MemberData(nameof(ChangeCalculationData))]
        public void GivenATargetSum_WhenChangeIsCalculated_ThenCorrectCoinsReturned(decimal cost,List<ICoin> expectedChange)
        {
            var change  = _currencyService.CalculcateChangeCoins(cost);
            change.Should().BeEquivalentTo(expectedChange);
        }
        public static IEnumerable<object[]> ChangeCalculationData =>
            new List<object[]>
            {
                new object[] { 0.05m,new List<ICoin>(){new Coin(USCoinTypes.Nickel)}},
                new object[] { 0.10m,new List<ICoin>(){new Coin(USCoinTypes.Dime)}},
                new object[] { 0.25m,new List<ICoin>(){new Coin(USCoinTypes.Quarter)}},
                new object[] { 0.01m,new List<ICoin>(){new Coin(USCoinTypes.Penny)}},
                new object[] { 0.02m,new List<ICoin>(){new Coin(USCoinTypes.Penny),new Coin(USCoinTypes.Penny)}},
                new object[] { 0.03m,new List<ICoin>(){new Coin(USCoinTypes.Penny),new Coin(USCoinTypes.Penny),new Coin(USCoinTypes.Penny)}},
                new object[] { 0.15m,new List<ICoin>(){new Coin(USCoinTypes.Dime),new Coin(USCoinTypes.Nickel)}},
                new object[] { 0.30m,new List<ICoin>(){new Coin(USCoinTypes.Quarter),new Coin(USCoinTypes.Nickel)}},
                new object[] { 0.31m,new List<ICoin>(){new Coin(USCoinTypes.Quarter),new Coin(USCoinTypes.Nickel),new Coin(USCoinTypes.Penny)}},
                new object[] { 0.35m,new List<ICoin>(){new Coin(USCoinTypes.Quarter),new Coin(USCoinTypes.Dime)}},
                new object[] { 0.50m,new List<ICoin>(){new Coin(USCoinTypes.Quarter),new Coin(USCoinTypes.Quarter)}}
            };
    }
}