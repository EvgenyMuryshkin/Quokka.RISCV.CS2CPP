using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Quokka.RISCV.CS2CPP.CodeModels.CPP;
using Quokka.RISCV.CS2CPP.Translator.Tools;
using System;
using System.Collections.Generic;
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

        public override void VisitIfStatement(IfStatementSyntax node)
        {
            var _if = new IfCPPModel()
            {
                Condition = Invoke<ExpressionVisitor>(node.Condition).Expression
            };

            using (Context.WithCodeContainer(_if))
            {
                Visit(node.Statement);
            }

            if (node.Else != null)
                Visit(node.Else);
        }

        public override void VisitElseClause(ElseClauseSyntax node)
        {
            var _else = new ElseCPPModel();

            using (Context.WithCodeContainer(_else))
            {
                Visit(node.Statement);
            }
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

        public override void VisitDoStatement(DoStatementSyntax node)
        {
            var _do = new DoLoopCPPModel()
            {
                Condition = Invoke<ExpressionVisitor>(node.Condition).Expression
            };

            using (Context.WithCodeContainer(_do))
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

        public override void VisitBreakStatement(BreakStatementSyntax node)
        {
            Context.MembersContainer.Members.Add(new BreakCPPModel());
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
            var assign = new AssignmentExpressionCPPModel()
            {
                Left = Invoke<ExpressionVisitor>(node.Left).Expression,
                Right = Invoke<ExpressionVisitor>(node.Right).Expression
            };

            var assignKind = (SyntaxKind)node.RawKind;

            Dictionary<SyntaxKind, AssignType> assignTypes = new Dictionary<SyntaxKind, AssignType>()
            {
                { SyntaxKind.SimpleAssignmentExpression, AssignType.Equals },
                { SyntaxKind.AddAssignmentExpression, AssignType.PlusEquals },
                { SyntaxKind.SubtractAssignmentExpression, AssignType.MinusEquals },
                { SyntaxKind.MultiplyAssignmentExpression, AssignType.MultEquals },
                { SyntaxKind.DivideAssignmentExpression, AssignType.DivEquals },
            };

            if (!assignTypes.ContainsKey(assignKind))
                Unsupported(node, $"Assignment kind not supported");

            assign.Type = assignTypes[assignKind];
            
            Context.MembersContainer.Members.Add(assign);
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

        public override void VisitSwitchStatement(SwitchStatementSyntax node)
        {
            Invoke<SwitchVisitor>(node);
        }
    }
}
