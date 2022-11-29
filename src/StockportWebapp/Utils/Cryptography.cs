using System.Security.Cryptography;
using System.Text;

namespace StockportWebapp.Utils
{
    public class Cryptography
    {
        public static string Sha256(string data)
        {
            var sha257 = SHA256.Create();
            var hashData = sha257.ComputeHash(Encoding.UTF8.GetBytes(data));
            return ByteArrayToHexaString(hashData);
        }

        public static byte[] HmacSha256(string data, byte[] key)
        {
            var kha = new HMACSHA256() { Key = key };
            return kha.ComputeHash(Encoding.UTF8.GetBytes(data));
        }

        public static string ByteArrayToHexaString(IEnumerable<byte> ba)
        {
            var hex = new StringBuilder();
            foreach (var b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString().ToLowerInvariant();
        }
    }
}