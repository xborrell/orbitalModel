using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using satelite.interfaces.log;

namespace satelite.backend.log
{
    public class FileObserver : ILogObserver, IDisposable
    {
        public StreamWriter Writer
        {
            get { return _writer; }
            set { _writer = value; }
        }
        private StreamWriter _writer;

        public FileObserver(string name)
        {
            Writer = new StreamWriter(name, false, Encoding.Default);
            Writer.AutoFlush = true;
        }

        ~FileObserver()
        {
            Dispose(false);
        }

        public void Decision(ILogItem item, params object[] args)
        {
            string mensaje = args.Length > 0 ? string.Format(item.Titulo, args) : item.Titulo;

            Writer.WriteLine(mensaje);
        }

        public void Paso(ILogItem item, params object[] args)
        {
            string mensaje = args.Length > 0 ? string.Format(item.Descripcion, args) : item.Descripcion;

            Writer.WriteLine(mensaje);
        }

        #region IDisposable Members
        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Writer.Dispose();
                }
            }
            disposed = true;
        }
        #endregion
    }
}
