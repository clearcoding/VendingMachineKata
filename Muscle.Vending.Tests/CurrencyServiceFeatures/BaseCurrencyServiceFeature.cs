using Muscle.Vending.Currency;

namespace Muscle.Vending.Tests.CurrencyServiceFeatures
{
    public abstract class BaseCurrencyServiceFeature
    {
        protected UsCurrencyService _currencyService;

        public BaseCurrencyServiceFeature()
        {
            _currencyService  = new UsCurrencyService();
        }
    }
}