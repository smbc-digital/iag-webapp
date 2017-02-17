using System.Text;
using StockportWebapp.Extensions;

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
            AddSourceForSafari9(source);

            return this;
        }

        private void AddSourceForSafari9(string source)
        {
            if (IsSafari9Exception(source))
            {
                _stringBuilder.Append(source);
            }
            else
            {
                AddSourceWithBothHttpAndHttpsForSafari9(source);
            }
        }

        private bool IsSafari9Exception(string source)
        {
            return source == "'unsafe-inline'"
                   || source == "'unsafe-eval'"
                   || source == "https:"
                   || source == "data:"
                   || source.StartsWith("*.");
        }

        private void AddSourceWithBothHttpAndHttpsForSafari9(string source)
        {
            source = source.StripHttpAndHttps();

            _stringBuilder.Append("http://" + source);
            _stringBuilder.Append(" ");
            _stringBuilder.Append("https://" + source);
        }

        public string Finish()
        {
            _stringBuilder.Append("; ");

            return _stringBuilder.ToString();
        }
    }
}