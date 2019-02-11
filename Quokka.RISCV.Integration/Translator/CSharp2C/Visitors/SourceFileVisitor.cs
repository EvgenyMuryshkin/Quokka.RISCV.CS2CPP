using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Quokka.RISCV.Integration.Translator.CodeModels.C;
using Quokka.RISCV.Integration.Translator.CodeWriters.C;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quokka.RISCV.Integration.Translator.CSharp2C.Visitors
{
    class TranslationContext
    {
        public List<CCodeModel> Models = new List<CCodeModel>();
        public List<string> Namespaces = new List<string>();
    }

    class SourceFileVisitor : CSharp2CVisitor
    {
        TranslationContext ctx = new TranslationContext();

        private void VisitChildren(SyntaxNode node)
        {
            foreach (var child in node.ChildNodes())
            {
                Visit(child);
            }
        }

        public override void VisitUsingDirective(UsingDirectiveSyntax node)
        {
            ctx.Namespaces.Add(node.Name.ToString());
        }

        public override void VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
        {
        }

        public override void VisitCompilationUnit(CompilationUnitSyntax node)
        {
            var systemIncludes = new[]
            {
                "stdint",
                "stdbool"
            };

            ctx.Models.AddRange(systemIncludes.Select(i => new CIncludeModel() { Name = i, Type = CIncludeModelType.System }));

            VisitChildren(node);
        }

        public string Translate(SyntaxTree sourceFile)
        {
            var csTree = (CSharpSyntaxTree)sourceFile;
            var csRoot = csTree.GetCompilationUnitRoot();

            Visit(csRoot);

            return CWriter.Write(new CFileModel() { Children = ctx.Models });
        }
    }
}
