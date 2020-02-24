using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Globalization;
using System.Linq;
using FluentAssertions;
using Moq;
using Muscle.Vending.Coins;
using Muscle.Vending.Currency;
using Muscle.Vending.Products;
using Xunit;

namespace Muscle.Vending.Tests
{
    public class VendingMachineTests
    {
        private readonly Mock<ICurrencyService> _mockUsCurrencyService;
        private static readonly ICoin Dime = new Coin(USCoinTypes.Dime);
        private Mock<IProductRepository> _mockProductRepo;
        private VendingMachine _vendingMachine;
        public VendingMachineTests()
        {
             _mockUsCurrencyService = new Mock<ICurrencyService>();
             _mockProductRepo = new Mock<IProductRepository>();

             var products = new List<Product>()
             {
                 new Product(){Name = "Cola"}
             };
             _mockProductRepo.SetupGet(p => p.Products).Returns(products);
             _mockUsCurrencyService.SetupGet(s => s.AvailableChange).Returns(new List<ICoin>());

             _vendingMachine  = new VendingMachine(_mockUsCurrencyService.Object,_mockProductRepo.Object);

        }

        [Fact]
        public void GivenNoCoinsInserted_WhenViewed_ThenCurrentDisplayIsInsertCoin()
        {
            Assert.Equal(VendingResponse.InsertedCoin,_vendingMachine.CurrentDisplay);
        }

        [Fact]
        public void GivenAValidCoin_WhenItsInserted_ThenTheCurrentDisplayIsAccepted(){
            
            var coin = new Coin(USCoinTypes.Dime);
            _mockUsCurrencyService.Setup(s => s.IsAccepted(It.IsAny<ICoin>())).Returns(true);
            _vendingMachine.InsertCoin(coin);    
            Assert.Equal(VendingResponse.Accepted,_vendingMachine.CurrentDisplay);
        }
        [Fact] 
        public void GivenAValidCoin_WhenItIsInserted_ThenTheDisplayedCurrentAmountIsCorrect()
        {
            var coin = new Coin(USCoinTypes.Dime);
            _mockUsCurrencyService.Setup(s => s.IsAccepted(It.IsAny<ICoin>())).Returns(true);
            _vendingMachine.InsertCoin(coin); 
            Assert.Equal(coin.Value,_vendingMachine.CurrentAmount);
   
        }


        [Fact] 
        public void GivenAnInvalidCoin_WhenItIsInserted_ThenTheCurrentDisplayIsInserCoin()
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

        [Fact]
        public void GivenTheUserHasInsertedTheCorrectMoney_WhenTheySelectProduct_ThenTheProductIsDispensedAndThankYouDisplayed()
        {
            
            IList<Product> products = new List<Product>(){new Product(){Name = ProductTypes.Cola,Price = 0.25m,AvailableStock = 1}};
            _mockUsCurrencyService.Setup(s => s.IsAccepted(It.IsAny<ICoin>())).Returns(true);
            _mockProductRepo.Setup(s => s.Products).Returns(products);
            
            _vendingMachine.InsertCoin(new Coin(USCoinTypes.Quarter));
            _vendingMachine.BuyProduct(ProductTypes.Cola);
            
            _vendingMachine.DispensedProduct.Name.Should().Be(ProductTypes.Cola);
            _vendingMachine.CurrentDisplay.Should().Be(VendingResponse.ThankYou);
            _vendingMachine.CurrentDisplay.Should().Be(VendingResponse.InsertedCoin);

        }
        [Fact]
        public void GivenTheUserHasNotInsertedTheCorrectMoney_WhenTheySelectProduct_ThenTheProductIsNotDispensedAndPriceDisplayed()
        {
            var product = new Product() {Name = ProductTypes.Cola, Price = 0.25m,AvailableStock = 1};
            IList<Product> products = new List<Product>(){product};
            _mockUsCurrencyService.Setup(s => s.IsAccepted(It.IsAny<ICoin>())).Returns(true);
            _mockProductRepo.Setup(s => s.Products).Returns(products);
            var coin = new Coin(USCoinTypes.Penny); 
            _vendingMachine.InsertCoin(coin);
            _vendingMachine.BuyProduct(ProductTypes.Cola);
            
            _vendingMachine.DispensedProduct.Should().BeNull();
            _vendingMachine.CurrentDisplay.Should().Be(VendingResponse.Price);
            _vendingMachine.CurrentDisplay.Should().Be((coin.Value).ToString(CultureInfo.InvariantCulture));

        }

