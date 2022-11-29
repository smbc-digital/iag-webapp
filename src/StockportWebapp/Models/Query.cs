namespace StockportWebapp.Models
{
    public class Query : IEquatable<Query>
    {
        private string _value;

        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = System.Text.Encodings.Web.UrlEncoder.Default.Encode(value);
            }
        }

        public string Name { get; set; }

        public Query(string name, string value)
        {
            Value = value;
            Name = name;
        }

        public bool Equals(Query other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Name, other.Name) && string.Equals(Value, other.Value);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name?.GetHashCode() ?? 0) * 397) ^ (Value?.GetHashCode() ?? 0);
            }
        }

        public override string ToString()
        {
            return string.Concat(Name, "=", Value);
        }
    }
}