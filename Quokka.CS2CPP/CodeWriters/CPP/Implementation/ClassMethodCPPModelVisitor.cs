using Quokka.CS2CPP.CodeModels.CPP;
using Quokka.CS2CPP.CodeWriters.Tools;
using System.Collections.Generic;
using System.Linq;

namespace Quokka.CS2CPP.CodeWriters.CPP.Implementation
{
    public class ClassMethodCModelVisitor : BaseCPPModelVisitor
    {
        public override void VisitMethodCPPModel(MethodCPPModel model)
        {
            Context.Method = model;

            var pars = string.Join(", ", model.Parameters.Select(arg =>
            {
                var pass = "";
                switch(arg.Pass)
                {
                    case ArgumentPassCPPModel.Ref:
                        pass = " &";
                        break;
                    case ArgumentPassCPPModel.Pointer:
                        pass = "* ";
                        break;
                    case ArgumentPassCPPModel.Raw:
                        pass = " ";
                        break;
                }
                return $"{TypeLookup.LookupCPPTypeName(arg.ParameterType)}{pass}{arg.Name}";
            }));

            AppendLine($"{TypeLookup.LookupCPPTypeName(model.ReturnType)} {Context.Class.Name}::{model.Name}({pars})");
            OpenBlock();
            VisitChildren(model.Members);
            CloseBlock();
        }

        public override void VisitDataCPPModel(DataCPPModel model)
        {
            AppendLine($"{Invoke<ExpressionBuilder>(model).Expression};");
        }

        public override void VisitAssignmentExpressionCPPModel(AssignmentExpressionCPPModel model)
        {
            var left = Invoke<ExpressionBuilder>(model.Left).Expression;
            var right = Invoke<ExpressionBuilder>(model.Right).Expression;
            AppendLine($"{left} = {right};");
        }

        public override void VisitWhileLoopCPPModel(WhileLoopCPPModel model)
        {
            var condition = Invoke<ExpressionBuilder>(model.Condition).Expression;
            AppendLine($"while({condition})");
            OpenBlock();
            VisitChildren(model.Members);
            CloseBlock();
        }

        public override void VisitForLoopCPPModel(ForLoopCPPModel model)
        {
            var declaration = Invoke<ExpressionBuilder>(model.Declaration).Expression ?? "";
            var initializers = model.Initializers.Select(i => Invoke<ExpressionBuilder>(i).Expression);
            var parts = new List<string>();
            parts.Add(declaration);
            parts.AddRange(initializers);
            var init = string.Join(", ", parts.Where(p => !string.IsNullOrWhiteSpace(p)));

            var condition = Invoke<ExpressionBuilder>(model.Condition).Expression ?? "";
            var incrementors = string.Join(", ", model.Incrementors.Select(i => Invoke<ExpressionBuilder>(i).Expression));

            AppendLine($"for({init}; {condition}; {incrementors})");
            OpenBlock();
            VisitChildren(model.Members);
            CloseBlock();
        }

        public override void VisitPostfixUnaryExpressionCPPModel(PostfixUnaryExpressionCPPModel model)
        {
            var expression = Invoke<ExpressionBuilder>(model).Expression;
            AppendLine($"{expression};");
        }

        public override void VisitPrefixUnaryExpressionCPPModel(PrefixUnaryExpressionCPPModel model)
        {
            var expression = Invoke<ExpressionBuilder>(model).Expression;
            AppendLine($"{expression};");
        }

        public override void VisitReturnExpresionCPPModel(ReturnExpresionCPPModel model)
        {
            var expression = Invoke<ExpressionBuilder>(model.Expression).Expression;
            AppendLine($"return {expression};");
        }

        public override void VisitLocalInvocationCPPModel(LocalInvocationCPPModel model)
        {
            var expression = Invoke<ExpressionBuilder>(model).Expression;
            AppendLine($"{expression};");
        }

        public override void VisitCastCPPModel(CastCPPModel model)
        {
            var expression = Invoke<ExpressionBuilder>(model).Expression;
            AppendLine($"{expression};");
        }
    }
}
