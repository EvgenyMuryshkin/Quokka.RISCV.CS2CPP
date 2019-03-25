using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Quokka.RISCV.CS2CPP.CodeModels.CPP;

namespace Quokka.RISCV.CS2CPP.Translator.Tools
{
    public static class SyntaxNodeTools
    {
        public static bool HasModifier(this SyntaxTokenList node, string modifier)
        {
            return node.Any(m => m.ValueText == modifier);
        }

        public static AccessTypeCPPModel ExtractAccessType(this SyntaxTokenList node)
        {
            if (node.HasModifier("public"))
                return AccessTypeCPPModel.Public;

            if (node.HasModifier("protected"))
                return AccessTypeCPPModel.Protected;

            return AccessTypeCPPModel.Private;
        }

        public static InstanceTypeCPPModel ExtractInstanceType(this SyntaxTokenList node)
        {
            if (node.HasModifier("static"))
                return InstanceTypeCPPModel.Static;

            return InstanceTypeCPPModel.NonStatic;
        }
    }
}
