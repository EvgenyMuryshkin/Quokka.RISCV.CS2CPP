using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Quokka.CS2CPP.CodeModels.CPP;
using Quokka.CS2CPP.Translator.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Quokka.CS2CPP.Translator.Visitors
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
            var identifiers = FlatIdentifiers(node);

            if (identifiers.Any() && identifiers[0].ToString() == "DMA")
            {
                var dmaType = TypeResolver.ResolveType(identifiers[0]);

                return;
            }

            Unsupported(node, "Unsupported member access expression");
        }

        List<IdentifierNameSyntax> FlatIdentifiers(SyntaxNode node)
        {
            switch(node)
            {
                case MemberAccessExpressionSyntax maes:
                    switch (maes.Name)
                    {
                        case IdentifierNameSyntax ins:
                            return FlatIdentifiers(maes.Expression).Concat(new[] { ins }).ToList();
                        default:
                            Unsupported(node, "Unsupported node name in flat identifiers");
                            return null;
                    }
                case IdentifierNameSyntax ins:
                    return new List<IdentifierNameSyntax>() { ins };
                default:
                    Unsupported(node, "Unsupoprted node type in flat identifiers");
                    return null;
            }
        }
    }  
}
