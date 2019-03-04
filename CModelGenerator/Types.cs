using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CModelGenerator
{
    public static class Types
    {
        public static Type MemberType(MemberInfo mi)
        {
            switch (mi)
            {
                case PropertyInfo pi: return pi.PropertyType;
                case FieldInfo fi: return fi.FieldType;
                default: throw new Exception($"Unsuported member type {mi}");
            }
        }

        public static string DefaultPropertyInitializer(Type t)
        {
            var value = DefaultInitializer(t);

            return value != "" ? $" = {value};" : "";
        }

        public static string DefaultFieldInitialier(Type t)
        {
            var value = DefaultInitializer(t);

            return value != "" ? $" = {value}" : "";
        }

        public static string DefaultInitializer(Type t)
        {
            if (t == typeof(string))
                return "\"\"";

            if (t.IsEnum)
                return "0";

            if (t.IsConstructedGenericType && typeof(List<>).IsAssignableFrom(t.GetGenericTypeDefinition()))
            {
                return $"new {ResolveTypeName(t)}()";
            }

            return "null";
        }

        public static string DefaultCtorValue(Type t)
        {
            if (t == typeof(string))
                return "\"\"";

            if (t.IsEnum)
                return "0";

            if (t.IsValueType)
                return "0";

            return "null";
        }

        public static string ResolveTypeName(Type t)
        {
            if (t.IsConstructedGenericType)
            {
                var genericType = t.GetGenericTypeDefinition();
                var genericTypeName = string.Join("", genericType.Name.TakeWhile(c => c != '`'));
                return $"{genericTypeName}<{string.Join(", ", t.GetGenericArguments().Select(a => ResolveTypeName(a)))}>";
            }

            return t.Name;
        }
    }
}
