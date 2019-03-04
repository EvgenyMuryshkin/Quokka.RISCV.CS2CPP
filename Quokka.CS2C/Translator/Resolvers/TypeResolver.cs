using Microsoft.CodeAnalysis.CSharp.Syntax;
using Quokka.CS2C.Translator.Tools;
using Quokka.CS2C.Translator.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Quokka.CS2C.Translator.Resolvers
{
    public class TypeResolver
    {
        public TranslationContext Context { get; set; }

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
