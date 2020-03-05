using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Muscle.Vending.Tests.CurrencyServiceFeatures
{
    public class ReturCoinsFeature:BaseCurrencyServiceFeature
    {
        [Fact]
        public void GivenSomeCoinsInserted_WhenCoinsRejected_ThenInsertedCoinsReturned_AndAvailableCoinsRemoved()
        {
            var coins = new List<ICoin>() {new Coin(USCoinTypes.Penny), new Coin(USCoinTypes.Penny)};
            foreach (var coin in coins)
            {
                _currencyService.InsertCoin(coin);

            }
            _currencyService.ReturnCoins().Should().Equal(coins);
            _currencyService.AvailableChange.Should().BeEmpty();
            _currencyService.InsertedCoins.Should().BeEmpty();

        }
        [Theory]
        [MemberData(nameof(CoinRejectCoins))]

        public void GivenSomeCoinsInsertedAndExistingAvailableChange_WhenCoinsRejected_ThenInsertedCoinsReturned_AndSomeAvailableCoinsRemoved(IList<ICoin>insertedCoins,IList<ICoin>availableCoins)
        {
          
            _currencyService.AvailableChange = availableCoins;
            foreach (var coin in insertedCoins)
            {
                _currencyService.InsertCoin(coin);

            }
            _currencyService.ReturnCoins().Should().Equal(insertedCoins);
            _currencyService.AvailableChange.Should().Equal(availableCoins);
            _currencyService.InsertedCoins.Should().BeEmpty();

        }
        
      

        public static IEnumerable<object[]> CoinRejectCoins =>
            new List<object[]>
            {
                new object[] { new List<ICoin>(){new Coin(USCoinTypes.Dime)},new List<ICoin>(){new Coin(USCoinTypes.Nickel)}},
                new object[] {new List<ICoin>(){new Coin(USCoinTypes.Dime)},new List<ICoin>(){new Coin(USCoinTypes.Dime)}},
                new object[] { new List<ICoin>(){new Coin(USCoinTypes.Dime)},new List<ICoin>(){new Coin(USCoinTypes.Quarter)}},
                new object[] { new List<ICoin>(){new Coin(USCoinTypes.Dime)},new List<ICoin>(){new Coin(USCoinTypes.Penny)}},
                new object[] { new List<ICoin>(){new Coin(USCoinTypes.Quarter),new Coin(USCoinTypes.Quarter)},new List<ICoin>(){new Coin(USCoinTypes.Penny),new Coin(USCoinTypes.Penny)}},
                new object[] { new List<ICoin>(){new Coin(USCoinTypes.Penny),new Coin(USCoinTypes.Quarter)},new List<ICoin>(){new Coin(USCoinTypes.Penny),new Coin(USCoinTypes.Penny),new Coin(USCoinTypes.Penny)}},
                new object[] { new List<ICoin>(){new Coin(USCoinTypes.Penny),new Coin(USCoinTypes.Quarter)},new List<ICoin>(){new Coin(USCoinTypes.Dime),new Coin(USCoinTypes.Nickel)}},
                new object[] { new List<ICoin>(){new Coin(USCoinTypes.Dime),new Coin(USCoinTypes.Quarter)},new List<ICoin>(){new Coin(USCoinTypes.Quarter),new Coin(USCoinTypes.Nickel)}},
                new object[] { new List<ICoin>(){new Coin(USCoinTypes.Nickel),new Coin(USCoinTypes.Quarter)},new List<ICoin>(){new Coin(USCoinTypes.Quarter),new Coin(USCoinTypes.Nickel),new Coin(USCoinTypes.Penny)}},
                new object[] {new List<ICoin>(){new Coin(USCoinTypes.Quarter),new Coin(USCoinTypes.Quarter)},new List<ICoin>(){new Coin(USCoinTypes.Quarter),new Coin(USCoinTypes.Dime)}},
                new object[] { new List<ICoin>(){new Coin(USCoinTypes.Quarter),new Coin(USCoinTypes.Quarter)},new List<ICoin>(){new Coin(USCoinTypes.Quarter),new Coin(USCoinTypes.Quarter)}}
            };

    }
}