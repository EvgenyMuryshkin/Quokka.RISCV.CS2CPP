using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Microsoft.CodeAnalysis;

namespace Quokka.CS2CPP.Translator.Tools
{
    public class ComponentsLibrary
    {
        public static Dictionary<string, Type> TypeAliases = new Dictionary<string, Type>()
        {
            { "bool", typeof(bool) },
            { "byte", typeof(byte) },
            { "sbyte", typeof(sbyte) },
            { "short", typeof(short) },
            { "ushort", typeof(ushort) },
            { "int", typeof(int) },
            { "uint", typeof(uint) },
            { "long", typeof(long) },
            { "ulong", typeof(ulong) },
            { "float", typeof(float) },
        };

        public Assembly ProjectAssembly { get; set; }

        public Dictionary<SyntaxTree, SemanticModel> SemanticModels { get; set; }

        public bool TryResoleStruct(
            IEnumerable<string> list, 
            out Type type)
        {
            var fullName = string.Join(".", list);

            type = ProjectAssembly
                .DefinedTypes
                .Where(t => t.IsValueType && !t.IsEnum)
                .SingleOrDefault(t => t.FullName == fullName);

            return type != null;
        }

		public bool TryResoleClass(
			IEnumerable<string> list,
			out Type type)
		{
			var fullName = string.Join(".", list);

			type = ProjectAssembly
				.DefinedTypes
				.Where(t => t.IsClass)
				.SingleOrDefault(t => t.FullName == fullName);

			return type != null;
		}

        public bool TryResolveMethod(IEnumerable<string> parts, out MethodInfo method)
        {
            method = null;
            if (parts.Count() < 3)
            {
                return false;
            }

            var className = string.Join(".", parts.Take(parts.Count() - 1));
            var methodName = parts.Last();

            var methods = ProjectAssembly.DefinedTypes.Select(c =>
            {
                return new
                {
                    Class = c,
                    Method = c.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).SingleOrDefault(m => m.Name == methodName)
                };
            }).Where(t => t.Method != null && t.Class.FullName == className);

            method = methods.Select(t => t.Method).SingleOrDefault();

            return method != null;
        }

        public bool TryResolveEnum(IEnumerable<string> parts, out Type enumType)
        {
            var list = parts.ToList();
            if (list.Count != 2)
            {
                enumType = null;
                return false;
            }

            var enumName = parts.Last();

            enumType = ProjectAssembly
                .DefinedTypes
                .Where(t => t.IsEnum && t.Name == enumName)
                .SingleOrDefault();

            return enumType != null;
        }
    }
}
