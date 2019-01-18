using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Quokka.RISCV.Integration.RuleHandlers
{
    public class ExtensionMatchFilesRuleHandler : RuleHandler
    {
        private List<string> _extensions;

        public ExtensionMatchFilesRuleHandler(
            string rootPath,
            IEnumerable<string> extensions) : base(rootPath)
        {
            _extensions = extensions
                ?.Select(e => e.StartsWith(".") ? e : $".{e}")
                .ToList() ?? throw new ArgumentNullException(nameof(extensions));
        }

        public override IEnumerable<string> MatchingFiles()
        {
            return AllFiles.Where(f => _extensions.Any(e => e.ToLower() == Path.GetExtension(f).ToLower()));
        }
    }
}
