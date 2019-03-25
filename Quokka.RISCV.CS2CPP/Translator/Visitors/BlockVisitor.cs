using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Quokka.RISCV.CS2CPP.CodeModels.CPP;
using Quokka.RISCV.CS2CPP.Translator.Tools;
using System;
using System.Linq;

namespace Quokka.RISCV.CS2CPP.Translator.Visitors
{
    class BlockVisitor : CSharp2CVisitor
    {
        public override void VisitBlock(BlockSyntax node)
        {
            VisitChildren(node.Statements);
        }

        public override void VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
        {
            var data = Invoke<VariableDeclarationVisitor>(node.Declaration).Data;
            Context.MembersContainer.Members.Add(data);
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

        public override void VisitReturnStatement(ReturnStatementSyntax node)
        {
            Context.MembersContainer.Members.Add(new ReturnExpresionCPPModel()
            {
                Expression = Invoke<ExpressionVisitor>(node.Expression).Expression,
            });
        }

        public override void VisitExpressionStatement(ExpressionStatementSyntax node)
        {
            Visit(node.Expression);
        }

        public override void VisitPostfixUnaryExpression(PostfixUnaryExpressionSyntax node)
        {
            Context.MembersContainer.Members.Add(Invoke<ExpressionVisitor>(node).Expression);
        }

        public override void VisitPrefixUnaryExpression(PrefixUnaryExpressionSyntax node)
        {
            Context.MembersContainer.Members.Add(Invoke<ExpressionVisitor>(node).Expression);
        }

        public override void VisitAssignmentExpression(AssignmentExpressionSyntax node)
        {
            Context.MembersContainer.Members.Add(new AssignmentExpressionCPPModel()
            {
                Left = Invoke<ExpressionVisitor>(node.Left).Expression,
                Right = Invoke<ExpressionVisitor>(node.Right).Expression
            });
        }

        public override void VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            Context.MembersContainer.Members.Add(Invoke<ExpressionVisitor>(node).Expression);
        }

        public override void VisitForStatement(ForStatementSyntax node)
        {
            var forModel = new ForLoopCPPModel()
            {
                Declaration = Invoke<VariableDeclarationVisitor>(node.Declaration).Data,
                Initializers = node.Initializers.Select(i => Invoke<VariableDeclaratorVisitor>(i).Initializer).ToList(),
                Condition = Invoke<ExpressionVisitor>(node.Condition).Expression,
                Incrementors = node.Incrementors.Select(i => Invoke<ExpressionVisitor>(i).Expression).ToList()
            };

            using (Context.WithCodeContainer(forModel))
            {
                Visit(node.Statement);
            }
        }
    }
}
