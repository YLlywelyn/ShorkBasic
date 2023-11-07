using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShorkSharp
{
    public class Position
    {
        public int index { get; protected set; } = 0;
        public int line { get; protected set; } = 0;
        public int column { get; protected set; } = 0;

        public string filename { get; protected set; } = "<stdin>";
        public string filetext { get; protected set; } = string.Empty;

        private Position(int index, int line, int column, string filename, string filetext)
        {
            this.index = index;
            this.line = line;
            this.column = column;
            this.filename = filename;
            this.filetext = filetext;
        }

        public Position(string filetext)
        {
        }
        public Position(string filetext, string filename)
        {
            this.filetext = filetext;
            this.filename = filename;
        }

        public void Advance(char currentCharacter)
        {
            this.index++;
            this.column++;

            if (currentCharacter == '\n')
            {
                this.column = 0;
                this.line++;
            }
        }

        public Position Copy()
        {
            return new Position(index, line, column, filename, filetext);
        }
    }
}
