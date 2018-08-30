namespace Payvision.CodeChallenge.Refactoring.FraudDetection
{
    public class Order
    {
        public Order(int orderId, int dealId, string email, string street, string city, string state, string zipCode, string creditCard)
        {
            OrderId = orderId;
            DealId = dealId;
            Email = new Email(email);
            CreditCard = creditCard;
            Address = new Address(street, city, state, zipCode);
        }

        private Address Address { get; }

        public int OrderId { get; }

        private int DealId { get; }

        private Email Email { get; }

        private string CreditCard { get; }

        public bool IsAFraudOf(Order otherOrder)
        {
            return DealId == otherOrder.DealId
                   && CreditCard != otherOrder.CreditCard
                   && (Email.IsSimilarTo(otherOrder.Email) || Address.IsSimilarTo(otherOrder.Address));
        }
    }
}