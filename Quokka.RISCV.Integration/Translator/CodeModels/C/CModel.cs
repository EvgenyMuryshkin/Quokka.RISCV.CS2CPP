using System;
using System.Collections.Generic;
namespace Quokka.RISCV.Integration.Translator.CodeModels.C
{
	public abstract partial class CModel
	{
	}
	public abstract partial class CCodeModel : CModel
	{
	}
	public enum CIncludeModelType
	{
		System = 0,
		User = 1,
	}
	public partial class CIncludeModel : CCodeModel
	{
		public CIncludeModelType Type { get; set; }
		public String Name { get; set; } = "";
	}
	public partial class CFileModel : CModel
	{
		public List<CCodeModel> Children = new List<CCodeModel>();
	}
	public abstract partial class CModelVisitor : CModelDefaultVisitor
	{
		public virtual void VisitCIncludeModel(CIncludeModel model) => DefaultVisit(model);
		public virtual void VisitCFileModel(CFileModel model) => DefaultVisit(model);
		public virtual void Visit(CModel model)
		{
			switch(model)
			{
				case CIncludeModel m: VisitCIncludeModel(m); break;
				case CFileModel m: VisitCFileModel(m); break;
			}
		}
	}
}
