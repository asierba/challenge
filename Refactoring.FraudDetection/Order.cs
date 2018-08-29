using System;
using System.Collections.Generic;
using System.Linq;

namespace Payvision.CodeChallenge.Refactoring.FraudDetection
{
    public class Order
    {
        private static readonly Dictionary<string, string> StreetKeyWords = new Dictionary<string, string>
        {
            {"st.", "street"},
            {"rd.", "road"},
        };

        private static readonly Dictionary<string, string> StateKeyWords = new Dictionary<string, string>
        {
            {"il", "illinois"},
            {"ca", "california"},
            {"ny", "new york"},
        };

        public int OrderId { get; private set; }

        public int DealId { get; private set; }

        public string Email { get; private set; }

        public string Street { get; private set; }

        public string City { get; private set; }

        public string State { get; private set; }

        public string ZipCode { get; private set; }

        public string CreditCard { get; private set; }

        public static Order FromCsv(string line)
        {
            var items = line.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);

            return new Order
            {
                OrderId = Int32.Parse(items[0]),
                DealId = Int32.Parse(items[1]),
                Email = NormalizeEmail(items[2].ToLower()),
                Street = NormalizeStreet(items[3].ToLower()),
                City = items[4].ToLower(),
                State = NormalizeState(items[5].ToLower()),
                ZipCode = items[6],
                CreditCard = items[7]
            };
        }

        private static string NormalizeState(string state) => 
            StateKeyWords.Aggregate(state, (acc, pair) => acc.Replace(pair.Key, pair.Value));

        private static string NormalizeStreet(string street) => 
            StreetKeyWords.Aggregate(street, (acc, pair) => acc.Replace(pair.Key, pair.Value));

        private static string NormalizeEmail(string email)
        {
            var emailParts = email.Split(new[] {'@'}, StringSplitOptions.RemoveEmptyEntries);
            var emailName = RemoveDots(RemoveAfterPlus(emailParts[0]));
            var emailDomain = emailParts[1];
            return $"{emailName}@{emailDomain}";
        }

        private static string RemoveAfterPlus(string emailName)
        {
            var atIndex = emailName.IndexOf("+", StringComparison.Ordinal);
            return atIndex < 0 ? emailName : emailName.Remove(atIndex);
        }

        private static string RemoveDots(string emailName)
        {
            return emailName.Replace(".", "");
        }

        public bool IsAFraudOf(Order otherOrder)
        {
            return IsSameDeal(otherOrder)
                   && !HasSameCreditCard(otherOrder)
                   && (HasSameEmail(otherOrder) || HasSameAddress(otherOrder));
        }

        private bool HasSameCreditCard(Order otherOrder) => CreditCard == otherOrder.CreditCard;

        private bool IsSameDeal(Order otherOrder) => DealId == otherOrder.DealId;

        private bool HasSameEmail(Order otherOrder) => Email == otherOrder.Email;

        private bool HasSameAddress(Order otherOrder)
        {
            return State == otherOrder.State
                   && ZipCode == otherOrder.ZipCode
                   && Street == otherOrder.Street
                   && City == otherOrder.City;
        }
    }
}