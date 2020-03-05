using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FluentAssertions;
using Moq;
using Muscle.Vending.Currency;
using Muscle.Vending.Display;
using Muscle.Vending.Products;
using Xunit;

namespace Muscle.Vending.Tests.VendingMachineTests
{
    public class SelectProductFeature:BaseVendingMachineFeature
    {
        
         public static IEnumerable<object[]> SelectProductData =>
            new List<object[]>
            {
                new object[] { new List<Product>(){new Product() {Name = ProductTypes.Cola, Price = 1.00m,AvailableStock = 1}},new List<ICoin>(){new Coin(USCoinTypes.Quarter),new Coin(USCoinTypes.Quarter),new Coin(USCoinTypes.Quarter),new Coin(USCoinTypes.Quarter)},ProductTypes.Cola},
                new object[] { new List<Product>(){new Product() {Name = ProductTypes.Chips, Price = 0.50m,AvailableStock = 1}},new List<ICoin>(){new Coin(USCoinTypes.Quarter),new Coin(USCoinTypes.Quarter)},ProductTypes.Chips},
                new object[] { new List<Product>(){new Product() {Name = ProductTypes.Candy, Price = 0.65m,AvailableStock = 1}},new List<ICoin>(){new Coin(USCoinTypes.Quarter),new Coin(USCoinTypes.Quarter),new Coin(USCoinTypes.Nickel),new Coin(USCoinTypes.Dime)},ProductTypes.Candy}

            };
        [Theory]
        [MemberData(nameof (SelectProductData))]
        public void GivenTheUserHasInsertedTheCorrectMoney_WhenTheySelectProduct_ThenTheProductIsDispensedAndThankYouDisplayed(IList<Product> products,IList<ICoin> insertedCoins,string productType)
        {
            
            _mockUsCurrencyService.Setup(s => s.IsAccepted(It.IsAny<ICoin>())).Returns(true);
            _mockUsCurrencyService.SetupGet(g => g.InsertedCoins).Returns(insertedCoins);
            _mockProductRepo.Setup(s => s.Products).Returns(products);
            _mockUsCurrencyService.SetupGet(g => g.InsertedCoins).Returns(insertedCoins);
            _vendingMachine.BuyProduct(productType);
            
            _vendingMachine.DispensedProduct.Name.Should().Be(productType);
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
            var coins = new List<ICoin>(){new Coin(USCoinTypes.Penny)}; 
            _mockUsCurrencyService.SetupGet(g => g.InsertedCoins).Returns(coins);

            _vendingMachine.BuyProduct(ProductTypes.Cola);
            
            _vendingMachine.DispensedProduct.Should().BeNull();
            _vendingMachine.CurrentDisplay.Should().Be(VendingResponse.Price);
            _vendingMachine.CurrentDisplay.Should().Be((coins.Sum(s=>s.Value)).ToString(CultureInfo.InvariantCulture));

        }
        [Fact]
        public void GivenTheUserHasNotInsertedAnyMoney_WhenTheySelectProduct_ThenTheProductIsNotDispensedAndInsertCoinDisplayed()
        {
            var product = new Product() {Name = ProductTypes.Cola, Price = 0.25m,AvailableStock = 1};
            IList<Product> products = new List<Product>(){product};
            _mockUsCurrencyService.Setup(s => s.IsAccepted(It.IsAny<ICoin>())).Returns(true);
            _mockProductRepo.Setup(s => s.Products).Returns(products);
            _mockUsCurrencyService.SetupGet(g => g.InsertedCoins).Returns(new List<ICoin>());

            _vendingMachine.BuyProduct(ProductTypes.Cola);
            
            _vendingMachine.DispensedProduct.Should().BeNull();
            _vendingMachine.CurrentDisplay.Should().Be(VendingResponse.Price);
            _vendingMachine.CurrentDisplay.Should().Be(VendingResponse.InsertedCoin);
            
        }
       
    }
}