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
            var methodInfo = TypeResolver.ResolveMethodInfo(node);

            var _method = new MethodCPPModel(Modifiers: new ModifiersCPPModel());
            _method.Name = node.Identifier.ToString();
            _method.ReturnType = methodInfo.ReturnType;
            _method.Modifiers.AccessType = node.Modifiers.ExtractAccessType();
            _method.Modifiers.InstanceType = node.Modifiers.ExtractInstanceType();

            using (Context.WithCodeContainer(_method))
            {
                Invoke<MethodVisitor>(node);
            }
        }
    }
}
