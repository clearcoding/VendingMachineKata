using System.Drawing;
using System.Runtime.InteropServices;
using Muscle.Vending.Coins;

namespace Muscle.Vending
{
    public static class USCoinTypes
    {
        public static CoinType Dime => new CoinType(1m, 1m, 0.1m);
        public static CoinType Penny => new CoinType(0.2m, 0.2m, 0.01m);
        public static CoinType Nickel => new CoinType(1.5m, 1.5m, 0.05m);
        public static CoinType Quarter => new CoinType(1.8m, 1.8m, 0.25m);
    }
}