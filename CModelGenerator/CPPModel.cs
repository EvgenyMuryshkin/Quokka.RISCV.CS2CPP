using System;
using System.Collections.Generic;
using System.Text;

namespace metadata
{
    public abstract class CPPModel
    {
    }

    public abstract class CodeCPPModel : CPPModel
    {

    }

    public interface IMembersContainerCPPModel
    {
        List<CPPModel> Members { get; set; }
    }

    public enum IncludeTypeCPPModel
    {
        None,
        System,
        User,
    }

    public class IncludeCPPModel : CodeCPPModel
    {
        public IncludeTypeCPPModel Type { get; set; }
        public string Name { get; set; }
    }

    public enum AccessTypeCPPModel
    {
        None,
        Private,
        Protected,
        Public,        
    }

    public enum InstanceTypeCPPModel
    {
        None,
        Static,
        NonStatic
    }

    public enum OverloadTypeCPPModel
    {
        None,
        Abstract,
        Virtual
    }

    public class ModifiersCPPModel : CPPModel
    {
        public AccessTypeCPPModel AccessType { get; set; }
        public InstanceTypeCPPModel InstanceType { get; set; }
        public OverloadTypeCPPModel OverloadType { get; set; }
    }

    public abstract class MemberCPPModel : CPPModel
    {
        public string Name { get; set; }
    }

    public class FieldCPPModel : MemberCPPModel
    {
        public ModifiersCPPModel Modifiers { get; set; }
        public Type FieldType { get; set; }
    }

    public class DataCPPModel : MemberCPPModel
    {
        public Type DataType { get; set; }
    }

    public class ArgumentCPPModel : MemberCPPModel
    {
    }

    public class MethodCPPModel : MemberCPPModel, IMembersContainerCPPModel
    {
        public Type ReturnType { get; set; }

        public ModifiersCPPModel Modifiers { get; set; }
        public List<ArgumentCPPModel> Arguments { get; set; }
        public List<CPPModel> Members { get; set; }
    }

    public class ClassCPPModel : MemberCPPModel, IMembersContainerCPPModel
    {
        public ModifiersCPPModel Modifiers { get; set; }
        public List<CPPModel> Members { get; set; }
    }

    public class NamespaceCPPModel : CPPModel, IMembersContainerCPPModel
    {
        public string Namespace { get; set; }
        public List<CPPModel> Members { get; set; }
    }

    public class FileCPPModel : CPPModel, IMembersContainerCPPModel
    {
        public List<CPPModel> Members { get; set; }
    }
}
