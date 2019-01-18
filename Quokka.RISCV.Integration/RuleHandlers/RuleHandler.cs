using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Quokka.RISCV.Integration.RuleHandlers
{
    public abstract class RuleHandler : IDisposable
    {
        protected string RootPath { get; set; }

        public RuleHandler(string rootPath)
        {
            RootPath = rootPath ?? throw new ArgumentNullException(nameof(rootPath));

            if (!RootPath.EndsWith("/"))
                RootPath += "/";
        }

        protected IEnumerable<string> AllFiles =>
            Directory
                .EnumerateFiles(RootPath, "*.*", SearchOption.AllDirectories)
                .Select(f => f.Substring(RootPath.Length));

        public abstract IEnumerable<string> MatchingFiles();

        public virtual void Reset()
        {

        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void OnDispose()
        {

        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    OnDispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
