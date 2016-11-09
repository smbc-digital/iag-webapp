using System;

namespace StockportWebapp.Models
{
    public class Query : IEquatable<Query>
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public Query(string name, string value)
        {
            Name = name;
            Value = value;
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
                return ((Name?.GetHashCode() ?? 0)*397) ^ (Value?.GetHashCode() ?? 0);
            }
        }

        public override string ToString()
        {
            return string.Concat(Name, "=", Value);
        }
    }
}