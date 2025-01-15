namespace StockportWebappTests_Unit.Unit.Utils;

public class CryptographyTest
{
    [Fact]
    public void ItReturnsAHexStringHashedWithSHA256()
    {
        // Act
        string result = Cryptography.Sha256("Hellow World");

        // Assert
        Assert.Equal("b652f076fb4feeb1f934ac9b8c0606852e93d3a73fb2596a51c92e480e246897", result);
    }

    [Fact]
    public void ItReturnsAHexStringHashedWithHMAC265()
    {
        // Act
        byte[] result = Cryptography.HmacSha256("Hello World", Encoding.UTF8.GetBytes("I am the key"));

        // Assert
        Assert.Equal("64313bc905b99e89c5796140165faba466471152e32d4a3f89f527a686e06511", Cryptography.ByteArrayToHexaString(result));
    }
}