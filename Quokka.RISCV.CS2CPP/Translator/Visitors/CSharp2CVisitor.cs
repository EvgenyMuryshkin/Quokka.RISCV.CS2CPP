using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Quokka.RISCV.CS2CPP.Translator.Resolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quokka.RISCV.CS2CPP.Translator.Visitors
{
    public abstract class CSharp2CVisitor : CSharpSyntaxVisitor
    {
        protected TranslationContext Context;

        protected TypeResolver TypeResolver => new TypeResolver() { Context = Context };

        protected T Resolve<T>() where T : CSharp2CVisitor, new()
        {
            return new T() { Context = Context };
        }

        protected T Invoke<T>(SyntaxNode node) where T : CSharp2CVisitor, new()
        {
            var visitor = Resolve<T>();

            if (node != null)
                visitor.Visit(node);

            return visitor;
        }

        protected List<T> Invoke<T>(IEnumerable<SyntaxNode> nodes) where T : CSharp2CVisitor, new()
        {
            var result = new List<T>();

            foreach (var node in nodes)
            {
                result.Add(Invoke<T>(node));
            }

            return result;
        }

        protected void EnsureNullOrEmpty(SyntaxNode node, string message)
        {
            if (node != null)
                throw new Exception(message);
        }

        protected void Unsupported(SyntaxNode node, string message)
        {
            var exceptionMessage = $"{message ?? ""}{Environment.NewLine}{node.CallerStackDump()}";

            throw new Exception($"{GetType().Name}: Unsupported node: {exceptionMessage}");
        }

        protected void VisitChildren(IEnumerable<SyntaxNode> children)
        {
            foreach (var child in children)
            {
                Visit(child);
            }
        }
        protected void VisitChildren(SyntaxNode node) => VisitChildren(node.ChildNodes());

        public override void DefaultVisit(SyntaxNode node)
        {
            throw new Exception($"{GetType().Name}: Unsupported node type {node.GetType().Name}: {node}");
        }
    }
}
