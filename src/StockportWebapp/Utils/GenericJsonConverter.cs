using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace StockportWebapp.Utils
{
    public class GenericJsonConverter<TInterface, TConcrete> : JsonConverter
    {
        public override bool CanWrite => false;
        public override bool CanRead => true;
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TInterface);
        }
        public override void WriteJson(JsonWriter writer,
            object value, JsonSerializer serializer)
        {
            throw new InvalidOperationException("Use default serialization.");
        }

        public override object ReadJson(JsonReader reader,
            Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var deserialized = (TConcrete)Activator.CreateInstance(typeof(TConcrete));
            serializer.Populate(reader, deserialized);
            return deserialized;
        }
    }
}
