using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Quokka.RISCV.Integration.RuleHandlers
{
    public class ModifiedFilesRuleHandler : RuleHandler
    {
        private FileSystemWatcher _watcher;
        HashSet<string> _modifiedFiles = new HashSet<string>();
        HashSet<string> _modifiedDirectories = new HashSet<string>();

        public ModifiedFilesRuleHandler(string rootPath) : base(rootPath)
        {
            _watcher = new FileSystemWatcher();
            _watcher.Path = RootPath;

            _watcher.Created += (e, a) =>
            {
                if (Directory.Exists(a.FullPath))
                    _modifiedDirectories.Add(a.FullPath);
                else
                    _modifiedFiles.Add(a.FullPath);
            };

            _watcher.Changed += (e, a) =>
            {
                if (Directory.Exists(a.FullPath))
                    _modifiedDirectories.Add(a.FullPath);
                else
                    _modifiedFiles.Add(a.FullPath);
            };

            _watcher.Renamed += (e, a) =>
            {
                if (Directory.Exists(a.FullPath))
                {
                    _modifiedDirectories.Remove(a.OldFullPath);
                    _modifiedDirectories.Add(a.FullPath);
                }
                else
                {
                    _modifiedFiles.Remove(a.OldFullPath);
                    _modifiedFiles.Add(a.FullPath);
                }
            };

            _watcher.Deleted += (e, a) =>
            {
                if (Directory.Exists(a.FullPath))
                    _modifiedDirectories.Remove(a.FullPath);
                else
                    _modifiedFiles.Remove(a.FullPath);

            };

            _watcher.EnableRaisingEvents = true;
        }

        public override IEnumerable<string> MatchingFiles()
        {
            var allModifiedFiles = _modifiedFiles
                .Union(_modifiedDirectories
                    .SelectMany(d => Directory.EnumerateFiles(d, "*.*", SearchOption.AllDirectories)))
                .ToHashSet();

            return AllFiles.Where(f => allModifiedFiles.Any(m => m.ToLower().EndsWith(f.ToLower())));
        }

        protected override void OnDispose()
        {
            _watcher.EnableRaisingEvents = false;
            _watcher.Dispose();
            _watcher = null;
        }

        public override void Reset()
        {
            _modifiedDirectories.Clear();
            _modifiedFiles.Clear();
        }
    }
}
