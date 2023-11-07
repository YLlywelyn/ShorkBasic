namespace ShorkSharp
{
    public class ParseResult
    {
        public ShorkError error { get; protected set; }
        public NodeBase node { get; protected set; }
        public int advanceCount { get; protected set; } = 0;

        public ParseResult() { }

        public void RegisterAdvancement()
        {
            advanceCount++;
        }

        public NodeBase Register(ParseResult result)
        {
            this.advanceCount += result.advanceCount;
            if (result.error != null) this.error = result.error;
            return result.node;
        }

        public ParseResult Success(NodeBase node)
        {
            this.node = node;
            return this;
        }

        public ParseResult Failure(ShorkError error)
        {
            if (this.error == null || this.advanceCount == 0)
                this.error = error;
            return this;
        }
    }
}
