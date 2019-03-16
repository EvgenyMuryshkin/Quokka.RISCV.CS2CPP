using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Quokka.CS2CPP.CodeModels.CPP;
using Quokka.CS2CPP.CodeWriters.CPP;
using System;
using System.Linq;
using System.Text;

namespace Quokka.CS2CPP.Translator.Visitors
{
    class SourceFileVisitor : CSharp2CVisitor
    {
        public override void VisitUsingDirective(UsingDirectiveSyntax node)
        {
            Context.Usings.Add(node.Name.ToString());
        }

        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            Resolve<ClassVisitor>().Visit(node);
        }

        public override void VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
        {
            using (Context.WithUsings())
            {
                Context.Namespaces.Push(node.Name.ToString());

                using (Context.WithCodeContainer(new NamespaceCPPModel(Namespace: node.Name.ToString())))
                {
                    VisitChildren(node.Members);
                }

                Context.Namespaces.Pop();
            }
        }

        public override void VisitCompilationUnit(CompilationUnitSyntax node)
        {
            using (Context.WithUsings())
            {
                var systemIncludes = new[]
                {
                    "stdint",
                    "stdbool"
                };

                Context.Models.AddRange(systemIncludes.Select(i => new IncludeCPPModel() { Name = i, Type = IncludeTypeCPPModel.System }));

                var userIncludes = new[]
                {
                    "dma"
                };
                Context.Models.AddRange(userIncludes.Select(i => new IncludeCPPModel() { Name = i, Type = IncludeTypeCPPModel.User }));

                VisitChildren(node);
            }
        }

        public string Translate(
            TranslationContext context,
            SyntaxTree sourceFile)
        {
            Context = context;

            var csTree = (CSharpSyntaxTree)sourceFile;
            var csRoot = csTree.GetCompilationUnitRoot();

            Visit(csRoot);

            return FileCPPModelVisitor.Translate(new FileCPPModel() { Members = Context.Models });
        }
    }
}
