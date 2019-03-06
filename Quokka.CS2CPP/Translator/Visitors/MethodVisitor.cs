using Microsoft.CodeAnalysis.CSharp.Syntax;
using Quokka.CS2CPP.CodeModels.CPP;
using Quokka.CS2CPP.Translator.Tools;

namespace Quokka.CS2CPP.Translator.Visitors
{
    class MethodVisitor : CSharp2CVisitor
    {
        public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            Invoke<BlockVisitor>(node.Body);
        }
    }
}
