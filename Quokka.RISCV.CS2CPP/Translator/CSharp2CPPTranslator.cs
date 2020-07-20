using Microsoft.CodeAnalysis.CSharp;
using Quokka.RISCV.CS2CPP.CodeModels.CPP;
using Quokka.RISCV.CS2CPP.Translator.Tools;
using Quokka.RISCV.CS2CPP.Translator.Visitors;
using Quokka.RISCV.Integration.DTO;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Quokka.RISCV.CS2CPP.Translator
{
    public class CSharp2CPPTranslator
    {
        public FSSnapshot Result = new FSSnapshot();
        public List<SOCResourceCPPModel> SOCResources = new List<SOCResourceCPPModel>();

        public void Run(FSSnapshot source)
        {
            // create syntax tree
            var trees = source
                .Files
                .OfType<FSTextFile>()
                .Where(f => f.Name.EndsWith(".cs"))
                .ToDictionary(f => f.Name, f => CSharpSyntaxTree.ParseText(f.Content));

            // compile into assembly and load into library
            var builder = new AssemblyBuilder();
            var library = builder.Build(trees.Values);

            // translate each source file
            foreach (var tree in trees)
            {
                var fileName = Path.GetFileNameWithoutExtension(tree.Key);

                var context = new TranslationContext()
                {
                    Library = library,
                    Root = tree.Value,
                    FileName = fileName,
                };

                var content = new SourceFileVisitor().Translate(context, tree.Value);

                Result.Files.Add(new FSTextFile() { Name = $"{fileName}.h", Content = content["h"] });
                Result.Files.Add(new FSTextFile() { Name = $"{fileName}.cpp", Content = content["cpp"] });

                SOCResources.AddRange(context.SOCResources);
            }
        }
    }
}
