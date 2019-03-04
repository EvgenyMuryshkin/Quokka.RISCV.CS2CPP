using Quokka.CS2C.CodeModels.C;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quokka.CS2C.CodeWriters.C
{
    public class BaseCModelVisitor : CModelVisitor
    {
        protected CWriter _writer = new CWriter();
        public void OpenBlock() => _writer.OpenBlock();
        public void CloseBlock() => _writer.CloseBlock();
        public void AppendLine(string value) => _writer.AppendLine(value);
        public override string ToString() => _writer.ToString();

        public override void DefaultVisit(CModel model)
        {
            throw new Exception($"Unsupported node type: {model.GetType().Name}: {model.ToString()}");
        }

        protected void VisitChildren(IEnumerable<CModel> children)
        {
            foreach (var child in children)
            {
                Visit(child);
            }
        }

        protected T Resolve<T>() where T : BaseCModelVisitor, new()
        {
            return new T() { _writer = _writer };
        }

        protected void Invoke<T>(CModel model) where T : BaseCModelVisitor, new()
        {
            Resolve<T>().Visit(model);
        }

        protected void Invoke<T>(IEnumerable<CModel> models) where T : BaseCModelVisitor, new()
        {
            foreach (var model in models)
            {
                Invoke<T>(model);
            }
        }
    }
}
