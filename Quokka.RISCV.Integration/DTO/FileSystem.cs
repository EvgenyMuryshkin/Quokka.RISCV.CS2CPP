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
    }

    public class FSBinaryFile : FSFile
    {
        public byte[] Content { get; set; }
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
    }
}
