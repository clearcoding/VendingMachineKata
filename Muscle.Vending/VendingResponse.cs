using System.Collections.Generic;

namespace Muscle.Vending
{
    public static class VendingResponse
    {
        public const string Accepted = "Accepted";
        public const string InsertedCoin = "Insert Coin";
        public static string ThankYou => "Thank You";
        public static string Price => "Price";
        public static string SoldOut => "Sold Out";
        public static string ExactChangeOnly => "Exact Change Only";
    }
}