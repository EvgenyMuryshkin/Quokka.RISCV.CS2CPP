using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Quokka.RISCV.CS2CPP.CodeModels.CPP;
using Quokka.RISCV.CS2CPP.Translator.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Quokka.RISCV.CS2CPP.Translator.Visitors
{
    class ExpressionVisitor : CSharp2CVisitor
    {
        public ExpressionCPPModel Expression { get; set; }

        public override void VisitLiteralExpression(LiteralExpressionSyntax node)
        {
            Expression = new LiteralExpressionCPPModel()
            {
                Value = node.Token.ToString()
            };
        }

        public override void VisitIdentifierName(IdentifierNameSyntax node)
        {
            Expression = new IdentifierExpressionCPPModel()
            {
                Identifier = node.Identifier.ToString()
            };
        }

        static Dictionary<string, BinaryExpressionTypeCPPModel> _binaryExpressionLookup = new Dictionary<string, BinaryExpressionTypeCPPModel>()
        {
            { "==",  BinaryExpressionTypeCPPModel.Equal },
            { "!=",  BinaryExpressionTypeCPPModel.NotEqual },
            { "<",  BinaryExpressionTypeCPPModel.Less },
            { "<=",  BinaryExpressionTypeCPPModel.LessOrEqual },
            { ">",  BinaryExpressionTypeCPPModel.Greater },
            { ">=",  BinaryExpressionTypeCPPModel.GreaterOrEqual },
            { "+",  BinaryExpressionTypeCPPModel.Add },
            { "-",  BinaryExpressionTypeCPPModel.Sub },
            { "*",  BinaryExpressionTypeCPPModel.Mult },
            { "/",  BinaryExpressionTypeCPPModel.Div },
        };

        BinaryExpressionTypeCPPModel ToBinaryExpressionType(SyntaxNode node, string op)
        {
            if (!_binaryExpressionLookup.ContainsKey(op))
                Unsupported(node, $"Unsupported binary operation type: {op}");

            return _binaryExpressionLookup[op];
        }

        static Dictionary<string, UnaryExpressionTypeCPPModel> _unaryExpressionLookup = new Dictionary<string, UnaryExpressionTypeCPPModel>()
        {
            { "++",  UnaryExpressionTypeCPPModel.Inclement },
            { "--",  UnaryExpressionTypeCPPModel.Decrement },
        };

        UnaryExpressionTypeCPPModel ToUnaryExpressionType(SyntaxNode node, string op)
        {
            if (!_unaryExpressionLookup.ContainsKey(op))
                Unsupported(node, $"Unsupported unary operation type: {op}");

            return _unaryExpressionLookup[op];
        }

        public override void VisitBinaryExpression(BinaryExpressionSyntax node)
        {
            Expression = new BinaryExpressionCPPModel()
            {
                Left = Invoke<ExpressionVisitor>(node.Left).Expression,
                Right = Invoke<ExpressionVisitor>(node.Right).Expression,
                Type = ToBinaryExpressionType(node, node.OperatorToken.ToString())
            };
        }

        public override void VisitPrefixUnaryExpression(PrefixUnaryExpressionSyntax node)
        {
            Expression = new PrefixUnaryExpressionCPPModel()
            {
                Operand = Invoke<ExpressionVisitor>(node.Operand).Expression,
                Type = ToUnaryExpressionType(node, node.OperatorToken.ToString())
            };
        }

        public override void VisitPostfixUnaryExpression(PostfixUnaryExpressionSyntax node)
        {
            Expression = new PostfixUnaryExpressionCPPModel()
            {
                Operand = Invoke<ExpressionVisitor>(node.Operand).Expression,
                Type = ToUnaryExpressionType(node, node.OperatorToken.ToString())
            };
        }

        public override void VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            var methodInfo = TypeResolver.ResolveMethodInfo(node);
            Expression = new LocalInvocationCPPModel()
            {
                Method = methodInfo.Name,
                Arguments = node
                                .ArgumentList
                                .Arguments
                                .Select(arg => 
                                    new ArgumentCPPModel()
                                    {
                                        Expression = Invoke<ExpressionVisitor>(arg.Expression).Expression
                                    })
                                .ToList()
                
            };
        }

        public override void VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
        {
            // check if access to DMA property
            var identifiers = node.RecursiveFlatIdentifiers();

            if (identifiers.Any() && identifiers[0].ToString() == "DMA")
            {
                var dmaType = TypeResolver.ResolveType(identifiers[0]);
                Expression = new IdentifierExpressionCPPModel()
                {
                    Identifier = $"{dmaType.Namespace}_{dmaType.Name}_{identifiers.Last().Identifier}"
                };

                return;
            }

            Unsupported(node, "Unsupported member access expression");
        }

        public override void VisitElementAccessExpression(ElementAccessExpressionSyntax node)
        {
            var member = Invoke<ExpressionVisitor>(node.Expression);
            var access = node.ArgumentList.Arguments.Select(arg => Invoke<ExpressionVisitor>(arg).Expression);

            Expression = new ElementAccessCPPModel()
            {
                Expression = Invoke<ExpressionVisitor>(node.Expression).Expression,
                Arguments = node.ArgumentList.Arguments.Select(arg => Invoke<ExpressionVisitor>(arg.Expression).Expression).ToList()
            };
        }

        public override void VisitCastExpression(CastExpressionSyntax node)
        {
            Expression = new CastCPPModel()
            {
                Type = TypeResolver.ResolveType(node.Type),
                Expression = Invoke<ExpressionVisitor>(node.Expression).Expression,
            };
        }

        public override void VisitParenthesizedExpression(ParenthesizedExpressionSyntax node)
        {
            Expression = new ParenthesizedExpressionCPPModel()
            {
                Expression = Invoke<ExpressionVisitor>(node.Expression).Expression,
            };
        }
    }  
}
