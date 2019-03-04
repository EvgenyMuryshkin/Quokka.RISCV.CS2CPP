using Quokka.CS2C.CodeModels.C;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quokka.CS2C.CodeWriters.Tools
{
    public static class CModelTools
    {
        public static string Modifiers(ModifiersCModel modifiers)
        {
            var list = new List<string>();

            if (modifiers.AccessType != AccessTypeCModel.None)
                list.Add($"{modifiers.AccessType.ToString().ToLower()}:");

            if (modifiers.InstanceType == InstanceTypeCModel.Static)
                list.Add("static");

            if (modifiers.OverloadType != OverloadTypeCModel.None)
            {
                list.Add(modifiers.OverloadType.ToString().ToLower());
            }

            return string.Join(" ", list.Where(v => !string.IsNullOrWhiteSpace(v)));
        }
    }
}
