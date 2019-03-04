using System;
using System.Collections.Generic;
using System.Text;

namespace metadata
{
    public abstract class CModel
    {
    }

    public abstract class CodeCModel : CModel
    {

    }

    public interface IMembersContainerCModel
    {
        List<CModel> Members { get; set; }
    }

    public enum IncludeTypeCModel
    {
        None,
        System,
        User,
    }

    public class IncludeCModel : CodeCModel
    {
        public IncludeTypeCModel Type { get; set; }
        public string Name { get; set; }
    }

    public enum AccessTypeCModel
    {
        None,
        Private,
        Protected,
        Public,        
    }

    public enum InstanceTypeCModel
    {
        None,
        Static,
        NonStatic
    }

    public enum OverloadTypeCModel
    {
        None,
        Abstract,
        Virtual
    }

    public class ModifiersCModel : CModel
    {
        public AccessTypeCModel AccessType { get; set; }
        public InstanceTypeCModel InstanceType { get; set; }
        public OverloadTypeCModel OverloadType { get; set; }
    }

    public abstract class MemberCModel : CModel
    {
        public string Name { get; set; }
    }

    public class FieldCModel : MemberCModel
    {
        public ModifiersCModel Modifiers { get; set; }
        public Type FieldType { get; set; }
    }

    public class DataCModel : MemberCModel
    {
        public Type DataType { get; set; }
    }

    public class ArgumentCModel : MemberCModel
    {
    }

    public class MethodCModel : MemberCModel, IMembersContainerCModel
    {
        public Type ReturnType { get; set; }

        public ModifiersCModel Modifiers { get; set; }
        public List<ArgumentCModel> Arguments { get; set; }
        public List<CModel> Members { get; set; }
    }

    public class ClassCModel : MemberCModel, IMembersContainerCModel
    {
        public ModifiersCModel Modifiers { get; set; }
        public List<CModel> Members { get; set; }
    }

    public class NamespaceCModel : CModel, IMembersContainerCModel
    {
        public string Namespace { get; set; }
        public List<CModel> Members { get; set; }
    }

    public class FileCModel : CModel, IMembersContainerCModel
    {
        public List<CModel> Members { get; set; }
    }
}
