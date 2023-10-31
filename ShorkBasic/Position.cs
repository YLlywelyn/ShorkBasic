namespace ShorkBasic
{
    public class Position
    {
        public int index { get; protected set; }
        public int line { get; protected set; }
        public int column { get; protected set; }
        public string filename { get; protected set; }
        
        public Position(string filename, int index = -1, int line = 0, int column = -1)
        {
            this.index = index;
            this.line = line;
            this.column = column;
            this.filename = filename;
        }
        
        public Position Copy()
        {
            return new Position(filename, index, line, column);
        }
        
        public void Advance(bool newLine = false)
        {
            this.index++;
            this.column++;
            
            if (newLine)
            {
                this.column = 0;
                this.line++;
            }
        }
    }
}
