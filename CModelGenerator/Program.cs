using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CPPModelGenerator
{
    class GeneratorContext
    {
        public List<Type> AllCPPModels { get; set; }
    }

    class Program
    {
        static string SolutionFolder(string start = null)
        {
            start = start ?? Directory.GetCurrentDirectory();

            if (!Directory.Exists(start))
                return null;

            if (Directory.EnumerateFiles(start, "*.sln").Any())
                return start;

            return SolutionFolder(Path.GetDirectoryName(start));
        }

        static string Visitor(GeneratorContext context)
        {
            var builder = new Generator();
            builder.AppendLine($"// generated code, do not modify");
            builder.AppendLine("using System;");
            builder.AppendLine("using System.Collections.Generic;");

            builder.AppendLine("namespace Quokka.RISCV.CS2CPP.CodeModels.CPP");
            builder.OpenBlock();

            foreach (var type in context.AllCPPModels)
            {
                builder.DumpType(type);
            }


            // visitor
            builder.AppendLine($"public abstract partial class CPPModelVisitor : CPPModelDefaultVisitor");
            builder.OpenBlock();

            var classes = context.AllCPPModels.Where(c => c.IsClass && !c.IsAbstract);
            foreach (var c in classes)
            {
                builder.AppendLine($"public virtual void Visit{c.Name}({c.Name} model) => DefaultVisit(model);");
            }

            builder.AppendLine($"public virtual void Visit(CPPModel model)");
            builder.OpenBlock();
            builder.AppendLine($"switch(model)");
            builder.OpenBlock();
            foreach (var c in classes)
            {
                builder.AppendLine($"case {c.Name} m: Visit{c.Name}(m); break;");
            }
            builder.CloseBlock();

            builder.CloseBlock();

            builder.CloseBlock();

            builder.CloseBlock();

            return builder.ToString();
        }

        static void Main(string[] args)
        {
            Console.WriteLine($"Running at {Directory.GetCurrentDirectory()}");

            var allCPPModels = Assembly.GetExecutingAssembly().ExportedTypes.Where(t => t.Namespace == "metadata");
            var context = new GeneratorContext()
            {
                AllCPPModels = allCPPModels.ToList()
            };

            var visitor = Visitor(context);
            var path = Path.Combine(SolutionFolder(), @"Quokka.RISCV.CS2CPP\CodeModels\CPP\CPPModel.cs");
            Console.WriteLine($"Writing {path}");

            File.WriteAllText(path, visitor);

            Console.WriteLine("CS2CPP Completed!");
        }
    }
}
