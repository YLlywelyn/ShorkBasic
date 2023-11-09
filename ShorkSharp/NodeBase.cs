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
}
