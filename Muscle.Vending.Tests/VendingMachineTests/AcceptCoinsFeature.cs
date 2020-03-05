using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using Muscle.Vending.Currency;
using Muscle.Vending.Display;
using Muscle.Vending.Products;
using Xunit;

namespace Muscle.Vending.Tests.VendingMachineTests
{
    public class AcceptCoinsFeature:BaseVendingMachineFeature
    {
       
        [Fact]
        public void GivenNoCoinsInserted_WhenViewed_ThenCurrentDisplayShowsInsertCoin()
        {
            _vendingMachine.CurrentDisplay.Should().Be(VendingResponse.InsertedCoin);
        }
        [Fact]
        public void GivenAValidCoin_WhenItsInserted_ThenTheCurrentDisplayIsAccepted(){
            
            var coin = new Coin(USCoinTypes.Dime);
            _mockUsCurrencyService.Setup(s => s.IsAccepted(It.IsAny<ICoin>())).Returns(true);
            _vendingMachine.InsertCoin(coin);    
            _vendingMachine.CurrentDisplay.Should().Be(VendingResponse.Accepted);
        }

        [Fact]
        public void GivenAValidCoin_WhenInserted_TheCurrentServiceInsertCoinIsPassedSameCoin()
        {
            var coin = new Coin(USCoinTypes.Dime);
            _mockUsCurrencyService.Setup(s => s.IsAccepted(It.IsAny<ICoin>())).Returns(true);
            _mockUsCurrencyService.Setup(s => s.InsertCoin(It.IsAny<ICoin>()));
            _vendingMachine.InsertCoin(coin);
            _mockUsCurrencyService.Verify(v=>v.InsertCoin(coin));

        }
        
        
        [Fact] 
        public void GivenAValidCoin_WhenItIsInserted_ThenTheDisplayedCurrentAmountIsCorrect()
        {
            var coin = new Coin(USCoinTypes.Dime);
            _mockUsCurrencyService.Setup(s => s.IsAccepted(It.IsAny<ICoin>())).Returns(true);
            _mockUsCurrencyService.SetupGet(s => s.InsertedCoins).Returns(new List<ICoin>(){coin});
            _vendingMachine.CurrentAmount.Should().Be(coin.Value);

        }

        [Fact] 
        public void GivenAnInvalidCoin_WhenItIsInserted_ThenTheCurrentDisplayIsInsertCoin()
        {
            var coin = new Coin(USCoinTypes.Penny);
            _mockUsCurrencyService.Setup(s => s.IsAccepted(It.IsAny<ICoin>())).Returns(false);
            _vendingMachine.InsertCoin(coin);
            Assert.Equal(VendingResponse.InsertedCoin,_vendingMachine.CurrentDisplay);
        }
        [Fact] 
        public void GivenAnInvalidCoin_WhenItIsInserted_ThenTheCoinIsReturned()
        {
            var coin = new Coin(USCoinTypes.Penny);
            _mockUsCurrencyService.Setup(s => s.IsAccepted(It.IsAny<ICoin>())).Returns(false);
            _vendingMachine.InsertCoin(coin); 
            
            Assert.Same(coin, _vendingMachine.ReturnSlot.Single());
        }

    }
}