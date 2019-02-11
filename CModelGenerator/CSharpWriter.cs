using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CModelGenerator
{
    public class CSharpWriter
    {
        StringBuilder _builder = new StringBuilder();

        int Indent = 0;
        string FormatIndent => string.Join("", Enumerable.Range(0, Indent).Select(i => "\t"));

        public void OpenBlock()
        {
            AppendLine("{");
            Indent++;
        }

        public void CloseBlock()
        {
            Indent--;
            AppendLine("}");
        }

        public void AppendLine(string value)
        {
            var lines = value.Split(Environment.NewLine);
            var indent = FormatIndent;

            foreach (var l in lines)
            {
                _builder.AppendLine($"{indent}{l}");
            }
        }

        void DumpEnum(Type type)
        {
            AppendLine($"public enum {type.Name}");
            OpenBlock();

            Type enumUnderlyingType = Enum.GetUnderlyingType(type);
            Array enumValues = Enum.GetValues(type);

            foreach (var index in Enumerable.Range(0, enumValues.Length))
            {
                var name = enumValues.GetValue(index).ToString();
                var value = Convert.ChangeType(enumValues.GetValue(index), enumUnderlyingType);

                AppendLine($"{name} = {value},");
            }

            CloseBlock();
        }

        void DumpClass(Type type)
        {
            var isAbstract = type.IsAbstract ? "abstract " : "";
            var baseClass = type.BaseType != typeof(object) ? $" : {type.BaseType.Name}" : "";
            AppendLine($"public {isAbstract}partial class {type.Name}{baseClass}");
            OpenBlock();

            foreach (var member in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                AppendLine($"public {ResolveStringType(member.PropertyType)} {member.Name} {{ get; set; }}{DefaultPropertyValue(member.PropertyType)}");
            }

            foreach (var member in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                AppendLine($"public {ResolveStringType(member.FieldType)} {member.Name}{DefaultFieldValue(member.FieldType)};");

            }
            CloseBlock();
        }

        string ResolveStringType(Type t)
        {
            if (t.IsConstructedGenericType)
            {
                var genericType = t.GetGenericTypeDefinition();
                var genericTypeName = string.Join("", genericType.Name.TakeWhile(c => c != '`'));
                return $"{genericTypeName}<{string.Join(", ", t.GetGenericArguments().Select(a => ResolveStringType(a)))}>";
            }

            return t.Name;
        }

        string DefaultPropertyValue(Type t)
        {
            var value = DefaultValue(t);

            return value != "" ? $" = {value};" : "";
        }

        string DefaultFieldValue(Type t)
        {
            var value = DefaultValue(t);

            return value != "" ? $" = {value}" : "";
        }

        string DefaultValue(Type t)
        {
            if (t == typeof(string))
                return "\"\"";

            if ( t.IsConstructedGenericType && typeof(List<>).IsAssignableFrom(t.GetGenericTypeDefinition()))
            {
                return $"new {ResolveStringType(t)}()";
            }

            return "";
        }

        public void DumpType(Type type)
        {
            if (type.IsEnum)
                DumpEnum(type);

            if (type.IsClass)
                DumpClass(type);            
        }

        public override string ToString()
        {
            return _builder.ToString();
        }
    }
}
