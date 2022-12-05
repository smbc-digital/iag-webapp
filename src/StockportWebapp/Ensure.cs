namespace StockportWebapp
{
    public static class Ensure
    {
        public static void ArgumentNotNullOrEmpty(string variable, string name)
        {
            if (string.IsNullOrWhiteSpace(variable))
            {
                throw new ArgumentException($"'{name}' cannot be null or empty.");
            }
        }

        public static void ArgumentIsAValidUri(string uriString, string name)
        {
            if (string.IsNullOrWhiteSpace(uriString) || !Uri.IsWellFormedUriString(uriString, UriKind.RelativeOrAbsolute))
            {
                throw new ArgumentException($"Configuration of {name} must exist and be a valid uri!");
            }
        }
    }
}