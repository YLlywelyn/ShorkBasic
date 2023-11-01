using ShorkBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShorkBasic
{
    abstract internal class ShorkObject
    {
        public dynamic value { get; protected set; }
        public Context context { get; protected set; }
        public Position startPosition { get; protected set; }
        public Position endPosition { get; protected set; }

        public ShorkObject(dynamic value)
        {
            this.value = value;
        }

        public ShorkObject SetPosition(Position startPosition, Position endPosition)
        {
            this.startPosition = startPosition;
            this.endPosition = endPosition;
            return this;
        }

        public ShorkObject SetContext(Context context)
        {
            this.context = context;
            return this;
        }
    }

    internal class ShorkNumber : ShorkObject
    {
        new public double value { get; protected set; }

        public ShorkNumber(double value)
            : base(value) { }
    }
}
