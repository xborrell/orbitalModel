using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using satelite.interfaces.log;

namespace satelite.backend.log
{
    public class LogItem : ILogItem
    {
        public LogType Tipo { get; protected set; }
        public string Titulo { get; protected set; }
        public string Descripcion { get; protected set; }

        public LogItem(LogType tipo, string titulo, string descripcion)
        {
            Tipo = tipo;
            Titulo = titulo;
            Descripcion = descripcion;
        }

        public LogItem(LogType tipo, string titulo)
        {
            Tipo = tipo;
            Titulo = titulo;
            Descripcion = titulo;
        }
    }
}