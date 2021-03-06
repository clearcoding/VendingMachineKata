using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Muscle.Vending.Coins;

namespace Muscle.Vending.Currency
{
    public class UsCurrencyService : ICurrencyService
    {
        public UsCurrencyService()
        {
            AvailableChange = new List<ICoin>();
            InsertedCoins = new List<ICoin>();
        }

        private IList<CoinType> AcceptedCoins => new List<CoinType>
            {USCoinTypes.Dime, USCoinTypes.Nickel, USCoinTypes.Quarter};

        private IList<CoinType> AllCoins => AcceptedCoins.Concat(new List<CoinType>() {USCoinTypes.Penny}).ToList();

        public bool IsAccepted(ICoin coin)
        {
            return AcceptedCoins.Any(a => a.Size == coin.Size && a.Weight == coin.Weight);
        }

        public IList<ICoin> CalculateChangeCoins(decimal change)
        {
            var changeGiven = new List<ICoin>();
            while (change > 0)
            {
                foreach (var coinType in AllCoins.OrderByDescending(o => o.Value))
                {
                    if (change >= coinType.Value)
                    {
                        changeGiven.Add(new Coin(coinType));
                        change -= coinType.Value;
                        break;
                    }
                }
            }

            return changeGiven;
        }

        public IList<ICoin> AvailableChange { get; set; }
        public IList<ICoin> InsertedCoins { get; set; }

        public IList<ICoin> ReturnCoins()
        {
            var returnCoins = InsertedCoins;
            foreach (var coin in InsertedCoins)
            {
                AvailableChange.Remove(coin);
            }

            InsertedCoins = new List<ICoin>();
            return returnCoins;
        }

        public void InsertCoin(ICoin coin)
        {
            AvailableChange.Add(coin);
            InsertedCoins.Add(coin);
        }
    }
}