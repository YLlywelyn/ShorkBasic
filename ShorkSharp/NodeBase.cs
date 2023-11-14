namespace ShorkSharp
{
    public abstract class NodeBase
    {
        public Position startPosition { get; protected set; }
        public Position endPosition { get; protected set; }

        protected NodeBase(Position startPosition, Position endPosition)
        {
            this.startPosition = startPosition.Copy();
            this.endPosition = endPosition.Copy();
        }
    }

    public class CodeBlockNode : NodeBase
    {
        public List<NodeBase> statements;

        public CodeBlockNode(IEnumerable<NodeBase> statements, Position startPosition, Position endPosition)
            : base(startPosition, endPosition)
        {
            this.statements = statements.ToList();
        }

        public override string ToString()
        {
            return string.Format("{{{0}}}", string.Join(", ", statements));
        }
    }

    public class NumberNode : NodeBase
    {
        public Token numToken { get; protected set; }

        public NumberNode(Token numToken)
            : base(numToken.startPosition, numToken.endPosition)
        {
            this.numToken = numToken;
        }

        public override string ToString()
        {
            return string.Format("({0})", numToken);
        }
    }

    public class StringNode : NodeBase
    {
        public Token strToken { get; protected set; }

        public StringNode(Token strToken)
            : base(strToken.startPosition, strToken.endPosition)
        {
            this.strToken = strToken;
        }

        public override string ToString()
        {
            return string.Format("({0})", strToken);
        }
    }

    public class ListNode : NodeBase
    {
        public List<NodeBase> elementNodes;

        public ListNode(IEnumerable<NodeBase> elementNodes, Position startPosition, Position endPosition)
            : base(startPosition, endPosition)
        {
            this.elementNodes = elementNodes.ToList();
        }

        public override string ToString()
        {
            return string.Format("[{0}]", string.Join(", ", elementNodes));
        }
    }

    public class VarAssignNode : NodeBase
    {
        public Token varNameToken { get; protected set; }
        public NodeBase valueNode { get; protected set; }

        public VarAssignNode(Token varNameToken, NodeBase valueNode)
            : base(varNameToken.startPosition, valueNode.endPosition)
        {
            this.varNameToken = varNameToken;
            this.valueNode = valueNode;
        }

        public override string ToString()
        {
            return string.Format("({0} = {1})", varNameToken, valueNode);
        }
    }

    public class VarAccessNode : NodeBase
    {
        public Token varNameToken { get; protected set; }

        public VarAccessNode(Token varNameToken)
            : base(varNameToken.startPosition, varNameToken.endPosition)
        {
            this.varNameToken = varNameToken;
        }

        public override string ToString()
        {
            return string.Format("({0})", varNameToken);
        }
    }

    public class BinaryOperationNode : NodeBase
    {
        public NodeBase leftNode { get; protected set; }
        public Token operatorToken { get; protected set; }
        public NodeBase rightNode { get; protected set; }

        public BinaryOperationNode(NodeBase leftNode, Token operatorToken, NodeBase rightNode)
            : base(leftNode.startPosition, rightNode.endPosition)
        {
            this.leftNode = leftNode;
            this.operatorToken = operatorToken;
            this.rightNode = rightNode;
        }

        public override string ToString()
        {
            return string.Format("({0} {1} {2})", leftNode, operatorToken, rightNode);
        }
    }

    public class UnaryOperationNode : NodeBase
    {
        public Token operatorToken { get; protected set; }
        public NodeBase operandNode { get; protected set; }

        public UnaryOperationNode(Token operatorToken, NodeBase operandNode)
            : base(operatorToken.startPosition, operandNode.endPosition)
        {
            this.operatorToken = operatorToken;
            this.operandNode = operandNode;
        }
    }

    public class IfNode : NodeBase
    {
        public IfCase[] cases { get; protected set; }
        public IfCase? elseCase { get; protected set; }

        public IfNode(IfCase[] cases)
            : base(cases[0].condition.startPosition, cases[^1].body.endPosition)
        {
            if (cases.Length > 1 && cases[^1].condition == null)
            {
                this.cases = cases.SkipLast(1).ToArray();
                this.elseCase = cases[^1];
            }
            else
            {
                this.cases = cases;
                this.elseCase = null;
            }
        }
    }

    public class ForNode : NodeBase
    {
        public Token varNameToken { get; protected set; }
        public NodeBase startValueNode { get; protected set; }
        public NodeBase endValueNode { get; protected set; }
        public NodeBase stepValueNode { get; protected set; }
        public NodeBase bodyNode { get; protected set; }
        public bool shouldReturnNull { get; protected set; }

        public ForNode(Token varNameToken,
                       NodeBase startValueNode,
                       NodeBase endValueNode,
                       NodeBase stepValueNode,
                       NodeBase bodyNode,
                       bool shouldReturnNull)
            : base(varNameToken.startPosition, bodyNode.endPosition)
        {
            this.varNameToken = varNameToken;
            this.startValueNode = startValueNode;
            this.endValueNode = endValueNode;
            this.stepValueNode = stepValueNode;
            this.bodyNode = bodyNode;
            this.shouldReturnNull = shouldReturnNull;
        }
    }

    public class WhileNode : NodeBase
    {
        public NodeBase conditionNode { get; protected set; }
        public NodeBase bodyNode { get; protected set; }
        public bool shouldReturnNull { get; protected set; }

        public WhileNode(NodeBase conditionNode, NodeBase bodyNode, bool shouldReturnNull)
            : base(conditionNode.startPosition, bodyNode.endPosition)
        {
            this.conditionNode = conditionNode;
            this.bodyNode = bodyNode;
            this.shouldReturnNull = shouldReturnNull;
        }
    }

    public class FunctionDefinitionNode : NodeBase
    {
        public Token varNameToken { get; protected set; }
        public Token[] argNameTokens { get; protected set; }
        public NodeBase bodyNode { get; protected set; }
        public bool shouldAutoReturn { get; protected set; }

        public FunctionDefinitionNode(Token varNameToken,
                                      Token[] argNameTokens,
                                      NodeBase bodyNode,
                                      bool shouldAutoReturn)
            : base(varNameToken.startPosition, bodyNode.endPosition)
        {
            this.varNameToken = varNameToken;
            this.argNameTokens = argNameTokens;
            this.bodyNode = bodyNode;
            this.shouldAutoReturn = shouldAutoReturn;
        }
    }

    public class CallNode : NodeBase
    {
        public NodeBase nodeToCall { get; protected set; }
        public NodeBase[] argumentNodes { get; protected set; }

        public CallNode(NodeBase nodeToCall, NodeBase[] argumentNodes)
            : base(nodeToCall.startPosition, (argumentNodes.Length > 0) ? argumentNodes[^1].endPosition : nodeToCall.endPosition)
        {
            this.nodeToCall = nodeToCall;
            this.argumentNodes = argumentNodes;
        }
    }

    public class ReturnNode : NodeBase
    {
        public NodeBase nodeToReturn { get; protected set; }

        public ReturnNode(Position startPosition, Position endPosition)
            : base(startPosition, endPosition) { }
        public ReturnNode(NodeBase nodeToReturn)
            : base(nodeToReturn.startPosition, nodeToReturn.endPosition)
        {
            this.nodeToReturn = nodeToReturn;
        }
    }

    public class ContinueNode : NodeBase
    {
        public ContinueNode(Position startPosition, Position endPosition)
            : base(startPosition, endPosition) { }
    }

    public class BreakNode : NodeBase
    {
        public BreakNode(Position startPosition, Position endPosition)
            : base(startPosition, endPosition) { }
    }
}
