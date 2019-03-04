using Microsoft.CodeAnalysis.CSharp;
using Quokka.CS2C.Translator.Tools;
using Quokka.CS2C.Translator.Visitors;
using Quokka.RISCV.Integration.DTO;
using System.IO;
using System.Linq;

namespace Quokka.CS2C.Translator
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

            // compile into assembly and load into library
            var builder = new AssemblyBuilder();
            var library = builder.Build(trees.Values);

            // load assembly for type info

            // translate each source file
            foreach (var tree in trees)
            {
                var context = new TranslationContext()
                {
                    Library = library
                };

                var fileName = $"{Path.GetFileNameWithoutExtension(tree.Key)}.cpp";
                var content = new SourceFileVisitor().Translate(context, tree.Value);

                result.Files.Add(new FSTextFile() { Name = fileName, Content = content });
            }

            return result;
        }
    }
}
