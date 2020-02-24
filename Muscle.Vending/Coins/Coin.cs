using Muscle.Vending.Coins;

namespace Muscle.Vending
{
    public class Coin : ICoin
    {
        private readonly CoinType _coinType;

        public Coin(CoinType coinType)
        {
            _coinType = coinType;
        }

        public decimal Value => _coinType.Value;


        public decimal Weight => _coinType.Weight;

        public decimal Size => _coinType.Size;
    }
}