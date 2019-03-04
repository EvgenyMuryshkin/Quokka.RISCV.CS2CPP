using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using System.Reflection;
using System.IO;
using Microsoft.CodeAnalysis.Emit;

namespace Quokka.CS2C.Translator.Tools
{
    public class AssemblyBuilder
    {
        public ComponentsLibrary Build(IEnumerable<SyntaxTree> compilationTrees)
        {
            ComponentsLibrary Library = new ComponentsLibrary();

            var assemblyTempName = Guid.NewGuid().ToString();
            string path = Path.Combine(Path.GetTempPath(), $"{assemblyTempName}.dll");

            try
            {
                var systemRefLocation = typeof(object).GetTypeInfo().Assembly.Location;
                // Create a reference to the library
                var systemReference = MetadataReference.CreateFromFile(systemRefLocation);

                var assemblies = new[] 
                {
                    Assembly.GetExecutingAssembly().Location
                };

                var allAssemblies = assemblies
                    .Union(AppDomain.CurrentDomain.GetAssemblies()
                        .Where(a => !a.IsDynamic)
                        .Select(a => { try { return a.Location; } catch { return null; } })
                        .Where(p => p != null))
                    .ToHashSet();

                var assemblyReferences = allAssemblies.Select(a =>
                {
                    try
                    {
                        //var assembly = Assembly.Load(a);
                        var assemblyReference = MetadataReference.CreateFromFile(a);

                        return assemblyReference;
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }).Where(a => a != null);

                var allReferences = new List<MetadataReference>();
                allReferences.Add(systemReference);
                allReferences.AddRange(assemblyReferences);

                var compilation = CSharpCompilation.Create(assemblyTempName)
                    .WithOptions(
                      new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                    .AddReferences(allReferences)
                    .AddSyntaxTrees(compilationTrees);

                EmitResult compilationResult = compilation.Emit(path);
                if (!compilationResult.Success)
                {
                    var errors = string.Join(",", compilationResult
                        .Diagnostics
                        .Where(d => d.Severity == DiagnosticSeverity.Error || d.IsWarningAsError));

                    throw new Exception($"Compilation failed: {errors}");
                }

                var content = File.ReadAllBytes(path);
                Library.ProjectAssembly = Assembly.Load(content);

                Library.SemanticModels = compilationTrees.Select(t => new
                {
                    Tree = t,
                    SemanticModel = compilation.GetSemanticModel(t)
                }).ToDictionary(t => t.Tree, t => t.SemanticModel);
            }
            finally
            {
                if (File.Exists(path))
                    File.Delete(path);
            }

            return Library;
        }
    }
}
