using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Quokka.CS2CPP.CodeModels.CPP;
using Quokka.CS2CPP.Translator.Tools;
using System;
using System.Collections.Generic;

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

        static Dictionary<string, ExpressionTypeCPPModel> _lookup = new Dictionary<string, ExpressionTypeCPPModel>()
        {
            { "==",  ExpressionTypeCPPModel.Equal },
            { "!=",  ExpressionTypeCPPModel.NotEqual },
            { "<",  ExpressionTypeCPPModel.Less },
            { "<=",  ExpressionTypeCPPModel.LessOrEqual },
            { ">",  ExpressionTypeCPPModel.Greater },
            { ">=",  ExpressionTypeCPPModel.GreaterOrEqual },
            { "+",  ExpressionTypeCPPModel.Add },
            { "-",  ExpressionTypeCPPModel.Sub },
            { "*",  ExpressionTypeCPPModel.Mult },
            { "/",  ExpressionTypeCPPModel.Div },
        };

        ExpressionTypeCPPModel ToExpressionType(SyntaxNode node, string op)
        {
            if (!_lookup.ContainsKey(op))
                Unsupported(node, $"Unsupported operation type: {op}");

            return _lookup[op];
        }

        public override void VisitBinaryExpression(BinaryExpressionSyntax node)
        {
            Expression = new BinaryExpressionCPPModel()
            {
                Left = Invoke<ExpressionVisitor>(node.Left).Expression,
                Right = Invoke<ExpressionVisitor>(node.Right).Expression,
                Type = ToExpressionType(node, node.OperatorToken.ToString())
            };
        }
    }  
}
