using System.Collections.Generic;
using FluentAssertions;
using Moq;
using Muscle.Vending.Display;
using Muscle.Vending.Products;
using Xunit;

namespace Muscle.Vending.Tests.VendingMachineTests
{
    public class MakeChangeVendingMachineFeature : BaseVendingMachineFeature
    {
        [Fact]
        public void GivenTheUserHasInsertedCoins_WhenTheySelectACheaperProduct_ThenTheChangeIsGiven()
        {
            IList<Product> products = new List<Product>()
                {new Product() {Name = ProductTypes.Cola, Price = 1.00m, AvailableStock = 1}};
            var expectedCoins = new List<ICoin>() {new Coin(USCoinTypes.Dime)};
            var insertedCoins = new List<ICoin>()
            {
                new Coin(USCoinTypes.Quarter), new Coin(USCoinTypes.Quarter), new Coin(USCoinTypes.Quarter),
                new Coin(USCoinTypes.Quarter), new Coin(USCoinTypes.Dime)
            };

            _mockUsCurrencyService.Setup(s => s.IsAccepted(It.IsAny<ICoin>())).Returns(true);
            _mockProductRepo.Setup(s => s.Products).Returns(products);
            _mockUsCurrencyService.Setup(s => s.CalculateChangeCoins(It.IsAny<decimal>())).Returns(expectedCoins);
            _mockUsCurrencyService.SetupGet(g => g.InsertedCoins).Returns(insertedCoins);


            _vendingMachine.BuyProduct(ProductTypes.Cola);

            _vendingMachine.ReturnSlot.Should().Equal(expectedCoins);
        }

        [Fact]
        public void GivenThereIsMoneyInTheMachine_WhenRejectButtonIsPushed_TheCorrectChangeIsReturned()
        {
            _mockUsCurrencyService.Setup(s => s.IsAccepted(It.IsAny<ICoin>())).Returns(true);
            var coins = new List<ICoin>() {new Coin(USCoinTypes.Dime), new Coin(USCoinTypes.Nickel)};
            _mockUsCurrencyService.SetupGet(g => g.InsertedCoins).Returns(coins);

            _vendingMachine.RejectCoins();
            _vendingMachine.ReturnSlot.Should().Equal(coins);
            _vendingMachine.CurrentDisplay.Should().Be(VendingResponse.InsertedCoin);
        }
    }
}