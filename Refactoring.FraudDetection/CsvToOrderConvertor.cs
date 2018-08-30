using System;
using System.Collections.Generic;
using System.Linq;
using Payvision.CodeChallenge.Refactoring.FraudDetection;

namespace Refactoring.FraudDetection
{
    internal static class CsvToOrderConverter
    {
        private const char CsvDelimiter = ',';

        public static List<Order> Convert(IEnumerable<string> csvRows)
        {
            return csvRows.Select(Convert).ToList();
        }

        private static Order Convert(string csvRow)
        {
            var columns = csvRow.Split(new[] {CsvDelimiter}, StringSplitOptions.RemoveEmptyEntries);

            var orderId = int.Parse(columns[0]);
            var dealId = int.Parse(columns[1]);
            var email = columns[2];
            var street = columns[3];
            var city = columns[4];
            var state = columns[5];
            var zipCode = columns[6];
            var creditCard = columns[7];
            
            return new Order
            (
                orderId,
                dealId,
                email,
                street,
                city,
                state,
                zipCode,
                creditCard
            );
        }
    }
}