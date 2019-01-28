using Quokka.RISCV.Integration.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Quokka.RISCV.Integration.Engine
{
    public class FSManager
    {
        private string _rootPath;

        public FSManager(string rootPath)
        {
            _rootPath = rootPath ?? throw new ArgumentNullException(nameof(rootPath));
            if (!_rootPath.EndsWith("\\") && !_rootPath.EndsWith("/"))
                _rootPath += "/";
        }

        public static void RecursiveCreateDirectory(string path)
        {
            if (Directory.Exists(path))
                return;

            var parent = Path.GetDirectoryName(path);
            RecursiveCreateDirectory(parent);

            Directory.CreateDirectory(path);
        }

        public void SaveSnapshot(FSSnapshot fsSnashot)
        {
            foreach (var file in fsSnashot.Files)
            {
                var fullPath = Path.Combine(_rootPath, file.Name);
                RecursiveCreateDirectory(Path.GetDirectoryName(fullPath));

                switch (file)
                {
                    case FSTextFile tf:
                        var localContent = string.Join(Environment.NewLine, tf.Content.Split("\n").Select(l => l.TrimEnd('\r')));
                        
                        File.WriteAllText(fullPath, localContent);
                        break;
                    case FSBinaryFile bf:
                        File.WriteAllBytes(fullPath, bf.Content);
                        break;
                }
            }
        }

        public FSSnapshot LoadSnapshot(
            ExtensionClasses classes, 
            IEnumerable<string> files)
        {
            var fs = new FSSnapshot();

            foreach (var file in files.Where(file => File.Exists(Path.Combine(_rootPath, file))))
            {
                var cls = classes.GetClass(Path.GetExtension(file));

                var fileName = Path.IsPathFullyQualified(file) ? file.Substring(_rootPath.Length) : file;

                switch (cls)
                {
                    case eExtensionClass.Text:
                        fs.Files.Add(new FSTextFile()
                        {
                            Name = fileName,
                            Content = File.ReadAllText(Path.Combine(_rootPath, file))
                        });
                        break;
                    case eExtensionClass.Binary:
                        fs.Files.Add(new FSBinaryFile()
                        {
                            Name = fileName,
                            Content = File.ReadAllBytes(Path.Combine(_rootPath, file))
                        });
                        break;
                }
            }

            return fs;
        }
    }
}
