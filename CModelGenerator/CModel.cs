using System;
using System.Collections.Generic;
using System.Text;

namespace metadata
{
    public abstract class CModel
    {
    }

    public abstract class CCodeModel : CModel
    {

    }

    public enum CIncludeModelType
    {
        System,
        User,
    }

    public class CIncludeModel : CCodeModel
    {
        public CIncludeModelType Type { get; set; }
        public string Name { get; set; }
    }

    public class CFileModel : CModel
    {
        public List<CCodeModel> Children = new List<CCodeModel>();
    }
}
