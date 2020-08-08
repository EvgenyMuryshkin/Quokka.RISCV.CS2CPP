using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Quokka.RISCV.CS2CPP.Translator.Tools;
using Quokka.RISCV.CS2CPP.Translator.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Quokka.RISCV.CS2CPP.Translator.Resolvers
{
    public class TypeResolver
    {
        public TranslationContext Context { get; set; }
        BindingFlags AllBindings = BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        public Type ResolveType(ExpressionSyntax node)
        {
            var symbolInfo = Context.SemanticModel.GetSymbolInfo(node);

            switch (symbolInfo.Symbol)
            {
                case IFieldSymbol ifs:
                {
                    var fullName = $"{symbolInfo.Symbol.ContainingNamespace}.{ifs.Type.Name}";
                    var type = Type.GetType(fullName);

                    if (type == null)
                    {
                        type = Context.Library.ProjectAssembly.ExportedTypes.SingleOrDefault(t => t.FullName == fullName);
                    }

                    return type;
                }
                default:
                {
                    var fullName = $"{symbolInfo.Symbol.ContainingNamespace}.{symbolInfo.Symbol.Name}";
                    var type = Type.GetType(fullName);

                    if (type == null)
                    {
                        type = Context.Library.ProjectAssembly.ExportedTypes.SingleOrDefault(t => t.FullName == fullName);
                    }

                    return type;
                }
            }
        }

        public Type ResolveType(TypeDeclarationSyntax node)
        {
            var ns = node.RecursizeParent<NamespaceDeclarationSyntax>().Name;
            var fullName = $"{ns}.{node.Identifier}";

            var type = Context.Library.ProjectAssembly.ExportedTypes.SingleOrDefault(t => t.FullName == fullName);

            return type;
        }

        public PropertyInfo ResolvePropertyInfo(PropertyDeclarationSyntax node)
        {
            if (!TryResolvePropertyInfo(node, out PropertyInfo result))
                throw new Exception($"Failed to resolve PropertyInfo: {node.ToString()}");

            return result;
        }

        public FieldInfo ResolveFieldInfo(VariableDeclaratorSyntax node)
        {
            if (!TryResolveFieldInfo(node, out FieldInfo result))
                throw new Exception($"Failed to resolve FieldInfo: {node.ToString()}");

            return result;
        }

        public MethodInfo ResolveMethodInfo(InvocationExpressionSyntax node)
        {
            if (!TryResolveMethodInfo(node, out MethodInfo result))
                throw new Exception($"Failed to resolve MethodInfo: {node.Expression.ToString()}");

            return result;
        }

        public bool TryResolvePropertyInfo(
            PropertyDeclarationSyntax node,
            out PropertyInfo result)
        {
            var classNode = node.RecursizeParent<ClassDeclarationSyntax>();
            var classType = ResolveType(classNode);

            result = classType.GetProperties(AllBindings).FirstOrDefault(p => p.Name == node.Identifier.ToString());

            return result != null;
        }


        public bool TryResolveFieldInfo(
            VariableDeclaratorSyntax node,
            out FieldInfo result)
        {
            var classNode = node.RecursizeParent<ClassDeclarationSyntax>();
            var classType = ResolveType(classNode);

            result = classType.GetFields(AllBindings).FirstOrDefault(p => p.Name == node.Identifier.ToString());

            return result != null;
        }

        public bool TryResolveMethodInfo(
            InvocationExpressionSyntax node,
            out MethodInfo result)
        {
            result = null;

            var resolver = new ResolveUsingsAndNamespacesVisitor();
            resolver.Visit(node);

            switch(node.Expression)
            {
                case IdentifierNameSyntax ins:
                    // simple invocation like method() on current code block
                    return Context.Library.TryResolveMethod(resolver.Namespaces.Concat(new[] { ins.Identifier.ValueText }), out result);
            }

            return false;
        }

        public MethodInfo ResolveMethodInfo(MethodDeclarationSyntax node)
        {
            if (!TryResolveMethodInfo(node, out MethodInfo result))
                throw new Exception($"Failed to resolve MethodInfo: {node.Identifier.ValueText}");

            return result;
        }

        public bool TryResolveMethodInfo(
            MethodDeclarationSyntax node,
            out MethodInfo result)
        {
            result = null;

            var resolver = new ResolveUsingsAndNamespacesVisitor();
            resolver.Visit(node);

            return Context.Library.TryResolveMethod(resolver.Namespaces.Concat(new[] { node.Identifier.ValueText}), out result);
        }
    }
}