        [Fact]
        public void GivenTheUserHasInsertedCoins_WhenTheySelectACheaperProduct_ThenTheChangeIsGiven()
        {  
            IList<Product> products = new List<Product>(){new Product(){Name = ProductTypes.Cola,Price = 1.00m,AvailableStock = 1}};
            var expectedCoins = new List<ICoin>(){new Coin(USCoinTypes.Dime)};
            var insertedCoins  = new List<ICoin>(){new Coin(USCoinTypes.Quarter),new Coin(USCoinTypes.Quarter),new Coin(USCoinTypes.Quarter),new Coin(USCoinTypes.Quarter),new Coin(USCoinTypes.Dime)};

            _mockUsCurrencyService.Setup(s => s.IsAccepted(It.IsAny<ICoin>())).Returns(true);
            _mockProductRepo.Setup(s => s.Products).Returns(products);
            _mockUsCurrencyService.Setup(s => s.CalculcateChangeCoins(It.IsAny<decimal>())).Returns(expectedCoins);

            foreach (var coin in insertedCoins)
            {
                _vendingMachine.InsertCoin(coin);

            }
            _vendingMachine.BuyProduct(ProductTypes.Cola);

            _vendingMachine.ReturnSlot.Should().Equal(expectedCoins);
        }

        [Fact]
        public void GivenThereIsMoneyInTheMachine_WhenRejectButtonIsPushed_TheCorrectChangeIsReturned()
        {
            _mockUsCurrencyService.Setup(s => s.IsAccepted(It.IsAny<ICoin>())).Returns(true);
            var coins = new List<ICoin>(){new Coin(USCoinTypes.Dime),new Coin(USCoinTypes.Nickel)};

            foreach (var coin in coins)
            {
                _vendingMachine.InsertCoin(coin);
            }
            _vendingMachine.RejectCoins();
            Assert.Equal(coins,_vendingMachine.ReturnSlot);
            Assert.Equal(VendingResponse.InsertedCoin,_vendingMachine.CurrentDisplay) ;
        }

        [Fact]
        public void GivenTheUserEnterCorrectMoneyAndSelectsProduct_WhenThereIsNoStock_ThenDisplayMessageIsSoldOut()
        {
            IList<Product> products = new List<Product>(){new Product(){Name = ProductTypes.Cola,Price = 1.00m,AvailableStock= 0}};

            var insertedCoins  = new List<ICoin>(){new Coin(USCoinTypes.Quarter),new Coin(USCoinTypes.Quarter),new Coin(USCoinTypes.Quarter),new Coin(USCoinTypes.Quarter)};
            foreach (var coin in insertedCoins)
            {
                _vendingMachine.InsertCoin(coin);
            }
            _vendingMachine.BuyProduct(ProductTypes.Cola);
            _vendingMachine.CurrentDisplay.Should().Be(VendingResponse.SoldOut);

        }

        [Fact]
        public void GivenThereIsNoChangeIntheMachine_ThenTheDisplayMessageShouldBeExactMoneyOnly()
        {
            _mockUsCurrencyService.SetupGet(s => s.AvailableChange).Returns(new List<ICoin>());

            _vendingMachine.CurrentDisplay.Should().Be(VendingResponse.ExactChangeOnly);

        }
    }

   
}