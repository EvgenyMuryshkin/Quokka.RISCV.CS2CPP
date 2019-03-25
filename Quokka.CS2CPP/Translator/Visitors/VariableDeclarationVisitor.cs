using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Quokka.CS2CPP.CodeModels.CPP;
using Quokka.CS2CPP.Translator.Tools;
using System;
using System.Linq;

namespace Quokka.CS2CPP.Translator.Visitors
{
    class VariableDeclarationVisitor : CSharp2CVisitor
    {
        public DataCPPModel Data;
        public override void VisitVariableDeclaration(VariableDeclarationSyntax node)
        {
            Data = new DataCPPModel()
            {
                DataType = TypeResolver.ResolveType(node.Type),
                Initializers = node.Variables.Select(variable => Invoke<VariableDeclaratorVisitor>(variable).Initializer).ToList()
            };
        }
    }  
}
