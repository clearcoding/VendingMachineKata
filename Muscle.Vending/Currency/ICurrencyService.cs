using System.Collections.Generic;

namespace Muscle.Vending.Currency
{
    public interface ICurrencyService
    {
        bool IsAccepted(ICoin coin);
        IList<ICoin> CalculcateChangeCoins(decimal change);

        IList<ICoin> AvailableChange { get; set; }
    }
}