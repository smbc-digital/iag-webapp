namespace StockportWebapp.Utils;

public class Cryptography
{
    public static string Sha256(string data)
    {
        SHA256 sha257 = SHA256.Create();
        byte[] hashData = sha257.ComputeHash(Encoding.UTF8.GetBytes(data));

        return ByteArrayToHexaString(hashData);
    }

    public static byte[] HmacSha256(string data, byte[] key)
    {
        HMACSHA256 kha = new() { Key = key };

        return kha.ComputeHash(Encoding.UTF8.GetBytes(data));
    }

    public static string ByteArrayToHexaString(IEnumerable<byte> ba)
    {
        StringBuilder hex = new();

        foreach (byte b in ba)
            hex.AppendFormat("{0:x2}", b);
        
        return hex.ToString().ToLowerInvariant();
    }
}