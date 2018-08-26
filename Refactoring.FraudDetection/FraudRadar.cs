// -----------------------------------------------------------------------
// <copyright file="FraudRadar.cs" company="Payvision">
//     Payvision Copyright © 2017
// </copyright>
// -----------------------------------------------------------------------

using System.IO.Abstractions;
using System.Linq;
using System.Collections.Generic;

namespace Payvision.CodeChallenge.Refactoring.FraudDetection
{
    public class FraudRadar
    {
        private readonly IFileSystem _fileSystem;

        public FraudRadar() : this(new FileSystem())
        {
        }

        public FraudRadar(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }
        
        public IEnumerable<FraudResult> Check(string filePath)
        {
            var orders = ReadOrders(filePath).ToList();
            return CheckFraud(orders);
        }

        private IEnumerable<Order> ReadOrders(string filePath)
        {
            var lines = _fileSystem.File.ReadAllLines(filePath);
            return lines.Select(Order.FromCsv);
        }

        private static IEnumerable<FraudResult> CheckFraud(List<Order> orders)
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