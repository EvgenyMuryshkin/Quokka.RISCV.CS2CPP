using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Quokka.CS2CPP.Translator.Visitors;

namespace Quokka.CS2CPP.Translator.Resolvers
{
    class ResolveUsingsAndNamespacesVisitor : CSharp2CVisitor
    {
        public List<string> Usings = new List<string>();
        public List<string> Namespaces = new List<string>();

        public override void DefaultVisit(SyntaxNode node)
        {
            var root = node.Parent<CompilationUnitSyntax>();

            foreach (var u in root.SelfWithChildren<UsingDirectiveSyntax>() )
            {
                if (u.StaticKeyword.RawKind != (int)SyntaxKind.None)
                {
                    Unsupported(node, $"Static using is not supported: {u.StaticKeyword}");
                }

                if (u.Alias != null)
                {
                    Unsupported(node, "Alias using is not supported");
                }

                Usings.Add(u.Name.ToString());
            }

            var parentClass = node.Parent<ClassDeclarationSyntax>();
            if (parentClass != null)
            {
                Namespaces.Insert(0, parentClass.Identifier.ValueText);
            }

            var parentNS = node.Parent<NamespaceDeclarationSyntax>();

            while (parentNS != null)
            {
                Namespaces.Insert(0, parentNS.Name.ToString());

                parentNS = parentNS.Parent<NamespaceDeclarationSyntax>();
            }
        }
    }
}
