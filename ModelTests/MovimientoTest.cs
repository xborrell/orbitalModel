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
    public class MovimientoTest
    {
        Fixture fixture = new SubFixture();
        Constantes constantes;

        public MovimientoTest()
        {
            IToolsFactory factory = new TestFactory();
            constantes = new Constantes(factory);
        }

        [Fact]
        public void VectorSerializationTest()
        {
            Vector position = new WpfVector(100F, 0F, 0F);
            Vector velocidad = new WpfVector(0F, 0F, 0F);
            var data = new SateliteData(constantes, position, velocidad);

            var calculador = new CalculadorMovimiento(constantes);
            calculador.CalcularNuevaPosicion(data);

            data.Posicion.X.Should().BeInRange(99.6014F, 99.6015F);
            data.Posicion.Y.Should().Be(0F);
            data.Posicion.Z.Should().Be(0F);
        }
    }
}
