using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Quokka.CS2C.CodeModels.C;

namespace Quokka.CS2C.Translator.Tools
{
    public static class SyntaxNodeTools
    {
        public static bool HasModifier(this SyntaxTokenList node, string modifier)
        {
            return node.Any(m => m.ValueText == modifier);
        }

        public static AccessTypeCModel ExtractAccessType(this SyntaxTokenList node)
        {
            if (node.HasModifier("public"))
                return AccessTypeCModel.Public;

            if (node.HasModifier("protected"))
                return AccessTypeCModel.Protected;

            return AccessTypeCModel.Private;
        }

        public static InstanceTypeCModel ExtractInstanceType(this SyntaxTokenList node)
        {
            if (node.HasModifier("static"))
                return InstanceTypeCModel.Static;

            return InstanceTypeCModel.NonStatic;
        }
    }
}
