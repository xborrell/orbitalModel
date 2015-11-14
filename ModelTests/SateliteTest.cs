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
    public class SateliteTest
    {
        Fixture fixture = new SubFixture();
        Constantes constantes;
        JsonSerializerSettings jsonSettings;

        public SateliteTest()
        {
            IToolsFactory factory = new TestFactory();
            constantes = new Constantes(factory);

            var resolver = new OrbitalContractResolver();
            resolver.Ignore(typeof(Vector), "Magnitude");

            resolver.Ignore(typeof(SateliteData), "SemiejeMayor");
            resolver.Ignore(typeof(SateliteData), "Altura");
            resolver.Ignore(typeof(SateliteData), "VelocidadPeriapsis");

            jsonSettings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                FloatParseHandling = FloatParseHandling.Decimal,
                
                ContractResolver = resolver
            };
            jsonSettings.Converters.Add(new VectorConverter());
            jsonSettings.Converters.Add(new SateliteDataConverter(constantes));
        }

        [Fact]
        public void VectorSerializationTest()
        {
            Vector original = new WpfVector(10.123456789F, 15F, 20F);

            string json = JsonConvert.SerializeObject(original, jsonSettings);
            string expected = @"{""X"":10.123457,""Y"":15.0,""Z"":20.0}";

            json.Should().Be(expected);
        }

        [Fact]
        public void VectorDeSerializationTest()
        {
            string jsonString = @"{""X"":10.123457,""Y"":15.0,""Z"":20.0}";

            Vector obtained = JsonConvert.DeserializeObject<WpfVector>(jsonString, jsonSettings);
            var expected = new WpfVector(10.123456789F, 15F, 20F);

            obtained.ShouldBeEquivalentTo(expected);
        }

        [Fact]
        public void SateliteDataSerializationTest()
        {
            Vector position = new WpfVector(-822.79774F, -4438.63582F, 5049.31502F);
            Vector velocidad = new WpfVector(7.418175658F, .709253354F, 1.828703177F);
            var original = new SateliteData( constantes, position, velocidad );

            string json = JsonConvert.SerializeObject(original, jsonSettings);
            string expected = @"{""Velocidad"":{""X"":7.4181757,""Y"":0.7092534,""Z"":1.82870317},""Posicion"":{""X"":-822.7977,""Y"":-4438.63574,""Z"":5049.315},""Orientacion"":{""X"":1.0,""Y"":0.0,""Z"":0.0},""Camara"":{""X"":-1.21481311,""Y"":-6.553388,""Z"":7.45502043},""Actitud"":1,""ActitudSolicitada"":0,""Impulso"":0.0,""ImpulsoSolicitado"":-1.0,""AlturaDeReferencia"":0.0,""Apoapsis"":-1.0,""Periapsis"":-1.0,""Inclinacion"":-1.0,""OrbitaSubiendo"":null}";

            json.Should().Be(expected);
        }

        [Fact]
        public void SateliteDeSerializationTest()
        {
            string jsonString = @"{""Velocidad"":{""X"":7.4181757,""Y"":0.7092534,""Z"":1.82870317},""Posicion"":{""X"":-822.7977,""Y"":-4438.63574,""Z"":5049.315},""Orientacion"":{""X"":1.0,""Y"":0.0,""Z"":0.0},""Camara"":{""X"":-1.21481311,""Y"":-6.553388,""Z"":7.45502043},""Actitud"":1,""ActitudSolicitada"":0,""Impulso"":0.0,""ImpulsoSolicitado"":-1.0,""AlturaDeReferencia"":0.0,""Apoapsis"":-1.0,""Periapsis"":-1.0,""Inclinacion"":-1.0,""OrbitaSubiendo"":null}";

            ISateliteData obtained = JsonConvert.DeserializeObject<ISateliteData>(jsonString, jsonSettings);
            
            Vector position = new WpfVector(-822.79774F, -4438.63582F, 5049.31502F);
            Vector velocidad = new WpfVector(7.418175658F, .709253354F, 1.828703177F);
            var expected = new SateliteData(constantes, position, velocidad);

            obtained.ShouldBeEquivalentTo(expected);
        }
    }
}
