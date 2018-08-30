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

            var bits = ToBitArray(input);

            var numOfOnes = 0;
            var indexOfOnes = new List<int>();
            for (var i = 0; i < bits.Length; i++)
            {
                if (bits[i])
                {
                    indexOfOnes.Add(i);
                    numOfOnes++;
                }
            }
            
            return new List<int>{numOfOnes}.Concat(indexOfOnes);
        }

        private static bool[] ToBitArray(int number)
        {
            var b = new BitArray(new[] {number});
            var bits = new bool[b.Count];
            b.CopyTo(bits, 0);
            return bits.ToArray();
        }
    }
}