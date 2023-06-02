namespace StockportWebappTests_Unit.Unit.Utils;

public class JwtDecoderTests
{
    private string _secretKeyValid = "secret";
    private string _secretKeyInvalid = "notsecret";
    private Mock<ILogger<JwtDecoder>> _logger = new Mock<ILogger<JwtDecoder>>();

    [Fact]
    public void ShouldDecodePayloadWithValidKey()
    {
        var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoidGVzdGluZyBuYW1lIiwiZW1haWwiOiJ0ZXN0aW5nQGVtYWlsIn0.jLDZVRKDV94Nl2r-ya8XzZzzj-nx3gMh1C_A-J5XvKQ";

        var encoding = new JwtDecoder(new GroupAuthenticationKeys() { Key = _secretKeyValid }, _logger.Object);

        var person = encoding.Decode(token);

        person.Email.Should().Be("testing@email");
        person.Name.Should().Be("testing name");
    }

    [Fact]
    public void ShouldFailDecryptionWithInvalidKey()
    {
        var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoidGVzdGluZyBuYW1lIiwiZW1haWwiOiJ0ZXN0aW5nQGVtYWlsIn0.QmkqA7HE-nOPqxx5kSG5NqDyVeBXUiJ3_i-lwZAdVkw";
        var encoding = new JwtDecoder(new GroupAuthenticationKeys() { Key = _secretKeyInvalid }, _logger.Object);

        Exception ex = Assert.Throws<Jose.IntegrityException>(() => encoding.Decode(token));

        ex.Message.Should().Be("Invalid signature.");

        LogTesting.Assert(_logger, LogLevel.Warning, $"IntegrityException was thrown from jwt decoder for token {token}");
    }

    [Fact]
    public void IfJsonStructureChangesInPayloadShouldNotError()
    {
        // json structure is { "somethingelse": "testing name", "invalid": "testing@email", "anoher prop": "test" }
        var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzb21ldGhpbmdlbHNlIjoidGVzdGluZyBuYW1lIiwiaW52YWxpZCI6InRlc3RpbmdAZW1haWwiLCJhbm9oZXIgcHJvcCI6InRlc3QifQ.Q-pSGiIo6HBbJ0fTMcstnXxuT42v-pEHOo9HoyspDWs";
        var encoding = new JwtDecoder(new GroupAuthenticationKeys() { Key = _secretKeyValid }, _logger.Object);

        var person = encoding.Decode(token);

        person.Name.Should().BeNull();
        person.Email.Should().BeNull();
    }

    [Fact]
    public void ShouldThrowExceptionIfInvalidJwtToken()
    {
        var token = "tokenhasbeentamperedwith";
        var encoding = new JwtDecoder(new GroupAuthenticationKeys() { Key = _secretKeyValid }, _logger.Object);

        Exception ex = Assert.Throws<InvalidJwtException>(() => encoding.Decode(token));

        ex.Message.Should().Be("Invalid JWT token");
        LogTesting.Assert(_logger, LogLevel.Warning, $"InvalidJwtException was thrown from jwt decoder for token {token}");
    }

    [Fact]
    public void ShouldThrowJsonExceptionIfInvalidJwtButInCorrectFormatWithDots()
    {
        var token = "tokenhasbeentamperedwith.test.test";

        var encoding = new JwtDecoder(new GroupAuthenticationKeys() { Key = _secretKeyValid }, _logger.Object);

        Exception ex = Assert.Throws<System.Text.Json.JsonException>(() => encoding.Decode(token));
    }
}
