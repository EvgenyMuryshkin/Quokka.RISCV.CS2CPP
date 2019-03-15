using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Quokka.CS2CPP.Translator.Tools;
using Quokka.CS2CPP.Translator.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Quokka.CS2CPP.Translator.Resolvers
{
    public class TypeResolver
    {
        public TranslationContext Context { get; set; }

        public Type ResolveType(ExpressionSyntax node)
        {
            var symbolInfo = Context.SemanticModel.GetSymbolInfo(node);
            var type = Type.GetType($"{symbolInfo.Symbol.ContainingNamespace}.{symbolInfo.Symbol.Name}");

            return type;
        }

        public MethodInfo ResolveMethodInfo(InvocationExpressionSyntax node)
        {
            if (!TryResolveMethodInfo(node, out MethodInfo result))
                throw new Exception($"Failed to resolve MethodInfo: {node.Expression.ToString()}");

            return result;
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
