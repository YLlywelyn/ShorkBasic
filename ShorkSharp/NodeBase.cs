using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return string.Format("({0})", numToken.value);
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
    }

    public class VarAccessNode : NodeBase
    {
        public Token varNameToken { get; protected set; }

        public VarAccessNode(Token varNameToken)
            : base(varNameToken.startPosition, varNameToken.endPosition)
        {
            this.varNameToken = varNameToken;
        }
    }
}
