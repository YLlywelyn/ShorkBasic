using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShorkSharp
{
    internal class ParseResult
    {
        public ShorkError error { get; protected set; }

        public ParseResult()
        {
        }

        public ParseResult Success(ParseResult parseResult)
        {

        }
    }
}
