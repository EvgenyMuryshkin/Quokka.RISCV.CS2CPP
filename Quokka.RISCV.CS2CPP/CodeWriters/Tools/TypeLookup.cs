using System;
using System.Collections.Generic;
using System.Text;

namespace Quokka.RISCV.CS2CPP.CodeWriters.Tools
{
    static class TypeLookup
    {
        static Dictionary<Type, string> nativeTypes = new Dictionary<Type, string>()
        {
            { typeof(void), "void" },
            { typeof(int), "int32_t" },
            { typeof(uint), "uint32_t" },
            { typeof(float), "float" },
            { typeof(double), "double" },
            { typeof(short), "int16_t" },
            { typeof(ushort), "uint16_t" },
            { typeof(char), "wchar_t" },
            { typeof(sbyte), "int8_t" },
            { typeof(byte), "uint8_t" },
            { typeof(long), "int64_t" },
            { typeof(ulong), "uint64_t" },
        };

        public static string LookupCPPTypeName(Type type)
        {
            if (nativeTypes.ContainsKey(type))
                return nativeTypes[type];

            if (type.IsClass || type.IsEnum)
                return type.Name;

            throw new Exception($"Unsupported type {type.Name}");
        }
    }
}
