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
        public override void VisitVariableDeclarator(VariableDeclaratorSyntax node)
        {
            var data = new DataCPPModel();
            data.DataType = TypeResolver.ResolveType(node.RecursizeParent<VariableDeclarationSyntax>().Type);
            data.Name = node.Identifier.ToString();
            data.Initializer = Invoke<ExpressionVisitor>(node.Initializer?.Value).Expression;
            Context.MembersContainer.Members.Add(data);
        }
    }  
}
