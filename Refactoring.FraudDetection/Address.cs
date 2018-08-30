using System.Collections.Generic;
using System.Linq;

namespace Payvision.CodeChallenge.Refactoring.FraudDetection
{
    public class Address
    {
        private static readonly Dictionary<string, string> StreetAlternativeWords = new Dictionary<string, string>
        {
            {"st.", "street"},
            {"rd.", "road"},
        };

        private static readonly Dictionary<string, string> StateAlternativeWords = new Dictionary<string, string>
        {
            {"il", "illinois"},
            {"ca", "california"},
            {"ny", "new york"},
        };

        public Address(string street, string city, string state, string zipCode)
        {
            Street = street;
            City = city;
            State = state;
            ZipCode = zipCode;
        }

        private string Street { get; }

        private string City { get; }

        private string State { get; }

        private string ZipCode { get; }

        public bool IsSimilarTo(Address otherAddress)
        {
            return NormalizeState(State) == NormalizeState(otherAddress.State)
                   && ZipCode == otherAddress.ZipCode
                   && NormalizeStreet(Street) == NormalizeStreet(otherAddress.Street)
                   && NormalizeCity(City) == NormalizeCity(otherAddress.City);
        }
        
        private static string NormalizeCity(string city) => 
            city.ToLower();

        private static string NormalizeState(string state) => 
            StateAlternativeWords.Aggregate(state.ToLower(), (acc, pair) => acc.Replace(pair.Key, pair.Value));

        private static string NormalizeStreet(string street) => 
            StreetAlternativeWords.Aggregate(street.ToLower(), (acc, pair) => acc.Replace(pair.Key, pair.Value));

    }
}