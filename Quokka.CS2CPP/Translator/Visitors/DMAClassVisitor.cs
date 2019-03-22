using Microsoft.CodeAnalysis.CSharp.Syntax;
using Quokka.CS2CPP.CodeModels.CPP;
using Quokka.CS2CPP.Translator.Tools;
using System;

namespace Quokka.CS2CPP.Translator.Visitors
{
    class DMAClassVisitor : CSharp2CVisitor
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
                            throw new Exception($"DMA Arrays must be initialized to a value of known size");

                        length = value.Length;
                        type = type.GetElementType();
                    }

                    Context.DMAModels.Add(new DMACPPModel()
                    {
                        Name = $"{classType.Namespace}_{classType.Name}_{prop.Name}",
                        Type = type,
                        Length = length,
                    });
                }
            }
        }
    }
}
