using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace satelite.interfaces.log
{
    public interface ILogItem
    {
        LogType Tipo { get; }
        string Titulo { get; }
        string Descripcion { get; }
    }
}