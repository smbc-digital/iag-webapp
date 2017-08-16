using StockportWebapp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace StockportWebappTests.Unit.Helpers
{
    public class ParisHashHelperTest
    {
        public ParisHashHelper HashHelper { get; set; } = new ParisHashHelper(new ParisKeys() { PreSalt= "preSalt", PostSalt = "postSalt", PrivateSalt = "privateSalt"});

        [Fact]
        public void ShouldReturnFalseIfHashDoesntMatch()
        {
            // Arrange
            var expectedResult = false;
            var url = "?authorisationcode=900000&receiptnumber=IP%2F280885&transactiontype=09&merchantnumber=84800683&data=BikeabilityContribution&serviceprocessed=true&merchanttid=03151707&amount=100.00&date=16%2F06%2F2017%2014%3A07%3A23&administrationcharge=0";
            var hash = "DOESNOTMATCH";

            // Act
            var result = HashHelper.IsMatchingHash(url, hash);

            // Assert
            result.Should().Be(expectedResult);
        }

        [Fact]
        public void ShouldReturnTrueIfHashMatches()
        {
            // Arrange
            var expectedResult = true;
            var url = "?authorisationcode=900000&receiptnumber=IP/280885&transactiontype=09&merchantnumber=84800683&data=BikeabilityContribution&serviceprocessed=true&merchanttid=03151707&amount=100.00&date=16/06/2017 14:07:23&administrationcharge=0";
            var hash = "96-D7-2B-28-EA-F5-98-AD-D9-99-35-2C-6F-7E-B9-E8-95-EF-28-B2-7F-9D-2C-D2-B5-60-F8-39-FE-51-6B-CD-E8-52-EB-4B-3E-83-48-AA-B8-99-69-06-62-33-26-69-0D-3C-D5-84-FB-76-7A-C8-CA-6E-60-4A-B5-6A-19-EB";

            // Act
            var result = HashHelper.IsMatchingHash(url, hash);

            // Assert
            result.Should().Be(expectedResult);
        }

    }
}
