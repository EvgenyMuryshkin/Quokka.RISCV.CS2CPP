using System;
using System.Collections.Generic;
using System.Text;

namespace Quokka.CS2CPP.CodeWriters.Tools
{
    static class TypeLookup
    {
        static Dictionary<Type, string> nativeTypes = new Dictionary<Type, string>()
        {
            { typeof(void), "void" },
            { typeof(int), "int" },
            { typeof(uint), "unsigned int" },
            { typeof(float), "float" },
            { typeof(double), "double" },
            { typeof(short), "short" },
            { typeof(ushort), "unsigned short" },
            { typeof(char), "char" },
            { typeof(byte), "unsigned char" },
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
