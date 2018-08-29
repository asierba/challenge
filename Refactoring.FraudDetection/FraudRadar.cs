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

        public FraudRadar() : this(new FileSystem()) { }

        public FraudRadar(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }
        
        public IEnumerable<FraudResult> Check(string filePath)
        {
            var lines = _fileSystem.File.ReadAllLines(filePath);
            var orders = lines.Select(Order.FromCsv).ToList();
            return CheckFraud(orders);
        }

        private static IEnumerable<FraudResult> CheckFraud(IReadOnlyList<Order> orders)
        {
            var fraudResults = new List<FraudResult>();
            for (var i = 0; i < orders.Count; i++)
            {
                var currentOrder = orders[i];

                for (var j = i + 1; j < orders.Count; j++)
                {
                    var otherOrder = orders[j];

                    if (otherOrder.IsAFraudOf(currentOrder))
                    {
                        fraudResults.Add(new FraudResult(otherOrder));
                    }
                }
            }

            return fraudResults;
        }
    }
}