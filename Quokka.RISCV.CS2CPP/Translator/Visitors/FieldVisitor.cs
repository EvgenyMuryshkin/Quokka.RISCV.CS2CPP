using Microsoft.CodeAnalysis.CSharp.Syntax;
using Quokka.RISCV.CS2CPP.CodeModels.CPP;
using System.Reflection;
using Quokka.RISCV.CS2CPP.Translator.Tools;

namespace Quokka.RISCV.CS2CPP.Translator.Visitors
{
    class FieldVisitor : CSharp2CVisitor
    {
        FieldDeclarationSyntax _fieldDeclaration;
        VariableDeclaratorSyntax _variableDeclaration;
        FieldInfo _fieldInfo;

        public override void VisitFieldDeclaration(FieldDeclarationSyntax node)
        {
            _fieldDeclaration = node;
            VisitChildren(node.Declaration.Variables);
        }

        public override void VisitVariableDeclarator(VariableDeclaratorSyntax node)
        {
            _variableDeclaration = node;
            _fieldInfo = TypeResolver.ResolveFieldInfo(node);

            var field = new FieldCPPModel(Modifiers: new ModifiersCPPModel())
            {
                FieldType = _fieldInfo.FieldType,
                Name = node.Identifier.ToString(),
            };
            field.Modifiers.AccessType = _fieldDeclaration.Modifiers.ExtractAccessType();
            field.Modifiers.InstanceType = _fieldDeclaration.Modifiers.ExtractInstanceType();

            if (node.Initializer != null)
                field.Initializer = Invoke<FieldInitializerVisitor>(node.Initializer.Value).Expression;

            Context.MembersContainer.Members.Add(field);
        }
    }
}
