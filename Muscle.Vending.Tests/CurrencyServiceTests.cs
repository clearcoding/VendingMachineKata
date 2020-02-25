using System.Collections.Generic;
using System.Globalization;
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
            var change  = _currencyService.CalculateChangeCoins(cost);
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