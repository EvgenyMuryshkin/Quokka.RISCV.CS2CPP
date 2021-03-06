// generated code, do not modify
using System;
using System.Collections.Generic;
namespace Quokka.RISCV.CS2CPP.CodeModels.CPP
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
	public enum AssignType
	{
		Equals = 0,
		PlusEquals = 1,
		MinusEquals = 2,
		MultEquals = 3,
		DivEquals = 4,
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
	public abstract partial class ExpressionCPPModel : CPPModel
	{
		public ExpressionCPPModel()
		{
		}
	}
	// generated class, do not modify
	public partial class LiteralExpressionCPPModel : ExpressionCPPModel
	{
		public LiteralExpressionCPPModel(String Value = "")
		{
			this.Value = Value ?? "";
		}
		public String Value { get; set; } = "";
	}
	// generated class, do not modify
	public partial class IdentifierExpressionCPPModel : ExpressionCPPModel
	{
		public IdentifierExpressionCPPModel(String Identifier = "")
		{
			this.Identifier = Identifier ?? "";
		}
		public String Identifier { get; set; } = "";
	}
	// generated class, do not modify
	public partial class AssignmentExpressionCPPModel : ExpressionCPPModel
	{
		public AssignmentExpressionCPPModel(AssignType Type = 0, ExpressionCPPModel Left = null, ExpressionCPPModel Right = null)
		{
			this.Type = Type;
			this.Left = Left ?? default(ExpressionCPPModel);
			this.Right = Right ?? default(ExpressionCPPModel);
		}
		public AssignType Type { get; set; } = 0;
		public ExpressionCPPModel Left { get; set; } = default(ExpressionCPPModel);
		public ExpressionCPPModel Right { get; set; } = default(ExpressionCPPModel);
	}
	public enum BinaryExpressionTypeCPPModel
	{
		Equal = 0,
		NotEqual = 1,
		Less = 2,
		LessOrEqual = 3,
		Greater = 4,
		GreaterOrEqual = 5,
		Add = 6,
		Sub = 7,
		Mult = 8,
		Div = 9,
		RightShift = 10,
		LeftShift = 11,
	}
	// generated class, do not modify
	public partial class BinaryExpressionCPPModel : ExpressionCPPModel
	{
		public BinaryExpressionCPPModel(BinaryExpressionTypeCPPModel Type = 0, ExpressionCPPModel Left = null, ExpressionCPPModel Right = null)
		{
			this.Type = Type;
			this.Left = Left ?? default(ExpressionCPPModel);
			this.Right = Right ?? default(ExpressionCPPModel);
		}
		public BinaryExpressionTypeCPPModel Type { get; set; } = 0;
		public ExpressionCPPModel Left { get; set; } = default(ExpressionCPPModel);
		public ExpressionCPPModel Right { get; set; } = default(ExpressionCPPModel);
	}
	public enum UnaryExpressionTypeCPPModel
	{
		Inclement = 0,
		Decrement = 1,
		Minus = 2,
	}
	// generated class, do not modify
	public abstract partial class UnaryExpressionCPPModel : ExpressionCPPModel
	{
		public UnaryExpressionCPPModel(UnaryExpressionTypeCPPModel Type = 0, ExpressionCPPModel Operand = null)
		{
			this.Type = Type;
			this.Operand = Operand ?? default(ExpressionCPPModel);
		}
		public UnaryExpressionTypeCPPModel Type { get; set; } = 0;
		public ExpressionCPPModel Operand { get; set; } = default(ExpressionCPPModel);
	}
	// generated class, do not modify
	public partial class PrefixUnaryExpressionCPPModel : UnaryExpressionCPPModel
	{
		public PrefixUnaryExpressionCPPModel()
		{
		}
	}
	// generated class, do not modify
	public partial class PostfixUnaryExpressionCPPModel : UnaryExpressionCPPModel
	{
		public PostfixUnaryExpressionCPPModel()
		{
		}
	}
	// generated class, do not modify
	public partial class ReturnExpresionCPPModel : ExpressionCPPModel
	{
		public ReturnExpresionCPPModel(ExpressionCPPModel Expression = null)
		{
			this.Expression = Expression ?? default(ExpressionCPPModel);
		}
		public ExpressionCPPModel Expression { get; set; } = default(ExpressionCPPModel);
	}
	// generated class, do not modify
	public partial class FieldCPPModel : MemberCPPModel
	{
		public FieldCPPModel(ModifiersCPPModel Modifiers = null, Type FieldType = null, ExpressionCPPModel Initializer = null)
		{
			this.Modifiers = Modifiers ?? default(ModifiersCPPModel);
			this.FieldType = FieldType ?? default(Type);
			this.Initializer = Initializer ?? default(ExpressionCPPModel);
		}
		public ModifiersCPPModel Modifiers { get; set; } = default(ModifiersCPPModel);
		public Type FieldType { get; set; } = default(Type);
		public ExpressionCPPModel Initializer { get; set; } = default(ExpressionCPPModel);
	}
	// generated class, do not modify
	public partial class DataInitializerCPPModel : MemberCPPModel
	{
		public DataInitializerCPPModel(ExpressionCPPModel Initializer = null)
		{
			this.Initializer = Initializer ?? default(ExpressionCPPModel);
		}
		public ExpressionCPPModel Initializer { get; set; } = default(ExpressionCPPModel);
	}
	// generated class, do not modify
	public partial class DataCPPModel : CPPModel
	{
		public DataCPPModel(Type DataType = null, List<DataInitializerCPPModel> Initializers = null)
		{
			this.DataType = DataType ?? default(Type);
			this.Initializers = Initializers ?? new List<DataInitializerCPPModel>();
		}
		public Type DataType { get; set; } = default(Type);
		public List<DataInitializerCPPModel> Initializers { get; set; } = new List<DataInitializerCPPModel>();
	}
	public enum ArgumentPassCPPModel
	{
		Raw = 0,
		Ref = 1,
		Pointer = 2,
	}
	// generated class, do not modify
	public partial class ParameterCPPModel : MemberCPPModel
	{
		public ParameterCPPModel(ArgumentPassCPPModel Pass = 0, Type ParameterType = null)
		{
			this.Pass = Pass;
			this.ParameterType = ParameterType ?? default(Type);
		}
		public ArgumentPassCPPModel Pass { get; set; } = 0;
		public Type ParameterType { get; set; } = default(Type);
	}
	// generated class, do not modify
	public partial class MethodCPPModel : MemberCPPModel, IMembersContainerCPPModel
	{
		public MethodCPPModel(Type ReturnType = null, ModifiersCPPModel Modifiers = null, List<ParameterCPPModel> Parameters = null, List<CPPModel> Members = null)
		{
			this.ReturnType = ReturnType ?? default(Type);
			this.Modifiers = Modifiers ?? default(ModifiersCPPModel);
			this.Parameters = Parameters ?? new List<ParameterCPPModel>();
			this.Members = Members ?? new List<CPPModel>();
		}
		public Type ReturnType { get; set; } = default(Type);
		public ModifiersCPPModel Modifiers { get; set; } = default(ModifiersCPPModel);
		public List<ParameterCPPModel> Parameters { get; set; } = new List<ParameterCPPModel>();
		public List<CPPModel> Members { get; set; } = new List<CPPModel>();
	}
	// generated class, do not modify
	public partial class ClassCPPModel : MemberCPPModel, IMembersContainerCPPModel
	{
		public ClassCPPModel(ModifiersCPPModel Modifiers = null, List<CPPModel> Members = null)
		{
			this.Modifiers = Modifiers ?? default(ModifiersCPPModel);
			this.Members = Members ?? new List<CPPModel>();
		}
		public ModifiersCPPModel Modifiers { get; set; } = default(ModifiersCPPModel);
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
	public abstract partial class LoopCPPModel : CPPModel, IMembersContainerCPPModel
	{
		public LoopCPPModel(List<CPPModel> Members = null)
		{
			this.Members = Members ?? new List<CPPModel>();
		}
		public List<CPPModel> Members { get; set; } = new List<CPPModel>();
	}
	// generated class, do not modify
	public partial class WhileLoopCPPModel : LoopCPPModel, IMembersContainerCPPModel
	{
		public WhileLoopCPPModel(ExpressionCPPModel Condition = null)
		{
			this.Condition = Condition ?? default(ExpressionCPPModel);
		}
		public ExpressionCPPModel Condition { get; set; } = default(ExpressionCPPModel);
	}
	// generated class, do not modify
	public partial class IfCPPModel : CPPModel, IMembersContainerCPPModel
	{
		public IfCPPModel(ExpressionCPPModel Condition = null, List<CPPModel> Members = null)
		{
			this.Condition = Condition ?? default(ExpressionCPPModel);
			this.Members = Members ?? new List<CPPModel>();
		}
		public ExpressionCPPModel Condition { get; set; } = default(ExpressionCPPModel);
		public List<CPPModel> Members { get; set; } = new List<CPPModel>();
	}
	// generated class, do not modify
	public partial class ElseCPPModel : CPPModel, IMembersContainerCPPModel
	{
		public ElseCPPModel(List<CPPModel> Members = null)
		{
			this.Members = Members ?? new List<CPPModel>();
		}
		public List<CPPModel> Members { get; set; } = new List<CPPModel>();
	}
	// generated class, do not modify
	public partial class SwitchCPPModel : CPPModel, IMembersContainerCPPModel
	{
		public SwitchCPPModel(ExpressionCPPModel Expression = null, List<CPPModel> Members = null)
		{
			this.Expression = Expression ?? default(ExpressionCPPModel);
			this.Members = Members ?? new List<CPPModel>();
		}
		public ExpressionCPPModel Expression { get; set; } = default(ExpressionCPPModel);
		public List<CPPModel> Members { get; set; } = new List<CPPModel>();
	}
	// generated class, do not modify
	public partial class CaseCPPModel : CPPModel, IMembersContainerCPPModel
	{
		public CaseCPPModel(List<CaseLabelCPPModel> Labels = null, List<CPPModel> Members = null)
		{
			this.Labels = Labels ?? new List<CaseLabelCPPModel>();
			this.Members = Members ?? new List<CPPModel>();
		}
		public List<CaseLabelCPPModel> Labels { get; set; } = new List<CaseLabelCPPModel>();
		public List<CPPModel> Members { get; set; } = new List<CPPModel>();
	}
	// generated class, do not modify
	public abstract partial class CaseLabelCPPModel : CPPModel
	{
		public CaseLabelCPPModel()
		{
		}
	}
	// generated class, do not modify
	public partial class CaseSwitchLabelCPPModel : CaseLabelCPPModel
	{
		public CaseSwitchLabelCPPModel(ExpressionCPPModel Condition = null)
		{
			this.Condition = Condition ?? default(ExpressionCPPModel);
		}
		public ExpressionCPPModel Condition { get; set; } = default(ExpressionCPPModel);
	}
	// generated class, do not modify
	public partial class DefaultSwitchLabelCPPModel : CaseLabelCPPModel
	{
		public DefaultSwitchLabelCPPModel()
		{
		}
	}
	// generated class, do not modify
	public partial class BreakCPPModel : CPPModel
	{
		public BreakCPPModel()
		{
		}
	}
	// generated class, do not modify
	public partial class DoLoopCPPModel : LoopCPPModel, IMembersContainerCPPModel
	{
		public DoLoopCPPModel(ExpressionCPPModel Condition = null)
		{
			this.Condition = Condition ?? default(ExpressionCPPModel);
		}
		public ExpressionCPPModel Condition { get; set; } = default(ExpressionCPPModel);
	}
	// generated class, do not modify
	public partial class ForLoopCPPModel : LoopCPPModel, IMembersContainerCPPModel
	{
		public ForLoopCPPModel(DataCPPModel Declaration = null, List<DataInitializerCPPModel> Initializers = null, ExpressionCPPModel Condition = null, List<ExpressionCPPModel> Incrementors = null)
		{
			this.Declaration = Declaration ?? default(DataCPPModel);
			this.Initializers = Initializers ?? new List<DataInitializerCPPModel>();
			this.Condition = Condition ?? default(ExpressionCPPModel);
			this.Incrementors = Incrementors ?? new List<ExpressionCPPModel>();
		}
		public DataCPPModel Declaration { get; set; } = default(DataCPPModel);
		public List<DataInitializerCPPModel> Initializers { get; set; } = new List<DataInitializerCPPModel>();
		public ExpressionCPPModel Condition { get; set; } = default(ExpressionCPPModel);
		public List<ExpressionCPPModel> Incrementors { get; set; } = new List<ExpressionCPPModel>();
	}
	// generated class, do not modify
	public partial class ArgumentCPPModel : CPPModel
	{
		public ArgumentCPPModel(ExpressionCPPModel Expression = null)
		{
			this.Expression = Expression ?? default(ExpressionCPPModel);
		}
		public ExpressionCPPModel Expression { get; set; } = default(ExpressionCPPModel);
	}
	// generated class, do not modify
	public abstract partial class InvocationCPPModel : ExpressionCPPModel
	{
		public InvocationCPPModel(List<ArgumentCPPModel> Arguments = null)
		{
			this.Arguments = Arguments ?? new List<ArgumentCPPModel>();
		}
		public List<ArgumentCPPModel> Arguments { get; set; } = new List<ArgumentCPPModel>();
	}
	// generated class, do not modify
	public partial class LocalInvocationCPPModel : InvocationCPPModel
	{
		public LocalInvocationCPPModel(String Method = "")
		{
			this.Method = Method ?? "";
		}
		public String Method { get; set; } = "";
	}
	// generated class, do not modify
	public partial class ArrayCreationExpressionCPPModel : ExpressionCPPModel
	{
		public ArrayCreationExpressionCPPModel(Type Type = null, ExpressionCPPModel Rank = null)
		{
			this.Type = Type ?? default(Type);
			this.Rank = Rank ?? default(ExpressionCPPModel);
		}
		public Type Type { get; set; } = default(Type);
		public ExpressionCPPModel Rank { get; set; } = default(ExpressionCPPModel);
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
	// generated class, do not modify
	public partial class ElementAccessCPPModel : ExpressionCPPModel
	{
		public ElementAccessCPPModel(ExpressionCPPModel Expression = null, List<ExpressionCPPModel> Arguments = null)
		{
			this.Expression = Expression ?? default(ExpressionCPPModel);
			this.Arguments = Arguments ?? new List<ExpressionCPPModel>();
		}
		public ExpressionCPPModel Expression { get; set; } = default(ExpressionCPPModel);
		public List<ExpressionCPPModel> Arguments { get; set; } = new List<ExpressionCPPModel>();
	}
	// generated class, do not modify
	public partial class CastCPPModel : ExpressionCPPModel
	{
		public CastCPPModel(Type Type = null, ExpressionCPPModel Expression = null)
		{
			this.Type = Type ?? default(Type);
			this.Expression = Expression ?? default(ExpressionCPPModel);
		}
		public Type Type { get; set; } = default(Type);
		public ExpressionCPPModel Expression { get; set; } = default(ExpressionCPPModel);
	}
	// generated class, do not modify
	public partial class ParenthesizedExpressionCPPModel : ExpressionCPPModel
	{
		public ParenthesizedExpressionCPPModel(ExpressionCPPModel Expression = null)
		{
			this.Expression = Expression ?? default(ExpressionCPPModel);
		}
		public ExpressionCPPModel Expression { get; set; } = default(ExpressionCPPModel);
	}
	// generated class, do not modify
	public partial class SOCResourceCPPModel : CPPModel
	{
		public SOCResourceCPPModel(UInt32 Address = 0, String Name = "", Type Type = null, Int32 Length = 0)
		{
			this.Address = Address;
			this.Name = Name ?? "";
			this.Type = Type ?? default(Type);
			this.Length = Length;
		}
		public UInt32 Address { get; set; } = default(UInt32);
		public String Name { get; set; } = "";
		public Type Type { get; set; } = default(Type);
		public Int32 Length { get; set; } = default(Int32);
	}
	public abstract partial class CPPModelVisitor : CPPModelDefaultVisitor
	{
		public virtual void VisitIncludeCPPModel(IncludeCPPModel model) => DefaultVisit(model);
		public virtual void VisitModifiersCPPModel(ModifiersCPPModel model) => DefaultVisit(model);
		public virtual void VisitLiteralExpressionCPPModel(LiteralExpressionCPPModel model) => DefaultVisit(model);
		public virtual void VisitIdentifierExpressionCPPModel(IdentifierExpressionCPPModel model) => DefaultVisit(model);
		public virtual void VisitAssignmentExpressionCPPModel(AssignmentExpressionCPPModel model) => DefaultVisit(model);
		public virtual void VisitBinaryExpressionCPPModel(BinaryExpressionCPPModel model) => DefaultVisit(model);
		public virtual void VisitPrefixUnaryExpressionCPPModel(PrefixUnaryExpressionCPPModel model) => DefaultVisit(model);
		public virtual void VisitPostfixUnaryExpressionCPPModel(PostfixUnaryExpressionCPPModel model) => DefaultVisit(model);
		public virtual void VisitReturnExpresionCPPModel(ReturnExpresionCPPModel model) => DefaultVisit(model);
		public virtual void VisitFieldCPPModel(FieldCPPModel model) => DefaultVisit(model);
		public virtual void VisitDataInitializerCPPModel(DataInitializerCPPModel model) => DefaultVisit(model);
		public virtual void VisitDataCPPModel(DataCPPModel model) => DefaultVisit(model);
		public virtual void VisitParameterCPPModel(ParameterCPPModel model) => DefaultVisit(model);
		public virtual void VisitMethodCPPModel(MethodCPPModel model) => DefaultVisit(model);
		public virtual void VisitClassCPPModel(ClassCPPModel model) => DefaultVisit(model);
		public virtual void VisitNamespaceCPPModel(NamespaceCPPModel model) => DefaultVisit(model);
		public virtual void VisitWhileLoopCPPModel(WhileLoopCPPModel model) => DefaultVisit(model);
		public virtual void VisitIfCPPModel(IfCPPModel model) => DefaultVisit(model);
		public virtual void VisitElseCPPModel(ElseCPPModel model) => DefaultVisit(model);
		public virtual void VisitSwitchCPPModel(SwitchCPPModel model) => DefaultVisit(model);
		public virtual void VisitCaseCPPModel(CaseCPPModel model) => DefaultVisit(model);
		public virtual void VisitCaseSwitchLabelCPPModel(CaseSwitchLabelCPPModel model) => DefaultVisit(model);
		public virtual void VisitDefaultSwitchLabelCPPModel(DefaultSwitchLabelCPPModel model) => DefaultVisit(model);
		public virtual void VisitBreakCPPModel(BreakCPPModel model) => DefaultVisit(model);
		public virtual void VisitDoLoopCPPModel(DoLoopCPPModel model) => DefaultVisit(model);
		public virtual void VisitForLoopCPPModel(ForLoopCPPModel model) => DefaultVisit(model);
		public virtual void VisitArgumentCPPModel(ArgumentCPPModel model) => DefaultVisit(model);
		public virtual void VisitLocalInvocationCPPModel(LocalInvocationCPPModel model) => DefaultVisit(model);
		public virtual void VisitArrayCreationExpressionCPPModel(ArrayCreationExpressionCPPModel model) => DefaultVisit(model);
		public virtual void VisitFileCPPModel(FileCPPModel model) => DefaultVisit(model);
		public virtual void VisitElementAccessCPPModel(ElementAccessCPPModel model) => DefaultVisit(model);
		public virtual void VisitCastCPPModel(CastCPPModel model) => DefaultVisit(model);
		public virtual void VisitParenthesizedExpressionCPPModel(ParenthesizedExpressionCPPModel model) => DefaultVisit(model);
		public virtual void VisitSOCResourceCPPModel(SOCResourceCPPModel model) => DefaultVisit(model);
		public virtual void Visit(CPPModel model)
		{
			switch(model)
			{
				case IncludeCPPModel m: VisitIncludeCPPModel(m); break;
				case ModifiersCPPModel m: VisitModifiersCPPModel(m); break;
				case LiteralExpressionCPPModel m: VisitLiteralExpressionCPPModel(m); break;
				case IdentifierExpressionCPPModel m: VisitIdentifierExpressionCPPModel(m); break;
				case AssignmentExpressionCPPModel m: VisitAssignmentExpressionCPPModel(m); break;
				case BinaryExpressionCPPModel m: VisitBinaryExpressionCPPModel(m); break;
				case PrefixUnaryExpressionCPPModel m: VisitPrefixUnaryExpressionCPPModel(m); break;
				case PostfixUnaryExpressionCPPModel m: VisitPostfixUnaryExpressionCPPModel(m); break;
				case ReturnExpresionCPPModel m: VisitReturnExpresionCPPModel(m); break;
				case FieldCPPModel m: VisitFieldCPPModel(m); break;
				case DataInitializerCPPModel m: VisitDataInitializerCPPModel(m); break;
				case DataCPPModel m: VisitDataCPPModel(m); break;
				case ParameterCPPModel m: VisitParameterCPPModel(m); break;
				case MethodCPPModel m: VisitMethodCPPModel(m); break;
				case ClassCPPModel m: VisitClassCPPModel(m); break;
				case NamespaceCPPModel m: VisitNamespaceCPPModel(m); break;
				case WhileLoopCPPModel m: VisitWhileLoopCPPModel(m); break;
				case IfCPPModel m: VisitIfCPPModel(m); break;
				case ElseCPPModel m: VisitElseCPPModel(m); break;
				case SwitchCPPModel m: VisitSwitchCPPModel(m); break;
				case CaseCPPModel m: VisitCaseCPPModel(m); break;
				case CaseSwitchLabelCPPModel m: VisitCaseSwitchLabelCPPModel(m); break;
				case DefaultSwitchLabelCPPModel m: VisitDefaultSwitchLabelCPPModel(m); break;
				case BreakCPPModel m: VisitBreakCPPModel(m); break;
				case DoLoopCPPModel m: VisitDoLoopCPPModel(m); break;
				case ForLoopCPPModel m: VisitForLoopCPPModel(m); break;
				case ArgumentCPPModel m: VisitArgumentCPPModel(m); break;
				case LocalInvocationCPPModel m: VisitLocalInvocationCPPModel(m); break;
				case ArrayCreationExpressionCPPModel m: VisitArrayCreationExpressionCPPModel(m); break;
				case FileCPPModel m: VisitFileCPPModel(m); break;
				case ElementAccessCPPModel m: VisitElementAccessCPPModel(m); break;
				case CastCPPModel m: VisitCastCPPModel(m); break;
				case ParenthesizedExpressionCPPModel m: VisitParenthesizedExpressionCPPModel(m); break;
				case SOCResourceCPPModel m: VisitSOCResourceCPPModel(m); break;
			}
		}
	}
}
