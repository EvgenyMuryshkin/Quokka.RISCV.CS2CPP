using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Quokka.CS2CPP.CodeModels.CPP;
using Quokka.CS2CPP.Translator.Tools;
using System;

namespace Quokka.CS2CPP.Translator.Visitors
{
    class VariableDeclarationVisitor : CSharp2CVisitor
    {
        public override void VisitVariableDeclaration(VariableDeclarationSyntax node)
        {
            foreach (var variable in node.Variables)
            {
                Invoke<VariableDeclaratorVisitor>(variable);
            }
        }
    }  
}
