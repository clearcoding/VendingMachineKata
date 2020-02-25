using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Muscle.Vending.Currency;
using Muscle.Vending.Display;
using Muscle.Vending.Products;

namespace Muscle.Vending
{
    public class VendingMachine
    {
        private readonly ICurrencyService _currencyService;
        private readonly IProductRepository _productRepository;
        private string _currentDisplay;
        private string _singleUseDisplay;
        public VendingMachine(ICurrencyService currencyService,IProductRepository productRepository)
        {
            _currencyService = currencyService;
            _productRepository = productRepository;
            _currentDisplay = VendingResponse.InsertedCoin;
        }

        public string CurrentDisplay
        {
            get
            {
                var message = string.Empty;

                if (!string.IsNullOrEmpty(_singleUseDisplay))
                {
                    message = _singleUseDisplay;
                    _singleUseDisplay = string.Empty;
                }
                else
                {
                    if ((_currencyService.AvailableChange.Count == 0) && _currentDisplay.Equals(VendingResponse.InsertedCoin))
                        _currentDisplay = VendingResponse.ExactChangeOnly;
                    message = _currentDisplay;
                }

                return message;
            }
            
        }

        public decimal CurrentAmount => InsertedCoins.Sum(s => s.Value);
        public IList<ICoin> ReturnSlot { get; private set; }
        public Product DispensedProduct { get; private set; }

        private IList<ICoin> InsertedCoins => _currencyService.InsertedCoins;


        public void InsertCoin(ICoin coin)
        {
            var result = _currencyService.IsAccepted(coin);

            if (result)
            {
                _currentDisplay = VendingResponse.Accepted;
                _currencyService.InsertCoin(coin);
                return;
            }

            _currentDisplay = VendingResponse.InsertedCoin;
            ReturnSlot = new List<ICoin>() {coin};
        }

        public void RejectCoins()
        {
            ReturnSlot = InsertedCoins;
            _currentDisplay = VendingResponse.InsertedCoin;
        }
        
        public void BuyProduct(string product)
        {
           
                var selectedProduct = _productRepository.Products.Single(s => s.Name == product);
                if (selectedProduct.AvailableStock == 0)
                {
                    _currentDisplay = VendingResponse.SoldOut;
                    return;

                }
                if (CurrentAmount >= selectedProduct.Price)
                {
                   
                        selectedProduct.AvailableStock -- ;
                        DispensedProduct = selectedProduct;
                        _singleUseDisplay = VendingResponse.ThankYou;
                        _currentDisplay = VendingResponse.InsertedCoin;
                        ReturnSlot = _currencyService.CalculateChangeCoins(selectedProduct.Price - CurrentAmount);
                        return;
                }
                _singleUseDisplay= VendingResponse.Price;
                _currentDisplay = CurrentAmount == 0 ? VendingResponse.InsertedCoin : CurrentAmount.ToString(CultureInfo.InvariantCulture);
            
        }
    }
}