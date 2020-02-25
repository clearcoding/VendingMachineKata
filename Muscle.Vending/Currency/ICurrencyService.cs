using System.Collections.Generic;

namespace Muscle.Vending.Currency
{
    public interface ICurrencyService
    {
        bool IsAccepted(ICoin coin);
        IList<ICoin> CalculateChangeCoins(decimal change);

        IList<ICoin> AvailableChange { get; set; }
        public IList<ICoin> InsertedCoins { get; set; }

        void InsertCoin(ICoin coin);

        IList<ICoin> ReturnCoins();
    }
}