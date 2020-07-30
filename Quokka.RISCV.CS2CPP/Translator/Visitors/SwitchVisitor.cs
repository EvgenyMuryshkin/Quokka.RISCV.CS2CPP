using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Quokka.RISCV.CS2CPP.CodeModels.CPP;
using System.Linq;

namespace Quokka.RISCV.CS2CPP.Translator.Visitors
{
    class SwitchVisitor : CSharp2CVisitor
    {
        public override void VisitSwitchStatement(SwitchStatementSyntax node)
        {
            var _switch = new SwitchCPPModel()
            {
                Expression = Invoke<ExpressionVisitor>(node.Expression).Expression
            };

            using (Context.WithCodeContainer(_switch))
            {
                foreach (var sections in node.Sections)
                {
                    Visit(sections);
                }
            }
        }

        public override void VisitSwitchSection(SwitchSectionSyntax node)
        {
            var _case = new CaseCPPModel();

            foreach (var label in node.Labels)
            {
                switch (label)
                {
                    case CaseSwitchLabelSyntax l:
                        _case.Labels.Add(new CaseSwitchLabelCPPModel()
                        {
                            Condition = Invoke<ExpressionVisitor>(l.Value).Expression
                        });
                        break;
                    case DefaultSwitchLabelSyntax d:
                        _case.Labels.Add(new DefaultSwitchLabelCPPModel());
                        break;
                    default:
                        Visit(label);
                        break;
                }    
            }

            using (Context.WithCodeContainer(_case))
            {
                foreach (var statement in node.Statements)
                {
                    Invoke<BlockVisitor>(statement);
                }
            }
        }
    }
}
