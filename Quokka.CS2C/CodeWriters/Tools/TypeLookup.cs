using System;
using System.Collections.Generic;
using System.Text;

namespace Quokka.CS2C.CodeWriters.Tools
{
    static class TypeLookup
    {
        static Dictionary<Type, string> nativeTypes = new Dictionary<Type, string>()
        {
            { typeof(void), "void" }
        };

        public static string LookupCPPTypeName(Type type)
        {
            if (nativeTypes.ContainsKey(type))
                return nativeTypes[type];

            return type.Name;
        }
    }
}
