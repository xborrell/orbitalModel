using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Autofac;
using Autofac.Core;
using satelite.backend;
using satelite.backend.log;
using satelite.backend.orbital;
using satelite.implementation.wpf;
using satelite.interfaces;

namespace CalculadorOrbital
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Attach(new ConsoleObserver());

            using (var container = InicializarAutofac())
            {
                ISistema sistema = container.Resolve<ISistema>();
                sistema.Ejecutar();
            }
        }

        private static IEnumerable<Assembly> InicializarModulos()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();

            IList<Assembly> binAssemblies = new List<Assembly>();

            string binFolder = GetAssemblyDirectory();

            IList<string> dllFiles = Directory.GetFiles(binFolder, "*.dll", SearchOption.TopDirectoryOnly).ToList();

            foreach (string dllFile in dllFiles)
            {
                AssemblyName assemblyName = AssemblyName.GetAssemblyName(dllFile);

                Assembly locatedAssembly = assemblies.FirstOrDefault(a =>
                    AssemblyName.ReferenceMatchesDefinition(
                        a.GetName(), assemblyName));

                if (locatedAssembly == null)
                {
                    Assembly assembly = Assembly.LoadFile(dllFile);
                    assemblies.Add(assembly);
                }
            }

            return assemblies;
        }

        private static IContainer InicializarAutofac()
        {
            var builder = new ContainerBuilder();

            var assemblies = InicializarModulos();

            var moduleType = typeof(ISateliteModules);
            var sateliteType = typeof(ISateliteModules);
            var types = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(
                    t => t.IsClass 
                 && !t.IsAbstract 
                 && moduleType.IsAssignableFrom(t)
                 && sateliteType.IsAssignableFrom(t)
                 );

            foreach (var tipo in types)
            {
                var module = (IModule)Activator.CreateInstance(tipo);
                builder.RegisterModule(module);
            }
            builder.RegisterType<Sistema>().As<ISistema>();

            return builder.Build();
        }

        public static string GetAssemblyDirectory()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }
    }
}
