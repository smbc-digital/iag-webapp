using System.Collections.Generic;
using System.Text;

namespace StockportWebappTests_Unit
{
    public class TestHelper
    {
        public static string AnyString = "Random string";

        public static string ByteArrayToHexaString(IEnumerable<byte> ba)
        {
            var hex = new StringBuilder();
            foreach (var b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString().ToLowerInvariant();
        }
    }
}
