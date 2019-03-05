using Quokka.CS2CPP.CodeModels.CPP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quokka.CS2CPP.CodeWriters.C
{
    public class BaseCPPModelVisitor : CPPModelVisitor
    {
        protected CWriter _writer = new CWriter();
        public void OpenBlock() => _writer.OpenBlock();
        public void CloseBlock() => _writer.CloseBlock();
        public void AppendLine(string value) => _writer.AppendLine(value);
        public override string ToString() => _writer.ToString();

        public override void DefaultVisit(CPPModel model)
        {
            throw new Exception($"Unsupported node type: {model.GetType().Name}: {model.ToString()}");
        }

        protected void VisitChildren(IEnumerable<CPPModel> children)
        {
            foreach (var child in children)
            {
                Visit(child);
            }
        }

        protected T Resolve<T>() where T : BaseCPPModelVisitor, new()
        {
            return new T() { _writer = _writer };
        }

        protected void Invoke<T>(CPPModel model) where T : BaseCPPModelVisitor, new()
        {
            Resolve<T>().Visit(model);
        }

        protected void Invoke<T>(IEnumerable<CPPModel> models) where T : BaseCPPModelVisitor, new()
        {
            foreach (var model in models)
            {
                Invoke<T>(model);
            }
        }
    }
}
