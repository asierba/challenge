// -----------------------------------------------------------------------
// <copyright file="PositiveBitCounterTest.cs" company="Payvision">
//     Payvision Copyright © 2017
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics;
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

        [Fact]
        public void Count_Performance_Pin()
        {
            var timeSpan = Time(Performance);
            Assert.InRange(timeSpan,
                TimeSpan.Zero,
                TimeSpan.Parse("00:00:00.8"));
        }

        private void Performance()
        {
            var stuff = Enumerable.Range(0, 500000)
                .Select(x => bitCounter.Count(x).ToList())
                .ToList()
                .Select(i => i.Sum());
        }

        [Fact]
        public void Count_Performance_Bait()
        {
            var timeSpan = Time(Performance);
            Assert.InRange(timeSpan,
                TimeSpan.Zero,
                TimeSpan.Zero);
        }

        private TimeSpan Time(Action toTime)
        {
            var timer = Stopwatch.StartNew();
            toTime();
            timer.Stop();
            return timer.Elapsed;
        }
    }
}