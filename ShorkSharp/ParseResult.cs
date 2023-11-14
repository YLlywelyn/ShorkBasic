namespace ShorkSharp
{
    public class ParseResult
    {
        public ShorkError error { get; protected set; }
        public NodeBase node { get; protected set; }
        public int advanceCount { get; protected set; } = 0;
        public int lastAdvanceCount { get; protected set; } = 0;
        public int toReverseCount { get; protected set; } = 0;

        public ParseResult() { }

        public void RegisterAdvancement()
        {
            lastAdvanceCount = 1;
            advanceCount++;
        }

        public NodeBase Register(ParseResult result)
        {
            lastAdvanceCount = result.advanceCount;
            this.advanceCount += result.advanceCount;
            if (result.error != null) this.error = result.error;
            return result.node;
        }

        public NodeBase TryRegister(ParseResult result)
        {
            if (result.error != null)
            {
                toReverseCount = result.advanceCount;
                return null;
            }
            return Register(result);
        }

        public ParseResult Success(NodeBase node)
        {
            this.node = node;
            return this;
        }

        public ParseResult Failure(ShorkError error)
        {
            if (this.error == null || this.lastAdvanceCount == 0)
                this.error = error;
            return this;
        }
    }

    public class ParseIfResult : ParseResult
    {
        public NodeBase condition { get; protected set; }
        public NodeBase body { get; protected set; }
        public bool shouldReturnNull;

        public (NodeBase condition, NodeBase body) Register(ParseResult condition, ParseResult body)
        {
            lastAdvanceCount = condition.advanceCount + body.advanceCount;
            this.advanceCount += condition.advanceCount + body.advanceCount;
            if (condition.error != null) this.error = condition.error;
            else if (body.error != null) this.error = body.error;
            return (condition.node, body.node);
        }

        public ParseIfResult Success(NodeBase condition, NodeBase body, bool shouldReturnNull)
        {
            this.condition = condition;
            this.body = body;
            this.shouldReturnNull = shouldReturnNull;
            return this;
        }
    }
}
