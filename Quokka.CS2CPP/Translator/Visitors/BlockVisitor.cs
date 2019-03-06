using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Quokka.CS2CPP.CodeModels.CPP;
using Quokka.CS2CPP.Translator.Tools;
using System;

namespace Quokka.CS2CPP.Translator.Visitors
{
    class BlockVisitor : CSharp2CVisitor
    {
        public override void VisitBlock(BlockSyntax node)
        {
            VisitChildren(node.Statements);
        }

        public override void VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
        {
            Invoke<VariableDeclarationVisitor>(node.Declaration);
        }

        public override void VisitWhileStatement(WhileStatementSyntax node)
        {
            var _while = new WhileLoopCPPModel()
            {
                Condition = Invoke<ExpressionVisitor>(node.Condition).Expression
            };

            using (Context.WithCodeContainer(_while))
            {
                Visit(node.Statement);
            }
        }
    }
}
