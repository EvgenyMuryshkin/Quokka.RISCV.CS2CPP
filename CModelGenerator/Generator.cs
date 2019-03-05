using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CPPModelGenerator
{
    public class Generator
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
        void DumpInterface(Type type)
        {
            var baseAndImpl = new List<string>();

            foreach (var iface in type.GetInterfaces())
            {
                baseAndImpl.Add(Types.ResolveTypeName(iface));
            }

            var baseInterfaces = baseAndImpl.Any() ? $" : {string.Join(", ", baseAndImpl)}" : "";
            AppendLine($"public partial interface {type.Name}{baseInterfaces}");
            OpenBlock();

            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            if (fields.Any())
            {
                throw new Exception($"Interface {type.Name} should not have fields");
            }

            var members = Enumerable.Empty<MemberInfo>().Concat(props).Concat(fields);
            var args = members.Select(m =>
            {
                var memberType = Types.MemberType(m);
                return $"{Types.ResolveTypeName(memberType)} {m.Name} = {Types.DefaultCtorValue(memberType)}";
            });

            foreach (var member in props)
            {
                AppendLine($"{Types.ResolveTypeName(member.PropertyType)} {member.Name} {{ get; set; }}");
            }

            CloseBlock();
        }

        void DumpClass(Type type)
        {
            var isAbstract = type.IsAbstract ? "abstract " : "";

            var baseAndImpl = new List<string>();
            if (type.BaseType != typeof(object))
                baseAndImpl.Add(Types.ResolveTypeName(type.BaseType));

            foreach (var iface in type.GetInterfaces())
            {
                baseAndImpl.Add(Types.ResolveTypeName(iface));
            }

            var baseClass = baseAndImpl.Any() ? $" : {string.Join(", ", baseAndImpl)}" : "";

            AppendLine($"// generated class, do not modify");

            AppendLine($"public {isAbstract}partial class {type.Name}{baseClass}");
            OpenBlock();

            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            var members = Enumerable.Empty<MemberInfo>().Concat(props).Concat(fields);
            var args = members.Select(m =>
            {
                var memberType = Types.MemberType(m);
                return $"{Types.ResolveTypeName(memberType)} {m.Name} = {Types.DefaultCtorValue(memberType)}";
            });

            AppendLine($"public {type.Name}({string.Join(", ", args)})");
            OpenBlock();
            foreach (var m in members)
            {
                var memberType = Types.MemberType(m);
                if (memberType.IsClass)
                {
                    AppendLine($"this.{m.Name} = {m.Name} ?? {Types.DefaultInitializer(memberType)};");
                }
                else
                {
                    AppendLine($"this.{m.Name} = {m.Name};");
                }
            }
            CloseBlock();

            foreach (var member in props)
            {
                AppendLine($"public {Types.ResolveTypeName(member.PropertyType)} {member.Name} {{ get; set; }}{Types.DefaultPropertyInitializer(member.PropertyType)}");
            }

            foreach (var member in fields)
            {
                AppendLine($"public {Types.ResolveTypeName(member.FieldType)} {member.Name}{Types.DefaultFieldInitialier(member.FieldType)};");
            }

            CloseBlock();
        }

        public void DumpType(Type type)
        {
            if (type.IsEnum)
                DumpEnum(type);

            if (type.IsClass)
                DumpClass(type);

            if (type.IsInterface)
                DumpInterface(type);
        }

        public override string ToString()
        {
            return _builder.ToString();
        }
    }
}
