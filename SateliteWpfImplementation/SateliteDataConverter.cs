using System;
using Newtonsoft.Json;
using satelite.backend;
using satelite.interfaces;
using satelite.implementation.wpf;
using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace satelite.implementation.wpf
{
    public class SateliteDataConverter : JsonConverter
    {
        Constantes constantes;

        public SateliteDataConverter(Constantes constantes)
        {
            this.constantes = constantes;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ISateliteData);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var data = (SateliteData)serializer.Deserialize(reader, typeof(SateliteData));

            data.AsignarConstantes(constantes);

            return data;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}
