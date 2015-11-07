using System;
using System.Collections.Generic;
using System.Text;

namespace satelite.interfaces.log
{
    public interface ILogable
    {
        ILogItem LogData { get; }
    }
}
