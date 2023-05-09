namespace StockportWebappTests_Unit.Unit.Utils;

public class CryptographyTest
{
    [Fact]
    public void ItReturnsAHexStringHashedWithSHA256()
    {
        var stringToHash = "Hellow World";
        var expectedResult = "b652f076fb4feeb1f934ac9b8c0606852e93d3a73fb2596a51c92e480e246897";

        var result = Cryptography.Sha256(stringToHash);

        result.Should().Be(expectedResult);
    }

    [Fact]
    public void ItReturnsAHexStringHashedWithHMAC265()
    {
        var stringToHash = "Hello World";
        var hashKey = "I am the key";
        var expectedResult = "64313bc905b99e89c5796140165faba466471152e32d4a3f89f527a686e06511";

        var result = Cryptography.HmacSha256(stringToHash, Encoding.UTF8.GetBytes(hashKey));

        Cryptography.ByteArrayToHexaString(result).Should().Be(expectedResult);
    }
}