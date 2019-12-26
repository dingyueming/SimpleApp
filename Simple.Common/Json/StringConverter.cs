using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Common.Json
{
    public class StringConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            string str = value?.ToString();
            if (str == null)
                writer.WriteNull();
            else
            {
                writer.WriteValue(str);
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return "";
            if (reader.TokenType == JsonToken.StartArray)
                reader.Skip();
            if (reader.Value == null)
                return "";
            string str = reader.Value.ToString();
            if (str == "[]")
                return "";
            else
                return str;
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }
}
