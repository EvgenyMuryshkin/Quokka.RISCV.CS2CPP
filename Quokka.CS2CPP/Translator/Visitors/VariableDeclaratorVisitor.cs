using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Quokka.CS2CPP.CodeModels.CPP;
using Quokka.CS2CPP.Translator.Tools;
using System;

namespace Quokka.CS2CPP.Translator.Visitors
{
    class VariableDeclaratorVisitor : CSharp2CVisitor
    {
        public DataInitializerCPPModel Initializer;

        public override void VisitVariableDeclarator(VariableDeclaratorSyntax node)
        {
            Initializer = new DataInitializerCPPModel()
            {
                Name = node.Identifier.ToString(),
                Initializer = Invoke<ExpressionVisitor>(node.Initializer?.Value).Expression
            };
        }
    }  
}
