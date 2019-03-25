using Quokka.RISCV.CS2CPP.CodeModels.CPP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quokka.RISCV.CS2CPP.CodeWriters.Tools
{
    public enum ModifersFlag
    {
        AccessTypeCPPModel = 1,
        InstanceTypeCPPModel = 2,
        OverloadTypeCPPModel = 4,
    }
       
    public static class CPPModelTools
    {
        public static string Modifiers(
            ModifiersCPPModel modifiers,
            ModifersFlag flags = ModifersFlag.AccessTypeCPPModel | ModifersFlag.InstanceTypeCPPModel | ModifersFlag.OverloadTypeCPPModel)
        {
            var list = new List<string>();
            Func<ModifersFlag, bool> hasFlag = (flag) => (flags & flag) == flag;

            if (hasFlag(ModifersFlag.AccessTypeCPPModel) && modifiers.AccessType != AccessTypeCPPModel.None)
                list.Add($"{modifiers.AccessType.ToString().ToLower()}:");

            if (hasFlag(ModifersFlag.InstanceTypeCPPModel) && modifiers.InstanceType == InstanceTypeCPPModel.Static)
                list.Add("static");

            if (hasFlag(ModifersFlag.OverloadTypeCPPModel) && modifiers.OverloadType != OverloadTypeCPPModel.None)
            {
                list.Add(modifiers.OverloadType.ToString().ToLower());
            }

            return string.Join(" ", list.Where(v => !string.IsNullOrWhiteSpace(v)));
        }
    }
}
