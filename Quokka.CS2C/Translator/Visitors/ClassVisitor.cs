using Microsoft.CodeAnalysis.CSharp.Syntax;
using Quokka.CS2C.CodeModels.C;
using Quokka.CS2C.Translator.Tools;

namespace Quokka.CS2C.Translator.Visitors
{
    class ClassVisitor : CSharp2CVisitor
    {
        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            var _class = new ClassCModel(Modifiers: new ModifiersCModel());

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
