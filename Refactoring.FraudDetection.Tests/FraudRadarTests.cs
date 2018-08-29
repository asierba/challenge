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
        
        [Theory]
        [InlineData(
@"1,1,bugs@bunny.com, Street1, , , ,11111111111
2,1,bugs@BUNNY.com, Street2, , , ,22222222222")] // lower case comparison
        [InlineData(
@"1,1,bugs@bunny.com, Street1, , , ,11111111111
2,1,b.ug..s@BUNNY.com, Street2, , , ,22222222222")] // '.' are ignored
        [InlineData(
@"1,1,bugs@bunny.com, Street1, , , ,11111111111
2,1,bugs+stuffafter@BUNNY.com, Street2, , , ,22222222222")] // characters after '+' are ignored
        public void CheckFraud_EmailsAreNormalizedForComparison(string contents)
        {
            var result = CheckFraud(contents);
            result.Count().ShouldBeEquivalentTo(1);
        }
        
        [Theory]
        [InlineData(
@"1,1,email1@example.com,123 Sesame St.,New York,NY,10011,11111111111
2,1,email2@example.com,123 sesame st.,New York,NY,10011,22222222222")] // lowercase
        [InlineData(
@"1,1,email1@example.com,123 Sesame Street,New York,NY,10011,11111111111
2,1,email2@example.com,123 Sesame St.,New York,NY,10011,22222222222")] // st. -> street
        [InlineData(
@"1,1,email1@example.com,123 Main Road,New York,NY,10011,11111111111
2,1,email2@example.com,123 Main rd.,New York,NY,10011,22222222222")] // rd. -> road
        public void CheckFraud_StreetsAreNormalizedForComparison(string contents)
        {
            var result = CheckFraud(contents);
            result.Count().ShouldBeEquivalentTo(1);
        }
        
        
        [Theory]
        [InlineData(
@"1,1,email1@example.com,123 Sesame St.,New York,ny,10011,11111111111
2,1,email2@example.com,123 sesame st.,New York,NY,10011,22222222222")] // lowercase
        [InlineData(
@"1,1,email1@example.com,123 Sesame St.,New York,NY,10011,11111111111
2,1,email2@example.com,123 Sesame St.,New York,new york,10011,22222222222")] // ny -> new york
// BUG illinois gets replaced with illinoislinois
//        [InlineData(
//@"1,1,email1@example.com,123 Sesame St.,New York,il,10011,11111111111
//2,1,email2@example.com,123 Sesame St.,New York,illinois,10011,22222222222")]  
// BUG california gets replaced with californialifornia
//        [InlineData(
//@"1,1,email1@example.com,123 Sesame St.,New York,ca,10011,11111111111
//2,1,email2@example.com,123 Sesame St.,New York,california,10011,22222222222")] 
        public void CheckFraud_SatesAreNormalizedForComparison(string contents)
        {
            var result = CheckFraud(contents);
            result.Count().ShouldBeEquivalentTo(1);
        }
        
        [Fact]
        public void CheckFraud_MultipleErrors()
        {
            const string contents = @"1,1,bugs@bunny.com, Street1, , , ,11111111111
2,1,bugs@bunny.com, Street1, , , ,22222222222
3,1,bugs@bunny.com, Street1, , , ,33333333333
4,1,bugs@bunny.com, Street1, , , ,44444444444";
            var result = CheckFraud(contents);
            
            result.Select(x => x.OrderId).ShouldBeEquivalentTo(new [] {2,3,3,4,4,4}); 
            // BUG frauds are duplicated in result 
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