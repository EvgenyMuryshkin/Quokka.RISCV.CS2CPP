using System;
using System.Collections.Generic;
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
