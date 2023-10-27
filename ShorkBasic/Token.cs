using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShorkBasic
{
    internal class Token
    {
        public TokenType type;
        public dynamic value;

        public Token(TokenType type, dynamic value = null)
        {
            this.type = type;
            this.value = value;
        }

        public override string ToString()
        {
            if ((object)value == null) return string.Format("{{{0}}}", type);
            else return string.Format("{{{0}: {1}}}", type, value);
        }

        public bool Matches(TokenType type)
        {
            return this.type == type;
        }
        public bool Matches(TokenType type, dynamic value)
        {
            return (this.type == type) && (this.value == value);
        }
    }
}
