using Quokka.RISCV.Integration.DTO;
using Quokka.RISCV.Integration.RuleHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quokka.RISCV.Integration.Engine
{
    public class RulesManager
    {
        string _rootPath;
        public RulesManager(string rootPath)
        {
            _rootPath = rootPath;
        }

        public List<RuleHandler> CreateRules(IEnumerable<FileRule> fileRules)
        {
            return fileRules
                .Where(rule => rule != null)
                .Select<FileRule, RuleHandler>(rule =>
                {
                    switch (rule)
                    {
                        case RegexMatchFilesRule rmf:
                        {
                            return new RegexMatchFilesRuleHandler(_rootPath, rmf.Patterns);
                        }

                        case ExtensionMatchFilesRule emf:
                        {
                            return new ExtensionMatchFilesRuleHandler(_rootPath, emf.Extensions);
                        }

                        case ModifiedFilesRule rmf:
                        {
                            return new ModifiedFilesRuleHandler(_rootPath);
                        }

                        default:
                            throw new Exception($"Unsupported rule type: {rule.GetType()}");
                    }
                })
                .ToList();
        }
    }
}
