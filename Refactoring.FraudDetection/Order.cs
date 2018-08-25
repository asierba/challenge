using System;

namespace Payvision.CodeChallenge.Refactoring.FraudDetection
{
    public class Order
    {
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

            var order = new Order
            {
                OrderId = Int32.Parse(items[0]),
                DealId = Int32.Parse(items[1]),
                Email = items[2].ToLower(),
                Street = items[3].ToLower(),
                City = items[4].ToLower(),
                State = items[5].ToLower(),
                ZipCode = items[6],
                CreditCard = items[7]
            };
            
            return Normalize(order);
        }

        private static Order Normalize(Order order)
        {
            order.Email = NormalizeEmail(order);
            order.Street = NormalizeStreet(order);
            order.State = NormalizeState(order);

            return order;
        }

        private static string NormalizeState(Order order)
        {
            return order.State.Replace("il", "illinois").Replace("ca", "california").Replace("ny", "new york");
        }

        private static string NormalizeStreet(Order order)
        {
            return order.Street.Replace("st.", "street").Replace("rd.", "road");
        }

        private static string NormalizeEmail(Order order)
        {
            var aux = order.Email.Split(new char[] {'@'}, StringSplitOptions.RemoveEmptyEntries);

            var atIndex = aux[0].IndexOf("+", StringComparison.Ordinal);

            aux[0] = atIndex < 0 ? aux[0].Replace(".", "") : aux[0].Replace(".", "").Remove(atIndex);

            return String.Join("@", new string[] {aux[0], aux[1]});
        }
    }
}