using Quokka.RISCV.CS2CPP.CodeModels.CPP;
using Quokka.RISCV.CS2CPP.CodeWriters.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Quokka.RISCV.CS2CPP.CodeWriters.CPP.Implementation
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

            var assignType = new Dictionary<AssignType, string>()
            {
                { AssignType.Equals, "=" },
                { AssignType.PlusEquals, "+=" },
                { AssignType.MinusEquals, "-=" },
                { AssignType.MultEquals, "*=" },
                { AssignType.DivEquals, "/=" },
            };

            if (!assignType.ContainsKey(model.Type))
                throw new Exception($"Assign type {model.Type} is not supported");

            AppendLine($"{left} {assignType[model.Type]} {right};");
        }

        public override void VisitWhileLoopCPPModel(WhileLoopCPPModel model)
        {
            var condition = Invoke<ExpressionBuilder>(model.Condition).Expression;
            AppendLine($"while({condition})");
            OpenBlock();
            VisitChildren(model.Members);
            CloseBlock();
        }

        public override void VisitDoLoopCPPModel(DoLoopCPPModel model)
        {
            var condition = Invoke<ExpressionBuilder>(model.Condition).Expression;
            AppendLine($"do");
            OpenBlock();
            VisitChildren(model.Members);
            CloseBlock();
            AppendLine($"while({condition});");
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

        public override void VisitIfCPPModel(IfCPPModel model)
        {
            var expression = Invoke<ExpressionBuilder>(model.Condition).Expression;
            AppendLine($"if ({expression})");
            OpenBlock();
            VisitChildren(model.Members);
            CloseBlock();
        }

        public override void VisitElseCPPModel(ElseCPPModel model)
        {
            AppendLine($"else");
            OpenBlock();
            VisitChildren(model.Members);
            CloseBlock();
        }

        public override void VisitSwitchCPPModel(SwitchCPPModel model)
        {
            var expression = Invoke<ExpressionBuilder>(model.Expression).Expression;
            AppendLine($"switch ({expression})");
            OpenBlock();
            VisitChildren(model.Members);
            CloseBlock();
        }

        public override void VisitCaseCPPModel(CaseCPPModel model)
        {
            VisitChildren(model.Labels);
            OpenBlock();
            VisitChildren(model.Members);
            CloseBlock();
        }

        public override void VisitDefaultSwitchLabelCPPModel(DefaultSwitchLabelCPPModel model)
        {
            AppendLine($"default:");
        }

        public override void VisitBreakCPPModel(BreakCPPModel model)
        {
            AppendLine($"break;");
        }

        public override void VisitCaseSwitchLabelCPPModel(CaseSwitchLabelCPPModel model)
        {
            var condition = Invoke<ExpressionBuilder>(model.Condition).Expression;
            AppendLine($"case {condition}:");
        }
    }
}
