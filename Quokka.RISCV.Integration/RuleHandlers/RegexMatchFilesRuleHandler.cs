using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Quokka.RISCV.Integration.RuleHandlers
{
    public class RegexMatchFilesRuleHandler : RuleHandler
    {
        private List<string> _patterns;

        public RegexMatchFilesRuleHandler(
            string rootPath,
            IEnumerable<string> patterns) : base(rootPath)
        {
            _patterns = patterns?.ToList() ?? throw new ArgumentNullException(nameof(patterns));
        }

        public override IEnumerable<string> MatchingFiles()
        {
            return AllFiles.Where(f => _patterns.Any(p => Regex.IsMatch(f, p)));
        }
    }
}
