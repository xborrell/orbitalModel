using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using satelite.interfaces.log;

namespace satelite.backend.log
{
    static public class Log
    {
        static private List<ILogObserver> observadores = new List<ILogObserver>();

        static public void Decision(ILogItem item, params object[] args)
        {
            observadores.ForEach(observer => observer.Decision(item, args));
        }

        static public void Paso(ILogItem item, params object[] args)
        {
            observadores.ForEach(observer => observer.Paso(item, args));
        }

        static public int ObserversCount()
        {
            return observadores.Count;
        }

        static public ILogObserver GetObserver(int i)
        {
            return observadores[i];
        }

        static public void Attach(ILogObserver observador)
        {
            if (!observadores.Contains(observador))
            {
                observadores.Add(observador);
            }
        }

        static public void Dettach(ILogObserver observador)
        {
            if (observadores.Contains(observador))
            {
                observadores.Remove(observador);
            }
        }
    }
}
