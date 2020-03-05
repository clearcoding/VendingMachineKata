using System.Collections.Generic;
using Moq;
using Muscle.Vending.Currency;
using Muscle.Vending.Products;

namespace Muscle.Vending.Tests.VendingMachineTests
{
    public abstract class BaseVendingMachineFeature
    {
        protected readonly Mock<ICurrencyService> _mockUsCurrencyService;
        protected static readonly ICoin Dime = new Coin(USCoinTypes.Dime);
        protected readonly Mock<IProductRepository> _mockProductRepo;
        protected VendingMachine _vendingMachine;
     
        public BaseVendingMachineFeature()
        {
            _mockUsCurrencyService = new Mock<ICurrencyService>();
            _mockProductRepo = new Mock<IProductRepository>();

            var products = new List<Product>()
            {
                new Product(){Name = "Cola"}
            };
            _mockProductRepo.SetupGet(p => p.Products).Returns(products);
            _mockUsCurrencyService.SetupGet(s => s.AvailableChange)
                .Returns(new List<ICoin>() {new Coin(USCoinTypes.Dime)});
            _vendingMachine  = new VendingMachine(_mockUsCurrencyService.Object,_mockProductRepo.Object);

        }
    }
}