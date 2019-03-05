using Microsoft.CodeAnalysis.CSharp.Syntax;
using Quokka.CS2CPP.CodeModels.CPP;
using Quokka.CS2CPP.Translator.Tools;

namespace Quokka.CS2CPP.Translator.Visitors
{
    class ClassVisitor : CSharp2CVisitor
    {
        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            var _class = new ClassCPPModel(Modifiers: new ModifiersCPPModel());

            _class.Name = node.Identifier.ToString();
            _class.Modifiers.InstanceType = node.Modifiers.ExtractInstanceType();

            using (Context.WithCodeContainer(_class))
            {
                Invoke<ClassMembersVisitor>(node.Members);
            }

            // unsupported
            EnsureNullOrEmpty(node.TypeParameterList, "Generics to be implemented");
        }
    }
}
