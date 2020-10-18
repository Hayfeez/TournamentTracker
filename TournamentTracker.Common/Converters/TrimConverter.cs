using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace TournamentTracker.Common.Converters
{
    public class TrimConverter : JsonConverter
    {
        public override bool CanRead => true;

        public override bool CanWrite => false;

        public override bool CanConvert(Type type) => type == typeof(string);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return ((string) reader.Value)?.Trim();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

}
