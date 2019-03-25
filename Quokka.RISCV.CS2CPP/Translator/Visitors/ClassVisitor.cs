using Microsoft.CodeAnalysis.CSharp.Syntax;
using Quokka.RISCV.CS2CPP.CodeModels.CPP;
using Quokka.RISCV.CS2CPP.Translator.Tools;

namespace Quokka.RISCV.CS2CPP.Translator.Visitors
{
    class ClassVisitor : CSharp2CVisitor
    {
        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            if (node.Identifier.ToString() == "DMA")
            {
                Invoke<DMAClassVisitor>(node);
                return;
            }

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
