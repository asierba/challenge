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

        public Order(int orderId, int dealId, string email, string street, string city, string state, string zipCode, string creditCard)
        {
            OrderId = orderId;
            DealId = dealId;
            Email = email;
            Street = street;
            City = city;
            State = state;
            ZipCode = zipCode;
            CreditCard = creditCard;
        }

        public int OrderId { get; }

        public int DealId { get; }

        public string Email { get; }

        public string Street { get; }

        public string City { get; }

        public string State { get; }

        public string ZipCode { get; }

        public string CreditCard { get; }

        private static string NormalizeCity(string city) => 
            city.ToLower();

        private static string NormalizeState(string state) => 
            StateKeyWords.Aggregate(state.ToLower(), (acc, pair) => acc.Replace(pair.Key, pair.Value));

        private static string NormalizeStreet(string street) => 
            StreetKeyWords.Aggregate(street.ToLower(), (acc, pair) => acc.Replace(pair.Key, pair.Value));

        private static string NormalizeEmail(string email)
        {
            var fullEmail = email.ToLower();
            var emailParts = fullEmail.Split(new[] {'@'}, StringSplitOptions.RemoveEmptyEntries);
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

        private bool HasSameEmail(Order otherOrder) =>
            NormalizeEmail(Email) == NormalizeEmail(otherOrder.Email);

        private bool HasSameAddress(Order otherOrder)
        {
            return NormalizeState(State) == NormalizeState(otherOrder.State)
                   && ZipCode == otherOrder.ZipCode
                   && NormalizeStreet(Street) == NormalizeStreet(otherOrder.Street)
                   && NormalizeCity(City) == NormalizeCity(otherOrder.City);
        }
    }
}