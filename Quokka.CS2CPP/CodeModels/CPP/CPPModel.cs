// generated code, do not modify
using System;
using System.Collections.Generic;
namespace Quokka.CS2CPP.CodeModels.CPP
{
	// generated class, do not modify
	public abstract partial class CPPModel
	{
		public CPPModel()
		{
		}
	}
	// generated class, do not modify
	public abstract partial class CodeCPPModel : CPPModel
	{
		public CodeCPPModel()
		{
		}
	}
	public partial interface IMembersContainerCPPModel
	{
		List<CPPModel> Members { get; set; }
	}
	public enum IncludeTypeCPPModel
	{
		None = 0,
		System = 1,
		User = 2,
	}
	// generated class, do not modify
	public partial class IncludeCPPModel : CodeCPPModel
	{
		public IncludeCPPModel(IncludeTypeCPPModel Type = 0, String Name = "")
		{
			this.Type = Type;
			this.Name = Name ?? "";
		}
		public IncludeTypeCPPModel Type { get; set; } = 0;
		public String Name { get; set; } = "";
	}
	public enum AccessTypeCPPModel
	{
		None = 0,
		Private = 1,
		Protected = 2,
		Public = 3,
	}
	public enum InstanceTypeCPPModel
	{
		None = 0,
		Static = 1,
		NonStatic = 2,
	}
	public enum OverloadTypeCPPModel
	{
		None = 0,
		Abstract = 1,
		Virtual = 2,
	}
	// generated class, do not modify
	public partial class ModifiersCPPModel : CPPModel
	{
		public ModifiersCPPModel(AccessTypeCPPModel AccessType = 0, InstanceTypeCPPModel InstanceType = 0, OverloadTypeCPPModel OverloadType = 0)
		{
			this.AccessType = AccessType;
			this.InstanceType = InstanceType;
			this.OverloadType = OverloadType;
		}
		public AccessTypeCPPModel AccessType { get; set; } = 0;
		public InstanceTypeCPPModel InstanceType { get; set; } = 0;
		public OverloadTypeCPPModel OverloadType { get; set; } = 0;
	}
	// generated class, do not modify
	public abstract partial class MemberCPPModel : CPPModel
	{
		public MemberCPPModel(String Name = "")
		{
			this.Name = Name ?? "";
		}
		public String Name { get; set; } = "";
	}
	// generated class, do not modify
	public partial class FieldCPPModel : MemberCPPModel
	{
		public FieldCPPModel(ModifiersCPPModel Modifiers = null, Type FieldType = null)
		{
			this.Modifiers = Modifiers ?? null;
			this.FieldType = FieldType ?? null;
		}
		public ModifiersCPPModel Modifiers { get; set; } = null;
		public Type FieldType { get; set; } = null;
	}
	// generated class, do not modify
	public partial class DataCPPModel : MemberCPPModel
	{
		public DataCPPModel(Type DataType = null)
		{
			this.DataType = DataType ?? null;
		}
		public Type DataType { get; set; } = null;
	}
	// generated class, do not modify
	public partial class ArgumentCPPModel : MemberCPPModel
	{
		public ArgumentCPPModel()
		{
		}
	}
	// generated class, do not modify
	public partial class MethodCPPModel : MemberCPPModel, IMembersContainerCPPModel
	{
		public MethodCPPModel(Type ReturnType = null, ModifiersCPPModel Modifiers = null, List<ArgumentCPPModel> Arguments = null, List<CPPModel> Members = null)
		{
			this.ReturnType = ReturnType ?? null;
			this.Modifiers = Modifiers ?? null;
			this.Arguments = Arguments ?? new List<ArgumentCPPModel>();
			this.Members = Members ?? new List<CPPModel>();
		}
		public Type ReturnType { get; set; } = null;
		public ModifiersCPPModel Modifiers { get; set; } = null;
		public List<ArgumentCPPModel> Arguments { get; set; } = new List<ArgumentCPPModel>();
		public List<CPPModel> Members { get; set; } = new List<CPPModel>();
	}
	// generated class, do not modify
	public partial class ClassCPPModel : MemberCPPModel, IMembersContainerCPPModel
	{
		public ClassCPPModel(ModifiersCPPModel Modifiers = null, List<CPPModel> Members = null)
		{
			this.Modifiers = Modifiers ?? null;
			this.Members = Members ?? new List<CPPModel>();
		}
		public ModifiersCPPModel Modifiers { get; set; } = null;
		public List<CPPModel> Members { get; set; } = new List<CPPModel>();
	}
	// generated class, do not modify
	public partial class NamespaceCPPModel : CPPModel, IMembersContainerCPPModel
	{
		public NamespaceCPPModel(String Namespace = "", List<CPPModel> Members = null)
		{
			this.Namespace = Namespace ?? "";
			this.Members = Members ?? new List<CPPModel>();
		}
		public String Namespace { get; set; } = "";
		public List<CPPModel> Members { get; set; } = new List<CPPModel>();
	}
	// generated class, do not modify
	public partial class FileCPPModel : CPPModel, IMembersContainerCPPModel
	{
		public FileCPPModel(List<CPPModel> Members = null)
		{
			this.Members = Members ?? new List<CPPModel>();
		}
		public List<CPPModel> Members { get; set; } = new List<CPPModel>();
	}
	public abstract partial class CPPModelVisitor : CPPModelDefaultVisitor
	{
		public virtual void VisitIncludeCPPModel(IncludeCPPModel model) => DefaultVisit(model);
		public virtual void VisitModifiersCPPModel(ModifiersCPPModel model) => DefaultVisit(model);
		public virtual void VisitFieldCPPModel(FieldCPPModel model) => DefaultVisit(model);
		public virtual void VisitDataCPPModel(DataCPPModel model) => DefaultVisit(model);
		public virtual void VisitArgumentCPPModel(ArgumentCPPModel model) => DefaultVisit(model);
		public virtual void VisitMethodCPPModel(MethodCPPModel model) => DefaultVisit(model);
		public virtual void VisitClassCPPModel(ClassCPPModel model) => DefaultVisit(model);
		public virtual void VisitNamespaceCPPModel(NamespaceCPPModel model) => DefaultVisit(model);
		public virtual void VisitFileCPPModel(FileCPPModel model) => DefaultVisit(model);
		public virtual void Visit(CPPModel model)
		{
			switch(model)
			{
				case IncludeCPPModel m: VisitIncludeCPPModel(m); break;
				case ModifiersCPPModel m: VisitModifiersCPPModel(m); break;
				case FieldCPPModel m: VisitFieldCPPModel(m); break;
				case DataCPPModel m: VisitDataCPPModel(m); break;
				case ArgumentCPPModel m: VisitArgumentCPPModel(m); break;
				case MethodCPPModel m: VisitMethodCPPModel(m); break;
				case ClassCPPModel m: VisitClassCPPModel(m); break;
				case NamespaceCPPModel m: VisitNamespaceCPPModel(m); break;
				case FileCPPModel m: VisitFileCPPModel(m); break;
			}
		}
	}
}
