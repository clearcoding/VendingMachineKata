using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Muscle.Vending.Tests.CurrencyServiceFeatures
{
    public class MakeChangeFeature : BaseCurrencyServiceFeature
    {
        [Theory]
        [MemberData(nameof(ChangeCalculationData))]
        public void GivenATargetSum_WhenChangeIsCalculated_ThenCorrectCoinsReturned(decimal cost,
            List<ICoin> expectedChange)
        {
            var change = _currencyService.CalculateChangeCoins(cost);
            change.Should().BeEquivalentTo(expectedChange);
        }

        [Fact]
        public void GivenSomeCoins_WhenInserted_ThenAvailableChangeWillIncrease_AndInsertedCoinsWillIncrease()
        {
            var coins = new List<ICoin>() {new Coin(USCoinTypes.Penny), new Coin(USCoinTypes.Penny)};
            foreach (var coin in coins)
            {
                _currencyService.InsertCoin(coin);
            }

            _currencyService.AvailableChange.Should().Equal(coins);
            _currencyService.InsertedCoins.Should().Equal(coins);
        }

        public static IEnumerable<object[]> ChangeCalculationData =>
            new List<object[]>
            {
                new object[] {0.05m, new List<ICoin>() {new Coin(USCoinTypes.Nickel)}},
                new object[] {0.10m, new List<ICoin>() {new Coin(USCoinTypes.Dime)}},
                new object[] {0.25m, new List<ICoin>() {new Coin(USCoinTypes.Quarter)}},
                new object[] {0.01m, new List<ICoin>() {new Coin(USCoinTypes.Penny)}},
                new object[] {0.02m, new List<ICoin>() {new Coin(USCoinTypes.Penny), new Coin(USCoinTypes.Penny)}},
                new object[]
                {
                    0.03m,
                    new List<ICoin>()
                        {new Coin(USCoinTypes.Penny), new Coin(USCoinTypes.Penny), new Coin(USCoinTypes.Penny)}
                },
                new object[] {0.15m, new List<ICoin>() {new Coin(USCoinTypes.Dime), new Coin(USCoinTypes.Nickel)}},
                new object[] {0.30m, new List<ICoin>() {new Coin(USCoinTypes.Quarter), new Coin(USCoinTypes.Nickel)}},
                new object[]
                {
                    0.31m,
                    new List<ICoin>()
                        {new Coin(USCoinTypes.Quarter), new Coin(USCoinTypes.Nickel), new Coin(USCoinTypes.Penny)}
                },
                new object[] {0.35m, new List<ICoin>() {new Coin(USCoinTypes.Quarter), new Coin(USCoinTypes.Dime)}},
                new object[] {0.50m, new List<ICoin>() {new Coin(USCoinTypes.Quarter), new Coin(USCoinTypes.Quarter)}}
            };
    }
}