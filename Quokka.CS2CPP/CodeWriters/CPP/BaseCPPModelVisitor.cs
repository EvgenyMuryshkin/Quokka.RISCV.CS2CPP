using Quokka.CS2CPP.CodeModels.CPP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quokka.CS2CPP.CodeWriters.CPP
{
    public class BaseCPPModelVisitor : CPPModelVisitor
    {
        protected CPPWriter _writer = new CPPWriter();
        public void OpenBlock() => _writer.OpenBlock();
        public void CloseBlock() => _writer.CloseBlock();
        public void AppendLine(string value) => _writer.AppendLine(value);
        public override string ToString() => _writer.ToString();

        public override void DefaultVisit(CPPModel model)
        {
            throw new Exception($"{GetType().Name}: Unsupported node type: {model.GetType().Name}: {model.ToString()}");
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

        protected T Invoke<T>(CPPModel model) where T : BaseCPPModelVisitor, new()
        {
            var result = Resolve<T>();

            if (model != null)
                result.Visit(model);

            return result;
        }

        protected List<T> Invoke<T>(IEnumerable<CPPModel> models) where T : BaseCPPModelVisitor, new()
        {
            var result = new List<T>();

            foreach (var model in models)
            {
                result.Add(Invoke<T>(model));
            }

            return result;
        }
    }
}
