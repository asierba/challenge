using System;
using System.Diagnostics;
using System.Linq;
using Payvision.CodeChallenge.Algorithms.CountingBits;
using Xunit;

namespace CountingBits.Tests
{
    // These tests are used for performance tuning. They are ignore by default.
    // Idea taken from https://stackoverflow.com/questions/15181358/how-can-i-unit-test-performa nce-optimisations-in-c#15182072
    public class PerformanceTests
    {
        private readonly PositiveBitCounter bitCounter = new PositiveBitCounter();
        
        [Fact(Skip = "Used only for performance tuning")] 
        public void Count_Performance_Pin()
        {
            var timeSpan = Time(ExecuteMultipleTimes);
            Assert.InRange(timeSpan,
                TimeSpan.Zero,
                TimeSpan.Parse("00:00:00.8"));
        }

        [Fact(Skip = "Used only for performance tuning")]
        public void Count_Performance_Bait()
        {
            var timeSpan = Time(ExecuteMultipleTimes);
            Assert.InRange(timeSpan,
                TimeSpan.Zero,
                TimeSpan.Zero);
        }

        private void ExecuteMultipleTimes()
        {
            var result = Enumerable.Range(0, 500000)
                .Select(x => bitCounter.Count(x).ToList())
                .ToList()
                .Select(i => i.Sum());
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