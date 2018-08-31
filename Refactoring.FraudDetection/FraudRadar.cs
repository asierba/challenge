// -----------------------------------------------------------------------
// <copyright file="FraudRadar.cs" company="Payvision">
//     Payvision Copyright © 2017
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.IO.Abstractions;
using System.Collections.Generic;
using Refactoring.FraudDetection;

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
            if (filePath == null)
                throw new ArgumentException(nameof(filePath));

            var lines = _fileSystem.File.ReadAllLines(filePath);
            var orders = CsvToOrderConverter.Convert(lines);
            return CheckFraud(orders);
        }

        private static IEnumerable<FraudResult> CheckFraud(IReadOnlyList<Order> orders)
        {
            for (var i = 0; i < orders.Count; i++)
            {
                var currentOrder = orders[i];

                for (var j = i + 1; j < orders.Count; j++)
                {
                    var otherOrder = orders[j];

                    if (otherOrder.IsAFraudOf(currentOrder))
                    {
                        yield return new FraudResult(otherOrder);
                    }
                }
            }
        }
    }
}