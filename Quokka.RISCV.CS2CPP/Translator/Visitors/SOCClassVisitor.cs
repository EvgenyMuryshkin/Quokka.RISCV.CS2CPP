using Microsoft.CodeAnalysis.CSharp.Syntax;
using Quokka.RISCV.CS2CPP.CodeModels.CPP;
using Quokka.RISCV.CS2CPP.Translator.Tools;
using System;

namespace Quokka.RISCV.CS2CPP.Translator.Visitors
{
    class SOCClassVisitor : CSharp2CVisitor
    {
        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            var classType = TypeResolver.ResolveType(node);
            var classInstance = Activator.CreateInstance(classType);

            foreach (var prop in classType.GetProperties())
            {
                if (!prop.GetGetMethod().IsStatic)
                {
                    int length = 0;
                    var type = prop.PropertyType;
                    if (type.IsArray)
                    {
                        var value = prop.GetValue(classInstance) as Array;
                        if (value == null)
                            throw new Exception($"SOC Arrays must be initialized to a value of known size");

                        length = value.Length;
                        type = type.GetElementType();
                    }

                    Context.SOCResources.Add(new SOCResourceCPPModel()
                    {
                        Name = $"{classType.Namespace}_{classType.Name}_{prop.Name}",
                        Type = type,
                        Length = length,
                    });
                }
            }

            foreach (var field in classType.GetFields())
            {
                if (!field.IsStatic)
                {
                    int length = 0;
                    var type = field.FieldType;
                    if (type.IsArray)
                    {
                        var value = field.GetValue(classInstance) as Array;
                        if (value == null)
                            throw new Exception($"SOC Arrays must be initialized to a value of known size");

                        length = value.Length;
                        type = type.GetElementType();
                    }

                    Context.SOCResources.Add(new SOCResourceCPPModel()
                    {
                        Name = $"{classType.Namespace}_{classType.Name}_{field.Name}",
                        Type = type,
                        Length = length,
                    });
                }
            }
        }
    }
}
