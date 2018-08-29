namespace Payvision.CodeChallenge.Refactoring.FraudDetection
{
    public class FraudResult
    {
        public FraudResult(Order order)
        {
            OrderId = order.OrderId;
        }

        public int OrderId { get; }

        public bool IsFraudulent => true;
    }
}