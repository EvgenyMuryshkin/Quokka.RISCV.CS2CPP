using Microsoft.CodeAnalysis.CSharp.Syntax;
using Quokka.RISCV.CS2CPP.CodeModels.CPP;

namespace Quokka.RISCV.CS2CPP.Translator.Visitors
{
    class FieldInitializerVisitor : ExpressionVisitor
    {
        public override void VisitArrayCreationExpression(ArrayCreationExpressionSyntax node)
        {
            if (node.Type.RankSpecifiers.Count != 1)
                Unsupported(node, "Multiranked arrays not supported");

            var rank = node.Type.RankSpecifiers[0];

            if (rank.Sizes.Count != 1)
                Unsupported(node, "Multidimensional arrays not supported");

            var type = TypeResolver.ResolveType(node.Type.ElementType);

            Expression = new ArrayCreationExpressionCPPModel()
            {
                Type = type.GetElementType(),
                Rank = Invoke<ExpressionVisitor>(rank.Sizes[0]).Expression
            };
        }
    }
}
