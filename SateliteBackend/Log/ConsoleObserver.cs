using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using satelite.interfaces.log;

namespace satelite.backend.log
{
    public class ConsoleObserver : ILogObserver
    {
        public void Decision(ILogItem item, params object[] args)
        {
            string mensaje = args.Length > 0 ? string.Format(item.Titulo, args) : item.Titulo;

            Console.WriteLine(mensaje);
        }

        public void Paso(ILogItem item, params object[] args)
        {
            string mensaje = args.Length > 0 ? string.Format(item.Descripcion, args) : item.Descripcion;

            Console.WriteLine("    " + mensaje);
        }
    }
}
