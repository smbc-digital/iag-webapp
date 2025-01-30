namespace StockportWebappTests_Unit.Unit.Utils;

public class JwtDecoderTests
{
    private readonly string _secretKeyValid = "secret";
    private readonly string _secretKeyInvalid = "notsecret";
    private readonly Mock<ILogger<JwtDecoder>> _logger = new();

    [Fact]
    public void ShouldDecodePayloadWithValidKey()
    {
        // Arrange
        string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoidGVzdGluZyBuYW1lIiwiZW1haWwiOiJ0ZXN0aW5nQGVtYWlsIn0.jLDZVRKDV94Nl2r-ya8XzZzzj-nx3gMh1C_A-J5XvKQ";

        JwtDecoder encoding = new(new GroupAuthenticationKeys() { Key = _secretKeyValid }, _logger.Object);

        // Act
        LoggedInPerson person = encoding.Decode(token);

        // Assert
        Assert.Equal("testing@email", person.Email);
        Assert.Equal("testing name", person.Name);
    }

    [Fact]
    public void ShouldFailDecryptionWithInvalidKey()
    {
        // Arrange
        string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoidGVzdGluZyBuYW1lIiwiZW1haWwiOiJ0ZXN0aW5nQGVtYWlsIn0.QmkqA7HE-nOPqxx5kSG5NqDyVeBXUiJ3_i-lwZAdVkw";
        JwtDecoder encoding = new(new GroupAuthenticationKeys() { Key = _secretKeyInvalid }, _logger.Object);
        
        // Act
        Exception ex = Assert.Throws<Jose.IntegrityException>(() => encoding.Decode(token));

        // Assert
        Assert.Equal("Invalid signature.", ex.Message);
        LogTesting.Assert(_logger, LogLevel.Warning, $"IntegrityException was thrown from jwt decoder for token {token}");
    }

    [Fact]
    public void IfJsonStructureChangesInPayloadShouldNotError()
    {
        // Arrange
        string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzb21ldGhpbmdlbHNlIjoidGVzdGluZyBuYW1lIiwiaW52YWxpZCI6InRlc3RpbmdAZW1haWwiLCJhbm9oZXIgcHJvcCI6InRlc3QifQ.Q-pSGiIo6HBbJ0fTMcstnXxuT42v-pEHOo9HoyspDWs";
        JwtDecoder encoding = new(new GroupAuthenticationKeys() { Key = _secretKeyValid }, _logger.Object);

        // Act
        LoggedInPerson person = encoding.Decode(token);

        // Assert
        Assert.Null(person.Name);
        Assert.Null(person.Email);
    }

    [Fact]
    public void ShouldThrowExceptionIfInvalidJwtToken()
    {
        // Arrange
        string token = "tokenhasbeentamperedwith";
        JwtDecoder encoding = new(new GroupAuthenticationKeys() { Key = _secretKeyValid }, _logger.Object);

        // Act
        Exception ex = Assert.Throws<InvalidJwtException>(() => encoding.Decode(token));

        // Assert
        Assert.Equal("Invalid JWT token", ex.Message);
        LogTesting.Assert(_logger, LogLevel.Warning, $"InvalidJwtException was thrown from jwt decoder for token {token}");
    }

    [Fact]
    public void ShouldThrowJsonExceptionIfInvalidJwtButInCorrectFormatWithDots()
    {
        // Arrange
        string token = "tokenhasbeentamperedwith.test.test";
        JwtDecoder encoding = new(new GroupAuthenticationKeys() { Key = _secretKeyValid }, _logger.Object);

        // Act & Assert
        Exception ex = Assert.Throws<System.Text.Json.JsonException>(() => encoding.Decode(token));
    }
}