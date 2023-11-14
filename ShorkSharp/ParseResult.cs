namespace ShorkSharp
{
    public class ParseResult
    {
        public ShorkError error { get; protected set; }
        public dynamic node { get; protected set; }
        public int advanceCount { get; protected set; } = 0;
        public int lastAdvanceCount { get; protected set; } = 0;
        public int toReverseCount { get; protected set; } = 0;

        public ParseResult() { }

        public void RegisterAdvancement()
        {
            lastAdvanceCount = 1;
            advanceCount++;
        }

        public dynamic Register(ParseResult result)
        {
            lastAdvanceCount = result.advanceCount;
            this.advanceCount += result.advanceCount;
            if (result.error != null) this.error = result.error;
            return result.node;
        }

        public dynamic TryRegister(ParseResult result)
        {
            if (result.error != null)
            {
                toReverseCount = result.advanceCount;
                return null;
            }
            return Register(result);
        }

        public ParseResult Success(dynamic node)
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
}
