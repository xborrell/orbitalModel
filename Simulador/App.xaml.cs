using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using Autofac.Core;
using satelite.interfaces;
using Xb.Simulador;
using Xb.Simulador.View;
using Xb.Simulador.Viewmodel;

namespace Simulador
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            string culture = ConfigurationManager.AppSettings["Culture"];
            if (!string.IsNullOrEmpty(culture))
            {
                CultureInfo ci = new CultureInfo(culture);
                Thread.CurrentThread.CurrentCulture = ci;
                Thread.CurrentThread.CurrentUICulture = ci;
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            using (var container = InicializarAutofac())
            {
                var window = container.Resolve<MainWindow>();
                window.Show();
            }

            //MainWindow window = new MainWindow();

            //MainWindowViewModel viewModel = new MainWindowViewModel();

            //window.DataContext = viewModel;

            //window.Show();
        }

        private IEnumerable<Assembly> InicializarModulos()
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

        private IContainer InicializarAutofac()
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
