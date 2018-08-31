// -----------------------------------------------------------------------
// <copyright file="PositiveBitCounterTest.cs" company="Payvision">
//     Payvision Copyright © 2017
// </copyright>
// -----------------------------------------------------------------------

using Xunit;

namespace CountingBits.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Payvision.CodeChallenge.Algorithms.CountingBits;

    public class PositiveBitCounterTest
    {
        private readonly PositiveBitCounter bitCounter = new PositiveBitCounter();

        [Fact]
        public void Count_NegativeValue_ArgumentExceptionExpected()
        {
            Assert.Throws<ArgumentException>(() => this.bitCounter.Count(-2));
        }

        [Fact]
        public void Count_Zero_NoOccurrences()
        {
            Assert.Equal(
                expected: new List<int> { 0 },
                actual: this.bitCounter.Count(0).ToList());
        }

        [Fact]
        public void Count_ValidInput_OneOcurrence()
        {
            Assert.Equal(
                expected: new List<int> { 1, 0 },
                actual: this.bitCounter.Count(1).ToList());
        }

        [Fact]
        public void Count_ValidInput_MultipleOcurrence()
        {
            Assert.Equal(
                expected: new List<int> { 3, 0, 5, 7 },
                actual: this.bitCounter.Count(161).ToList());
        }
    }
}