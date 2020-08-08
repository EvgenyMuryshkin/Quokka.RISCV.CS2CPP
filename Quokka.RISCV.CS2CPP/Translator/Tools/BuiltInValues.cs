using System.Collections.Generic;

namespace Quokka.RISCV.CS2CPP.Translator.Tools
{
    public static class BuiltInValues
    {
        static Dictionary<string, object> mapBuildInValues = new Dictionary<string, object>()
        {
            { "char.MaxValue", char.MaxValue },
            { "char.MinValue", char.MinValue },
            { "byte.MaxValue", byte.MaxValue },
            { "byte.MinValue", byte.MinValue },
            { "sbyte.MaxValue", sbyte.MaxValue },
            { "sbyte.MinValue", sbyte.MinValue },
            { "short.MaxValue", short.MaxValue },
            { "short.MinValue", short.MinValue },
            { "ushort.MaxValue", ushort.MaxValue },
            { "ushort.MinValue", ushort.MinValue },
            { "int.MaxValue", int.MaxValue },
            { "int.MinValue", int.MinValue },
            { "uint.MaxValue", uint.MaxValue },
            { "uint.MinValue", uint.MinValue },
            { "long.MaxValue", long.MaxValue },
            { "long.MinValue", long.MinValue },
            { "ulong.MaxValue", ulong.MaxValue },
            { "ulong.MinValue", ulong.MinValue },
            { "float.MaxValue", float.MaxValue },
            { "float.MinValue", float.MinValue },
        };

        public static bool IsBuiltInValue(string value)
        {
            return mapBuildInValues.ContainsKey(value);
        }

        public static object BuiltInValue(string value)
        {
            return mapBuildInValues[value];
        }
    }
}
