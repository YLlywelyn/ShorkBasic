using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ShorkBasic
{
    internal static class Interpreter
    {
        internal static ShorkObject VisitNode(NodeBase node, Context context)
        {
            switch (node)
            {
                default:
                    throw new NotImplementedError(string.Format("No visit method for type '{0}'.", node.GetType().Name));
                case NumberNode:
                    return VisitNumberNode((NumberNode)node, context);
                case BinaryOperationNode:
                    return VisitBinaryOperationNode((BinaryOperationNode)node, context);
                case UnaryOperationNode:
                    return VisitUnaryOperationNode((UnaryOperationNode)node, context);
            }
        }

        static ShorkNumber VisitNumberNode(NumberNode node, Context context)
        {
            return (ShorkNumber)(new ShorkNumber(node.numToken.value).SetContext(context).SetPosition(node.startPosition, node.endPosition));
        }

        static ShorkObject VisitBinaryOperationNode(BinaryOperationNode node, Context context)
        {
            ShorkObject leftValue = VisitNode(node.leftNode, context);
            ShorkObject rightValue = VisitNode(node.rightNode, context);
            ShorkObject result = null;

            switch (node.opToken.type)
            {
                case TokenType.PLUS:
                    result = leftValue.Add(rightValue);
                    break;
                case TokenType.MINUS:
                    result = leftValue.Subtract(rightValue);
                    break;
                case TokenType.MULTIPLY:
                    result = leftValue.MultiplyBy(rightValue);
                    break;
                case TokenType.DIVIDE:
                    result = leftValue.DivideBy(rightValue);
                    break;
                case TokenType.EXPONENT:
                    result = leftValue.PowerOf(rightValue);
                    break;
                default:
                    throw new RuntimeError(node.startPosition, node.endPosition,
                                            "Binary operation somehow has an invalid operator.", context);
            }

            return result.SetPosition(node.startPosition, node.endPosition);
        }

        static ShorkObject VisitUnaryOperationNode(UnaryOperationNode node, Context context)
        {
            ShorkObject number = VisitNode(node.node, context);

            if (number.GetType() != typeof(ShorkNumber))
                throw new RuntimeError(node.startPosition, node.endPosition,
                                        "Unary operation should not be performed on non-number.", context);

            if (node.opToken.type == TokenType.MINUS)
                number = number.MultiplyBy(new ShorkNumber(-1));
            return number;
        }
    }
}
