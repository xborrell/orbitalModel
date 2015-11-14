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
    public class OrbitalContractResolver : DefaultContractResolver
    {
        protected readonly Dictionary<Type, HashSet<string>> Ignores;

        public OrbitalContractResolver()
        {
            this.Ignores = new Dictionary<Type, HashSet<string>>();
        }

        public void Ignore(Type type, params string[] propertyName)
        {
            if (!this.Ignores.ContainsKey(type)) this.Ignores[type] = new HashSet<string>();

            foreach (var prop in propertyName)
            {
                this.Ignores[type].Add(prop);
            }
        }

        public bool IsIgnored(Type type, string propertyName)
        {
            if (type == null) return false;

            if (!this.Ignores.ContainsKey(type)) return false;

            if (this.Ignores[type].Count == 0) return true;

            return this.Ignores[type].Contains(propertyName);
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            if (this.IsIgnored(property.DeclaringType, property.PropertyName)
            || this.IsIgnored(property.DeclaringType.BaseType, property.PropertyName))
            {
                property.ShouldSerialize = instance => { return false; };
            }

            return property;
        }
    }
}
