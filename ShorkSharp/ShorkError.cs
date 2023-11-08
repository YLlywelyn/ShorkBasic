using System.Text;

namespace ShorkSharp
{
    public class ShorkError
    {
        public string errorName { get; protected set; }
        public string details { get; protected set; }

        public Position startPosition { get; protected set; }

        public ShorkError(string errorName, string details, Position startPosition)
        {
            this.errorName = errorName;
            this.details = details;
            this.startPosition = startPosition;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("{0}: {1}", errorName, details);

            if (startPosition != null)
                sb.AppendFormat("\nFile: '{0}', line {1}", startPosition.filename, startPosition.line+1);

            return sb.ToString();
        }
    }

    public class InvalidCharacterError : ShorkError
    {
        public InvalidCharacterError(string details, Position startPosition)
            : base("Invalid Character", details, startPosition) { }
    }

    public class InvalidSyntaxError : ShorkError
    {
        public InvalidSyntaxError(string details, Position startPosition)
            : base("Invalid Syntax", details, startPosition) { }
    }

    public class InvalidEscapeSequenceError : ShorkError
    {
        public InvalidEscapeSequenceError(string details, Position startPosition)
            : base("Invalid Escape Sequence", details, startPosition) { }
    }
}
