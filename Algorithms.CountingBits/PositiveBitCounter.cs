// -----------------------------------------------------------------------
// <copyright file="BitCounter.cs" company="Payvision">
//     Payvision Copyright © 2017
// </copyright>
// -----------------------------------------------------------------------

using System.Collections;
using System.Linq;
using System;
using System.Collections.Generic;

namespace Payvision.CodeChallenge.Algorithms.CountingBits
{
    public class PositiveBitCounter
    {
        public IEnumerable<int> Count(int input)
        {
            if (input < 0)
                throw new ArgumentException(nameof(input));

            var bits = ToBitArray(input).ToList();

            var numOfOnes = bits.Count(x => x);
            var indexOfOnes = GetIndexesForOnes(bits);
            
            return new List<int>{numOfOnes}.Concat(indexOfOnes);
        }

        private static IEnumerable<bool> ToBitArray(int number)
        {
            var b = new BitArray(new[] {number});
            var bits = new bool[b.Count];
            b.CopyTo(bits, 0);
            return bits;
        }

        private static IEnumerable<int> GetIndexesForOnes(IReadOnlyList<bool> bits)
        {
            for (var i = 0; i < bits.Count(); i++)
            {
                if(bits[i])
                    yield return i;
            }
        }
    }
}