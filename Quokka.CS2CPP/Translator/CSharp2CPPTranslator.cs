using Microsoft.CodeAnalysis.CSharp;
using Quokka.CS2CPP.Translator.Tools;
using Quokka.CS2CPP.Translator.Visitors;
using Quokka.RISCV.Integration.DTO;
using System.IO;
using System.Linq;

namespace Quokka.CS2CPP.Translator
{
    public class CSharp2CPPTranslator
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

                result.Files.Add(new FSTextFile() { Name = $"{fileName}.h", Content = content["h"] });
                result.Files.Add(new FSTextFile() { Name = $"{fileName}.cpp", Content = content["cpp"] });
            }

            return result;
        }
    }
}
