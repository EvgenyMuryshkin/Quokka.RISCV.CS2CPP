using Microsoft.CodeAnalysis.CSharp.Syntax;
using Quokka.CS2CPP.CodeModels.CPP;
using Quokka.CS2CPP.Translator.Tools;
using System.Linq;
using System.Reflection;

namespace Quokka.CS2CPP.Translator.Visitors
{
    class MethodVisitor : CSharp2CVisitor
    {
        MethodCPPModel _method;
        MethodInfo _methodInfo;

        public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            _methodInfo = TypeResolver.ResolveMethodInfo(node);

            _method = new MethodCPPModel(Modifiers: new ModifiersCPPModel());
            _method.Name = node.Identifier.ToString();
            _method.ReturnType = _methodInfo.ReturnType;
            _method.Modifiers.AccessType = node.Modifiers.ExtractAccessType();
            _method.Modifiers.InstanceType = node.Modifiers.ExtractInstanceType();

            Visit(node.ParameterList);

            using (Context.WithCodeContainer(_method))
            {
                Invoke<BlockVisitor>(node.Body);
            }
        }

        public override void VisitParameterList(ParameterListSyntax node)
        {
            VisitChildren(node.Parameters);
        }

        public override void VisitParameter(ParameterSyntax node)
        {
            var param = _methodInfo.GetParameters().Single(p => p.Name == node.Identifier.ToString());

            _method.Parameters.Add(new ParameterCPPModel()
            {
                Pass = param.ParameterType.IsByRef ? ArgumentPassCPPModel.Ref : ArgumentPassCPPModel.Raw,
                Name = node.Identifier.ToString(),
                ParameterType = param.ParameterType.IsByRef ? param.ParameterType.GetElementType() : param.ParameterType,
            });
        }
    }
}
