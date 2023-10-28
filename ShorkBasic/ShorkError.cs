namespace ShorkBasic
{
    internal class ShorkError : Exception
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
            return string.Format("{0}", errorName);
        }
    }
}