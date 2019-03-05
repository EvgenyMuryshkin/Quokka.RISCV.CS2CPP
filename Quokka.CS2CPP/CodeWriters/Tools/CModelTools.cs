using Quokka.CS2CPP.CodeModels.CPP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quokka.CS2CPP.CodeWriters.Tools
{
    public static class CPPModelTools
    {
        public static string Modifiers(ModifiersCPPModel modifiers)
        {
            var list = new List<string>();

            if (modifiers.AccessType != AccessTypeCPPModel.None)
                list.Add($"{modifiers.AccessType.ToString().ToLower()}:");

            if (modifiers.InstanceType == InstanceTypeCPPModel.Static)
                list.Add("static");

            if (modifiers.OverloadType != OverloadTypeCPPModel.None)
            {
                list.Add(modifiers.OverloadType.ToString().ToLower());
            }

            return string.Join(" ", list.Where(v => !string.IsNullOrWhiteSpace(v)));
        }
    }
}
