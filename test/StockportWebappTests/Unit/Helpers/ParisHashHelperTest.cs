using StockportWebapp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace StockportWebappTests.Unit.Helpers
{
    public class ParisHashHelperTest
    {
        public ParisHashHelper HashHelper { get; set; } = new ParisHashHelper();

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
            Assert.Equal(result, expectedResult);
        }

        [Fact]
        public void ShouldReturnTrueIfHashMatches()
        {
            // Arrange
            var expectedResult = true;
            var url = "?authorisationcode=900000&receiptnumber=IP/280885&transactiontype=09&merchantnumber=84800683&data=BikeabilityContribution&serviceprocessed=true&merchanttid=03151707&amount=100.00&date=16/06/2017 14:07:23&administrationcharge=0";
            var hash = "FC-4A-FB-68-C7-E4-50-76-D3-07-42-71-09-79-48-EE-CB-C3-31-D0-40-CE-82-A6-98-B1-D1-63-4E-22-69-3D-E7-8F-0D-7C-19-98-9C-D5-4E-55-CC-5C-65-83-50-57-4E-45-8C-84-94-7A-4D-ED-E5-7E-A9-60-82-5E-6C-0F";

            // Act
            var result = HashHelper.IsMatchingHash(url, hash);

            // Assert
            Assert.Equal(result, expectedResult);
        }

    }
}
