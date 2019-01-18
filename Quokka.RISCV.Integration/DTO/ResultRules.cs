using System;
using System.Collections.Generic;
using System.Text;

namespace Quokka.RISCV.Integration.DTO
{
    public abstract class FileRule
    {

    }

    public class ModifiedFilesRule : FileRule
    {

    }

    public class RegexMatchFilesRule : FileRule
    {
        public List<string> Patterns { get; set; } = new List<string>();
    }

    public class ExtensionMatchFilesRule : FileRule
    {
        public List<string> Extensions { get; set; } = new List<string>();
    }

    /// <summary>
    /// Synthetic rule
    /// </summary>
    public class AllFilesRule : RegexMatchFilesRule
    {
        public AllFilesRule()
        {
            Patterns = new List<string>() { ".*" };
        }
    }
}
