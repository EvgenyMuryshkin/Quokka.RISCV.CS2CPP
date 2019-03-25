using Microsoft.CodeAnalysis.CSharp.Syntax;
using Quokka.RISCV.CS2CPP.CodeModels.CPP;
using Quokka.RISCV.CS2CPP.Translator.Resolvers;
using Quokka.RISCV.CS2CPP.Translator.Tools;
using System.Reflection;

namespace Quokka.RISCV.CS2CPP.Translator.Visitors
{
    class ClassMembersVisitor : CSharp2CVisitor
    {
        public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            Invoke<MethodVisitor>(node);
        }
    }
}
