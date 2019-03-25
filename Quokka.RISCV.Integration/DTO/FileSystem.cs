using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quokka.RISCV.Integration.DTO
{
    public enum eExtensionClass
    {
        Text,
        Binary
    }

    public abstract class FSFile
    {
        public string Name { get; set; }
    }

    public class FSTextFile : FSFile
    {
        public string Content { get; set; }

        public override string ToString()
        {
            return $"{Name} = [{Content.Substring(0, 10)}]";
        }
    }

    public class FSBinaryFile : FSFile
    {
        public byte[] Content { get; set; }

        public override string ToString()
        {
            return $"{Name} = [{Content.Length} bytes]";
        }
    }
    /// <summary>
    /// Set of files in base64 encoding
    /// </summary>
    public class FSSnapshot
    {
        public List<FSFile> Files { get; set; } = new List<FSFile>();

        public T Get<T>(string name)
            where T : FSFile
        {
            return Files.SingleOrDefault(f => f.Name == name) as T;
        }

        public void Add(FSFile file)
        {
            Files.Add(file);
        }

        public void Add(string name, string content)
        {
            Add(new FSTextFile() { Name = name, Content = content });
        }

        public void Add(string name, byte[] content)
        {
            Add(new FSBinaryFile() { Name = name, Content = content });
        }

        public void Merge(FSSnapshot snapshot, Func<FSFile, bool> filter = null)
        {
            var newFiles = snapshot.Files.Where(f => filter == null || filter(f)).ToList();
            var newFileNames = newFiles.Select(f => f.Name).ToHashSet();

            Files = Files.Where(f => !newFileNames.Contains(f.Name)).ToList();

            Files.AddRange(newFiles);
        }
    }

    public class ExtensionClasses
    {
        public Dictionary<string, eExtensionClass> Lookup = new Dictionary<string, eExtensionClass>();

        string NormalizeExtension(string extension)
        {
            return (extension.StartsWith(".") ? "" : ".") + extension.ToLower();
        }

        public bool Contains(string extension)
        {
            return Lookup.ContainsKey(NormalizeExtension(extension));
        }

        public ExtensionClasses Text(string extension)
        {
            Lookup[NormalizeExtension(extension)] = eExtensionClass.Text;

            return this;
        }

        public ExtensionClasses Binary(string extension)
        {
            Lookup[NormalizeExtension(extension)] = eExtensionClass.Binary;

            return this;
        }

        public eExtensionClass GetClass(string extension)
        {
            if (!Lookup.TryGetValue(NormalizeExtension(extension), out eExtensionClass cls))
                cls = eExtensionClass.Binary;

            return cls;
        }
    }
}
