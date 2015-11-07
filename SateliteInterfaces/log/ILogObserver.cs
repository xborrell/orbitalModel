using System;
using System.Collections.Generic;
using System.Text;

namespace satelite.interfaces.log
{
    public interface ILogObserver
    {
        void Decision(ILogItem item, params object[] args);
        void Paso(ILogItem item, params object[] args);
    }
}
