using System.Collections.Generic;
using FluentAssertions;
using Muscle.Vending.Display;
using Muscle.Vending.Products;
using Xunit;

namespace Muscle.Vending.Tests.VendingMachineTests
{
    public class SoldOutFeature : BaseVendingMachineFeature
    {
        [Fact]
        public void GivenTheUserEnterCorrectMoneyAndSelectsProduct_WhenThereIsNoStock_ThenDisplayMessageIsSoldOut()
        {
            IList<Product> products = new List<Product>()
                {new Product() {Name = ProductTypes.Cola, Price = 1.00m, AvailableStock = 0}};

            var insertedCoins = new List<ICoin>()
            {
                new Coin(USCoinTypes.Quarter), new Coin(USCoinTypes.Quarter), new Coin(USCoinTypes.Quarter),
                new Coin(USCoinTypes.Quarter)
            };
            foreach (var coin in insertedCoins)
            {
                _vendingMachine.InsertCoin(coin);
            }

            _vendingMachine.BuyProduct(ProductTypes.Cola);
            _vendingMachine.CurrentDisplay.Should().Be(VendingResponse.SoldOut);
        }
    }
}