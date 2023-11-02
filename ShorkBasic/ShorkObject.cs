using DecimalMath;
using ShorkBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShorkBasic
{
    abstract public class ShorkObject
    {
        public object value { get; protected set; }
        public Context context { get; protected set; }
        public Position startPosition { get; protected set; }
        public Position endPosition { get; protected set; }

        public ShorkObject(object value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return value.ToString();
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

        public virtual ShorkObject Add(ShorkObject other)
        {
            throw new RuntimeError(this.startPosition, other.endPosition,
                                    string.Format("The '+' operator is not defined for types '{0}' and '{1}'.",
                                                    this.GetType().Name, other.GetType().Name),
                                                    this.context);
        }
        public virtual ShorkObject Subtract(ShorkObject other)
        {
            throw new RuntimeError(this.startPosition, other.endPosition,
                                    string.Format("The '-' operator is not defined for types '{0}' and '{1}'.",
                                                    this.GetType().Name, other.GetType().Name),
                                                    this.context);
        }
        public virtual ShorkObject MultiplyBy(ShorkObject other)
        {
            throw new RuntimeError(this.startPosition, other.endPosition,
                                    string.Format("The '*' operator is not defined for types '{0}' and '{1}'.",
                                                    this.GetType().Name, other.GetType().Name),
                                                    this.context);
        }
        public virtual ShorkObject DivideBy(ShorkObject other)
        {
            throw new RuntimeError(this.startPosition, other.endPosition,
                                    string.Format("The '/' operator is not defined for types '{0}' and '{1}'.",
                                                    this.GetType().Name, other.GetType().Name),
                                                    this.context);
        }
        public virtual ShorkObject PowerOf(ShorkObject other)
        {
            throw new RuntimeError(this.startPosition, other.endPosition,
                                    string.Format("The '^' operator is not defined for types '{0}' and '{1}'.",
                                                    this.GetType().Name, other.GetType().Name),
                                                    this.context);
        }
    }

    public class ShorkNumber : ShorkObject
    {
        new public decimal value
        {
            get { return (decimal)base.value; }
            protected set { base.value = value; }
        }

        public ShorkNumber(decimal value)
            : base(value) { }

        public override string ToString()
        {
            return base.ToString();
        }

        public override ShorkObject Add(ShorkObject other)
        {
            switch (other)
            {
                default:
                    return base.Add(other);
                case ShorkNumber:
                    return new ShorkNumber(this.value + ((ShorkNumber)other).value);
            }
        }

        public override ShorkObject Subtract(ShorkObject other)
        {
            switch (other)
            {
                default:
                    return base.Subtract(other);
                case ShorkNumber:
                    return new ShorkNumber(this.value - ((ShorkNumber)other).value);
            }
        }

        public override ShorkObject MultiplyBy(ShorkObject other)
        {
            switch (other)
            {
                default:
                    return base.MultiplyBy(other);
                case ShorkNumber:
                    return new ShorkNumber(this.value * ((ShorkNumber)other).value);
            }
        }

        public override ShorkObject DivideBy(ShorkObject other)
        {
            switch (other)
            {
                default:
                    return base.DivideBy(other);
                case ShorkNumber:
                    if (((ShorkNumber)other).value == 0)
                        throw new RuntimeError(this.startPosition, other.endPosition,
                                                "Attempt to divide by zero.",
                                                context);
                    return new ShorkNumber(this.value * ((ShorkNumber)other).value);
            }
        }

        public override ShorkObject PowerOf(ShorkObject other)
        {
            switch (other)
            {
                default:
                    return base.PowerOf(other);
                case ShorkNumber:
                    return new ShorkNumber(DecimalEx.Pow(this.value, ((ShorkNumber)other).value));
            }
        }
    }
}
