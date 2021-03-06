﻿using Quokka.RISCV.CS2CPP.CodeModels.CPP;
using Quokka.RISCV.CS2CPP.CodeWriters.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quokka.RISCV.CS2CPP.CodeWriters.CPP
{
    public class BaseCPPModelVisitor : CPPModelVisitor
    {
        protected CPPWriter _writer = new CPPWriter();

        public CodeWriterContext Context { get; set; }

        // CPP Writer wrappers
        public void OpenBlock() => _writer.OpenBlock();
        public void CloseBlock() => _writer.CloseBlock();
        public void CloseBlockWithSemicolon() => _writer.CloseBlockWithSemicolon();
        public void AppendLine(string value) => _writer.AppendLine(value);
        public override string ToString() => _writer.ToString();

        // Base boilerplate 

        protected void Unsupported(CPPModel model, string message)
        {
            throw new Exception($"[{GetType().Name}] {model.GetType().Name} ({model}): {message}");
        }

        public override void DefaultVisit(CPPModel model)
        {
            Unsupported(model, $"Unsupported node type");
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
            return new T()
            {
                _writer = _writer,
                Context = Context
            };
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
