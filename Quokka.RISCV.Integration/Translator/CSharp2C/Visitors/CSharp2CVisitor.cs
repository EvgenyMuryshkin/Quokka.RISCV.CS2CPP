using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quokka.RISCV.Integration.Translator.CSharp2C.Visitors
{
    public abstract class CSharp2CVisitor : CSharpSyntaxVisitor
    {
        public override void DefaultVisit(SyntaxNode node)
        {
            throw new Exception($"Unsupported node type {node.GetType().Name}: {node}");
        }
    }
}
