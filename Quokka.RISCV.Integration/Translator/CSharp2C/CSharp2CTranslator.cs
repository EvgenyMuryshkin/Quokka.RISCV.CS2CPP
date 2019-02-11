using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Quokka.RISCV.Integration.DTO;
using Quokka.RISCV.Integration.Engine;
using Quokka.RISCV.Integration.Translator.CSharp2C.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quokka.RISCV.Integration.Translator.CSharp2C
{
    public class CSharp2CTranslator
    {
        public FSSnapshot Run(FSSnapshot source)
        {
            var result = new FSSnapshot();
            // create syntax tree
            var trees = source
                .Files
                .OfType<FSTextFile>()
                .Where(f => f.Name.EndsWith(".cs"))
                .ToDictionary(f => f.Name, f => CSharpSyntaxTree.ParseText(f.Content));

            // compile into assembly

            // load assembly for type info

            // translate each source file
            result.Files.AddRange(trees.Select(p => new FSTextFile() { Name = p.Key, Content = new SourceFileVisitor().Translate(p.Value) }));

           return result;
        }
    }
}
