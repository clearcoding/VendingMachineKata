namespace Muscle.Vending.Coins
{
    public class CoinType
    {
        public decimal Weight { get; }
        public decimal Size { get; }
        public decimal Value { get; }

        public CoinType(decimal weight, decimal size, decimal value)
        {
            Weight = weight;
            Size = size;
            Value = value;
        }
    }
}