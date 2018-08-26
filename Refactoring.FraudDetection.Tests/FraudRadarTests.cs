// -----------------------------------------------------------------------
// <copyright file="FraudRadarTests.cs" company="Payvision">
//     Payvision Copyright © 2017
// </copyright>
// -----------------------------------------------------------------------

using System.IO.Abstractions.TestingHelpers;
using Xunit;

namespace Payvision.CodeChallenge.Refactoring.FraudDetection.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using FluentAssertions;

    public class FraudRadarTests
    {
        [Fact]
        public void CheckFraud_OneLineFile_NoFraudExpected()
        {
            var result = ExecuteTest(Path.Combine(Environment.CurrentDirectory, "Files", "OneLineFile.txt"));

            result.Should().NotBeNull("The result should not be null.");
            result.Count().ShouldBeEquivalentTo(0, "The result should not contains fraudulent lines");
        }

        [Fact]
        public void CheckFraud_TwoLines_SecondLineFraudulent()
        {
            var result = ExecuteTest(Path.Combine(Environment.CurrentDirectory, "Files", "TwoLines_FraudulentSecond.txt"));

            result.Should().NotBeNull("The result should not be null.");
            result.Count().ShouldBeEquivalentTo(1, "The result should contains the number of lines of the file");
            result.First().IsFraudulent.Should().BeTrue("The first line is not fraudulent");
            result.First().OrderId.Should().Be(2, "The first line is not fraudulent");
        }

        [Fact]
        public void CheckFraud_ThreeLines_SecondLineFraudulent()
        {
            var result = ExecuteTest(Path.Combine(Environment.CurrentDirectory, "Files", "ThreeLines_FraudulentSecond.txt"));

            result.Should().NotBeNull("The result should not be null.");
            result.Count().ShouldBeEquivalentTo(1, "The result should contains the number of lines of the file");
            result.First().IsFraudulent.Should().BeTrue("The first line is not fraudulent");
            result.First().OrderId.Should().Be(2, "The first line is not fraudulent");
        }

        [Fact]
        public void CheckFraud_FourLines_MoreThanOneFraudulent()
        {
            var result = ExecuteTest(Path.Combine(Environment.CurrentDirectory, "Files", "FourLines_MoreThanOneFraudulent.txt"));

            result.Should().NotBeNull("The result should not be null.");
            result.Count().ShouldBeEquivalentTo(2, "The result should contains the number of lines of the file");
            result.First().OrderId.Should().Be(2);
            result.ElementAt(1).OrderId.Should().Be(4);
        }
        
        [Fact]
        public void CheckFraud_EmailsAreTheSame()
        {
            const string contents = 
@"1,1,bugs@bunny.com, , , , ,12345689010
2,1,bugs@BUNNY.com, , , , ,12345689011";
            var result = CheckFraud(contents);

            result.Count().ShouldBeEquivalentTo(1);
        }


        private static List<FraudResult> ExecuteTest(string filePath)
        {
            var fraudRadar = new FraudRadar();

            return fraudRadar.Check(filePath).ToList();
        }

        private static List<FraudResult> CheckFraud(string contents)
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {
                    @"file.txt", new MockFileData(contents)
                },
            });

            var fraudRadar = new FraudRadar(fileSystem);
            return fraudRadar.Check(@"file.txt").ToList();
        }
    }
}