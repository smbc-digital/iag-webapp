using System.Text;

namespace StockportWebapp.Utils
{
    public class ContentSecurityPolicyElement
    {
        private StringBuilder _stringBuilder;

        public ContentSecurityPolicyElement(string sourcetype, bool containsSelf = true)
        {
            _stringBuilder = new StringBuilder(sourcetype);

            if (containsSelf)
            {
                _stringBuilder.Append(" 'self'");
            }
        }

        public ContentSecurityPolicyElement AddSource(string source)
        {
            _stringBuilder.Append(" ");
            _stringBuilder.Append(source);

            return this;
        }

        public string Finish()
        {
            _stringBuilder.Append("; ");

            return _stringBuilder.ToString();
        }
    }
}