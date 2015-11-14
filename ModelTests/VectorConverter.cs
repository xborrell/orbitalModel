using System;
using Newtonsoft.Json;
using satelite.backend;
using satelite.interfaces;
using SimpleFixture;
using Xunit;
using FluentAssertions;
using SimpleFixture.NSubstitute;
using NSubstitute;
using satelite.implementation.wpf;
using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace ModelTests
{
    public class VectorConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Vector);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return serializer.Deserialize(reader, typeof(WpfVector));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}
