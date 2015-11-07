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

namespace ModelTests
{
    public class SateliteTest
    {
        Fixture fixture = new SubFixture();
        Constantes constantes;

        public SateliteTest()
        {
            IToolsFactory factory = new TestFactory();
            constantes = new Constantes(factory);
        }

        //[Fact]
        //public void SateliteDataSerializationTest()
        //{
        //    Vector posicion = fixture.Generate<WpfVector>();
        //    Vector velocidad = fixture.Generate<WpfVector>();

        //    ISateliteData original = new SateliteData(constantes, posicion, velocidad);

        //    string json = JsonConvert.SerializeObject(original, Formatting.Indented);

        //    var converter = new ResultDataConverter(typeof(WpfVector));
        //    var deserializeSettings = new JsonSerializerSettings();
        //    deserializeSettings.Converters.Add(converter);
        //    ISateliteData obtained = JsonConvert.DeserializeObject<SateliteData>(json, deserializeSettings);

        //    obtained.ShouldBeEquivalentTo(original);
        //}

        [Fact]
        public void VectorSerializationTest()
        {
            Vector original = fixture.Generate<WpfVector>();

            string json = JsonConvert.SerializeObject(original, Formatting.Indented);
            Vector obtained = (Vector)JsonConvert.DeserializeObject(json);

            obtained.ShouldBeEquivalentTo(original);
        }
    }
}
