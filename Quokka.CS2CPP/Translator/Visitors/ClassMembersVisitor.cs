using Microsoft.CodeAnalysis.CSharp.Syntax;
using Quokka.CS2CPP.CodeModels.CPP;
using Quokka.CS2CPP.Translator.Resolvers;
using Quokka.CS2CPP.Translator.Tools;
using System.Reflection;

namespace Quokka.CS2CPP.Translator.Visitors
{
    class ClassMembersVisitor : CSharp2CVisitor
    {
        public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            Invoke<MethodVisitor>(node);
        }
    }
}
