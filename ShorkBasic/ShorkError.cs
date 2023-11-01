namespace ShorkBasic
{
    public class ShorkError : Exception
    {
        public Position startPosition { get; protected set; }
        public Position endPosition { get; protected set; }
        public string errorName { get; protected set; }
        public string details { get; protected set; }
        
        public ShorkError(Position startPosition, Position endPosition, string errorName, string details)
        {
            this.startPosition = startPosition;
            this.endPosition = endPosition;
            this.errorName = errorName;
            this.details = details;
        }
        
        public override string ToString()
        {
            return string.Format("{0}: {1}\nFile: '{2}' Line {3}", errorName, details, startPosition.filename, startPosition.line+1);
        }
    }

    public class InvalidCharacterError : ShorkError
    {
        public InvalidCharacterError(Position startPosition, Position endPosition, string details)
            : base(startPosition, endPosition, "Invalid Character", details) {}
    }

    public class InvalidSyntaxError : ShorkError
    {
        public InvalidSyntaxError(Position startPosition, Position endPosition, string details)
            : base(startPosition, endPosition, "Invalid Syntax", details) { }
    }

    public class RuntimeError : ShorkError
    {
        public Context context {get; protected set; }

        public RuntimeError(Position startPosition, Position endPosition, string details)
            : base(startPosition, endPosition, "Runtime Error", details) { }
    }
}