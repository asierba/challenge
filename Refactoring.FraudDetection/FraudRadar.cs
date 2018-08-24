// -----------------------------------------------------------------------
// <copyright file="FraudRadar.cs" company="Payvision">
//     Payvision Copyright © 2017
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;

namespace Payvision.CodeChallenge.Refactoring.FraudDetection
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class FraudRadar
    {
        public IEnumerable<FraudResult> Check(string filePath)
        {
            var orders = ReadOrders(filePath).ToList();
            return ChecFraud(orders);
        }

        private static IEnumerable<Order> ReadOrders(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            return lines.Select(Order.CreateOrder);
        }

        private static IEnumerable<FraudResult> ChecFraud(List<Order> orders)
        {
            var fraudResults = new List<FraudResult>();
            for (int i = 0; i < orders.Count; i++)
            {
                var current = orders[i];
                bool isFraudulent = false;

                for (int j = i + 1; j < orders.Count; j++)
                {
                    isFraudulent = false;

                    if (current.DealId == orders[j].DealId
                        && current.Email == orders[j].Email
                        && current.CreditCard != orders[j].CreditCard)
                    {
                        isFraudulent = true;
                    }

                    if (current.DealId == orders[j].DealId
                        && current.State == orders[j].State
                        && current.ZipCode == orders[j].ZipCode
                        && current.Street == orders[j].Street
                        && current.City == orders[j].City
                        && current.CreditCard != orders[j].CreditCard)
                    {
                        isFraudulent = true;
                    }

                    if (isFraudulent)
                    {
                        fraudResults.Add(new FraudResult {IsFraudulent = true, OrderId = orders[j].OrderId});
                    }
                }
            }

            return fraudResults;
        }
    }
}