using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShorkSharp
{
    internal class ShorkError
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
    }

    internal class InvalidCharacterError : ShorkError
    {
        public InvalidCharacterError(string details, Position startPosition)
            : base("Invalid Character", details, startPosition) { }
    }
    
    internal class InvalidSyntaxError : ShorkError
    {
        public InvalidSyntaxError(string details, Position startPosition)
            : base("Invalid Syntax", details, startPosition) { }
    }
}
