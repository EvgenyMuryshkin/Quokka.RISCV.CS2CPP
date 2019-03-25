using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Quokka.RISCV.CS2CPP.CodeModels.CPP;
using Quokka.RISCV.CS2CPP.CodeWriters.CPP;
using Quokka.RISCV.CS2CPP.CodeWriters.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Declaration = Quokka.RISCV.CS2CPP.CodeWriters.CPP.Declaration;
using Implementation = Quokka.RISCV.CS2CPP.CodeWriters.CPP.Implementation;


namespace Quokka.RISCV.CS2CPP.Translator.Visitors
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

        public Dictionary<string, string> Translate(
            TranslationContext context,
            SyntaxTree sourceFile)
        {
            var result = new Dictionary<string, string>();

            Context = context;

            var csTree = (CSharpSyntaxTree)sourceFile;
            var csRoot = csTree.GetCompilationUnitRoot();

            Visit(csRoot);

            result["h"] = Declaration
                .FileCPPModelVisitor
                .Translate(
                    new CodeWriterContext()
                    {
                        FileName = Context.FileName,
                    }, 
                    new FileCPPModel() { Members = Context.Models }
                );

            result["cpp"] = Implementation
                .FileCPPModelVisitor
                .Translate(
                    new CodeWriterContext()
                    {
                        FileName = Context.FileName,
                    },
                    new FileCPPModel() { Members = Context.Models }
                );

            return result;
        }
    }
}
