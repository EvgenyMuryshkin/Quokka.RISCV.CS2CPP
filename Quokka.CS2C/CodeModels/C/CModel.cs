// generated code, do not modify
using System;
using System.Collections.Generic;
namespace Quokka.CS2C.CodeModels.C
{
	// generated class, do not modify
	public abstract partial class CModel
	{
		public CModel()
		{
		}
	}
	// generated class, do not modify
	public abstract partial class CodeCModel : CModel
	{
		public CodeCModel()
		{
		}
	}
	public partial interface IMembersContainerCModel
	{
		List<CModel> Members { get; set; }
	}
	public enum IncludeTypeCModel
	{
		None = 0,
		System = 1,
		User = 2,
	}
	// generated class, do not modify
	public partial class IncludeCModel : CodeCModel
	{
		public IncludeCModel(IncludeTypeCModel Type = 0, String Name = "")
		{
			this.Type = Type;
			this.Name = Name ?? "";
		}
		public IncludeTypeCModel Type { get; set; } = 0;
		public String Name { get; set; } = "";
	}
	public enum AccessTypeCModel
	{
		None = 0,
		Private = 1,
		Protected = 2,
		Public = 3,
	}
	public enum InstanceTypeCModel
	{
		None = 0,
		Static = 1,
		NonStatic = 2,
	}
	public enum OverloadTypeCModel
	{
		None = 0,
		Abstract = 1,
		Virtual = 2,
	}
	// generated class, do not modify
	public partial class ModifiersCModel : CModel
	{
		public ModifiersCModel(AccessTypeCModel AccessType = 0, InstanceTypeCModel InstanceType = 0, OverloadTypeCModel OverloadType = 0)
		{
			this.AccessType = AccessType;
			this.InstanceType = InstanceType;
			this.OverloadType = OverloadType;
		}
		public AccessTypeCModel AccessType { get; set; } = 0;
		public InstanceTypeCModel InstanceType { get; set; } = 0;
		public OverloadTypeCModel OverloadType { get; set; } = 0;
	}
	// generated class, do not modify
	public abstract partial class MemberCModel : CModel
	{
		public MemberCModel(String Name = "")
		{
			this.Name = Name ?? "";
		}
		public String Name { get; set; } = "";
	}
	// generated class, do not modify
	public partial class FieldCModel : MemberCModel
	{
		public FieldCModel(ModifiersCModel Modifiers = null, Type FieldType = null)
		{
			this.Modifiers = Modifiers ?? null;
			this.FieldType = FieldType ?? null;
		}
		public ModifiersCModel Modifiers { get; set; } = null;
		public Type FieldType { get; set; } = null;
	}
	// generated class, do not modify
	public partial class DataCModel : MemberCModel
	{
		public DataCModel(Type DataType = null)
		{
			this.DataType = DataType ?? null;
		}
		public Type DataType { get; set; } = null;
	}
	// generated class, do not modify
	public partial class ArgumentCModel : MemberCModel
	{
		public ArgumentCModel()
		{
		}
	}
	// generated class, do not modify
	public partial class MethodCModel : MemberCModel, IMembersContainerCModel
	{
		public MethodCModel(Type ReturnType = null, ModifiersCModel Modifiers = null, List<ArgumentCModel> Arguments = null, List<CModel> Members = null)
		{
			this.ReturnType = ReturnType ?? null;
			this.Modifiers = Modifiers ?? null;
			this.Arguments = Arguments ?? new List<ArgumentCModel>();
			this.Members = Members ?? new List<CModel>();
		}
		public Type ReturnType { get; set; } = null;
		public ModifiersCModel Modifiers { get; set; } = null;
		public List<ArgumentCModel> Arguments { get; set; } = new List<ArgumentCModel>();
		public List<CModel> Members { get; set; } = new List<CModel>();
	}
	// generated class, do not modify
	public partial class ClassCModel : MemberCModel, IMembersContainerCModel
	{
		public ClassCModel(ModifiersCModel Modifiers = null, List<CModel> Members = null)
		{
			this.Modifiers = Modifiers ?? null;
			this.Members = Members ?? new List<CModel>();
		}
		public ModifiersCModel Modifiers { get; set; } = null;
		public List<CModel> Members { get; set; } = new List<CModel>();
	}
	// generated class, do not modify
	public partial class NamespaceCModel : CModel, IMembersContainerCModel
	{
		public NamespaceCModel(String Namespace = "", List<CModel> Members = null)
		{
			this.Namespace = Namespace ?? "";
			this.Members = Members ?? new List<CModel>();
		}
		public String Namespace { get; set; } = "";
		public List<CModel> Members { get; set; } = new List<CModel>();
	}
	// generated class, do not modify
	public partial class FileCModel : CModel, IMembersContainerCModel
	{
		public FileCModel(List<CModel> Members = null)
		{
			this.Members = Members ?? new List<CModel>();
		}
		public List<CModel> Members { get; set; } = new List<CModel>();
	}
	public abstract partial class CModelVisitor : CModelDefaultVisitor
	{
		public virtual void VisitIncludeCModel(IncludeCModel model) => DefaultVisit(model);
		public virtual void VisitModifiersCModel(ModifiersCModel model) => DefaultVisit(model);
		public virtual void VisitFieldCModel(FieldCModel model) => DefaultVisit(model);
		public virtual void VisitDataCModel(DataCModel model) => DefaultVisit(model);
		public virtual void VisitArgumentCModel(ArgumentCModel model) => DefaultVisit(model);
		public virtual void VisitMethodCModel(MethodCModel model) => DefaultVisit(model);
		public virtual void VisitClassCModel(ClassCModel model) => DefaultVisit(model);
		public virtual void VisitNamespaceCModel(NamespaceCModel model) => DefaultVisit(model);
		public virtual void VisitFileCModel(FileCModel model) => DefaultVisit(model);
		public virtual void Visit(CModel model)
		{
			switch(model)
			{
				case IncludeCModel m: VisitIncludeCModel(m); break;
				case ModifiersCModel m: VisitModifiersCModel(m); break;
				case FieldCModel m: VisitFieldCModel(m); break;
				case DataCModel m: VisitDataCModel(m); break;
				case ArgumentCModel m: VisitArgumentCModel(m); break;
				case MethodCModel m: VisitMethodCModel(m); break;
				case ClassCModel m: VisitClassCModel(m); break;
				case NamespaceCModel m: VisitNamespaceCModel(m); break;
				case FileCModel m: VisitFileCModel(m); break;
			}
		}
	}
}
