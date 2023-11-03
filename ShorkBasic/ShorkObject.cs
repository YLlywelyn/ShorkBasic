using DecimalMath;
using ShorkBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ShorkBasic
{
    public class ShorkObject
    {
        public dynamic value { get; protected set; }
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

        public dynamic SetPosition(Position startPosition, Position endPosition)
        {
            this.startPosition = startPosition;
            this.endPosition = endPosition;
            return this;
        }

        public dynamic SetContext(Context context)
        {
            this.context = context;
            return this;
        }

        public dynamic Copy()
        {
            switch (this)
            {
                default:
                    throw new RuntimeError(this.startPosition, this.endPosition,
                                                string.Format("Copying not implemented for type '{0}'",
                                                    this.GetType().Name),
                                                    this.context);
                case ShorkNumber:
                    return new ShorkNumber(this.value).SetContext(context).SetPosition(startPosition, endPosition);
                case ShorkBool:
                    return new ShorkBool(this.value).SetContext(context).SetPosition(startPosition, endPosition);
            }
        }

        public virtual dynamic Add(ShorkObject other)
        {
            throw new RuntimeError(this.startPosition, other.endPosition,
                                    string.Format("The '+' operator is not defined for types '{0}' and '{1}'.",
                                                    this.GetType().Name, other.GetType().Name),
                                                    this.context);
        }
        public virtual dynamic Subtract(ShorkObject other)
        {
            throw new RuntimeError(this.startPosition, other.endPosition,
                                    string.Format("The '-' operator is not defined for types '{0}' and '{1}'.",
                                                    this.GetType().Name, other.GetType().Name),
                                                    this.context);
        }
        public virtual dynamic MultiplyBy(ShorkObject other)
        {
            throw new RuntimeError(this.startPosition, other.endPosition,
                                    string.Format("The '*' operator is not defined for types '{0}' and '{1}'.",
                                                    this.GetType().Name, other.GetType().Name),
                                                    this.context);
        }
        public virtual dynamic DivideBy(ShorkObject other)
        {
            throw new RuntimeError(this.startPosition, other.endPosition,
                                    string.Format("The '/' operator is not defined for types '{0}' and '{1}'.",
                                                    this.GetType().Name, other.GetType().Name),
                                                    this.context);
        }
        public virtual dynamic PowerOf(ShorkObject other)
        {
            throw new RuntimeError(this.startPosition, other.endPosition,
                                    string.Format("The '^' operator is not defined for types '{0}' and '{1}'.",
                                                    this.GetType().Name, other.GetType().Name),
                                                    this.context);
        }
        public virtual ShorkBool CompareEquals(ShorkObject other)
        {
            throw new RuntimeError(this.startPosition, other.endPosition,
                                    string.Format("The '==' operator is not defined for types '{0}' and '{1}'.",
                                                    this.GetType().Name, other.GetType().Name),
                                                    this.context);
        }
        public virtual ShorkBool CompareNotEquals(ShorkObject other)
        {
            throw new RuntimeError(this.startPosition, other.endPosition,
                                    string.Format("The '!=' operator is not defined for types '{0}' and '{1}'.",
                                                    this.GetType().Name, other.GetType().Name),
                                                    this.context);
        }
        public virtual ShorkBool CompareLessThan(ShorkObject other)
        {
            throw new RuntimeError(this.startPosition, other.endPosition,
                                    string.Format("The '<' operator is not defined for types '{0}' and '{1}'.",
                                                    this.GetType().Name, other.GetType().Name),
                                                    this.context);
        }
        public virtual ShorkBool CompareLessThanOrEqual(ShorkObject other)
        {
            throw new RuntimeError(this.startPosition, other.endPosition,
                                    string.Format("The '<=' operator is not defined for types '{0}' and '{1}'.",
                                                    this.GetType().Name, other.GetType().Name),
                                                    this.context);
        }
        public virtual ShorkBool CompareGreaterThan(ShorkObject other)
        {
            throw new RuntimeError(this.startPosition, other.endPosition,
                                    string.Format("The '>' operator is not defined for types '{0}' and '{1}'.",
                                                    this.GetType().Name, other.GetType().Name),
                                                    this.context);
        }
        public virtual ShorkBool CompareGreaterThanOrEqual(ShorkObject other)
        {
            throw new RuntimeError(this.startPosition, other.endPosition,
                                    string.Format("The '>=' operator is not defined for types '{0}' and '{1}'.",
                                                    this.GetType().Name, other.GetType().Name),
                                                    this.context);
        }
        public virtual ShorkBool AndWith(ShorkObject other)
        {
            throw new RuntimeError(this.startPosition, other.endPosition,
                                    string.Format("The 'AND' operator is not defined for types '{0}' and '{1}'.",
                                                    this.GetType().Name, other.GetType().Name),
                                                    this.context);
        }
        public virtual ShorkBool OredWith(ShorkObject other)
        {
            throw new RuntimeError(this.startPosition, other.endPosition,
                                    string.Format("The 'OR' operator is not defined for types '{0}' and '{1}'.",
                                                    this.GetType().Name, other.GetType().Name),
                                                    this.context);
        }
        public virtual dynamic Notted()
        {
            throw new RuntimeError(this.startPosition, this.endPosition,
                                    string.Format("The 'NOT' operator is not defined for '{0}'.",
                                                    this.GetType().Name),
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

        public override dynamic Add(ShorkObject other)
        {
            switch (other)
            {
                default:
                    return base.Add(other);
                case ShorkNumber:
                    return new ShorkNumber(this.value + ((ShorkNumber)other).value).SetContext(context);
            }
        }
        public override dynamic Subtract(ShorkObject other)
        {
            switch (other)
            {
                default:
                    return base.Subtract(other);
                case ShorkNumber:
                    return new ShorkNumber(this.value - ((ShorkNumber)other).value).SetContext(context);
            }
        }
        public override dynamic MultiplyBy(ShorkObject other)
        {
            switch (other)
            {
                default:
                    return base.MultiplyBy(other);
                case ShorkNumber:
                    return new ShorkNumber(this.value * ((ShorkNumber)other).value).SetContext(context);
            }
        }
        public override dynamic DivideBy(ShorkObject other)
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
                    return new ShorkNumber(this.value * ((ShorkNumber)other).value).SetContext(context);
            }
        }
        public override dynamic PowerOf(ShorkObject other)
        {
            switch (other)
            {
                default:
                    return base.PowerOf(other);
                case ShorkNumber:
                    return new ShorkNumber(DecimalEx.Pow(this.value, ((ShorkNumber)other).value)).SetContext(context);
            }
        }
        public override ShorkBool CompareEquals(ShorkObject other)
        {
            switch (other)
            {
                default:
                    return base.CompareEquals(other);
                case ShorkNumber:
                    return (ShorkBool)new ShorkBool(this.value == ((ShorkNumber)other).value).SetContext(context);
            }
        }
        public override ShorkBool CompareNotEquals(ShorkObject other)
        {
            switch (other)
            {
                default:
                    return base.CompareEquals(other);
                case ShorkNumber:
                    return (ShorkBool)new ShorkBool(this.value != ((ShorkNumber)other).value).SetContext(context);
            }
        }
        public override ShorkBool CompareLessThan(ShorkObject other)
        {
            switch (other)
            {
                default:
                    return base.CompareEquals(other);
                case ShorkNumber:
                    return (ShorkBool)new ShorkBool(this.value < ((ShorkNumber)other).value).SetContext(context);
            }
        }
        public override ShorkBool CompareLessThanOrEqual(ShorkObject other)
        {
            switch (other)
            {
                default:
                    return base.CompareEquals(other);
                case ShorkNumber:
                    return (ShorkBool)new ShorkBool(this.value <= ((ShorkNumber)other).value).SetContext(context);
            }
        }
        public override ShorkBool CompareGreaterThan(ShorkObject other)
        {
            switch (other)
            {
                default:
                    return base.CompareEquals(other);
                case ShorkNumber:
                    return (ShorkBool)new ShorkBool(this.value > ((ShorkNumber)other).value).SetContext(context);
            }
        }
        public override ShorkBool CompareGreaterThanOrEqual(ShorkObject other)
        {
            switch (other)
            {
                default:
                    return base.CompareEquals(other);
                case ShorkNumber:
                    return (ShorkBool)new ShorkBool(this.value >= ((ShorkNumber)other).value).SetContext(context);
            }
        }
    }

    public class ShorkBool : ShorkObject
    {
        new public bool value
        {
            get { return (bool)base.value; }
            protected set { base.value = value; }
        }

        public ShorkBool(bool value)
            : base(value) { }

        new public ShorkBool Copy()
        {
            return (ShorkBool)base.Copy();
        }

        public override ShorkBool CompareEquals(ShorkObject other)
        {
            switch (other)
            {
                default:
                    return base.CompareEquals(other);
                case ShorkBool:
                    return (ShorkBool)new ShorkBool(this.value == ((ShorkBool)other).value).SetContext(context);
            }
        }
        public override ShorkBool CompareNotEquals(ShorkObject other)
        {
            switch (other)
            {
                default:
                    return base.CompareEquals(other);
                case ShorkBool:
                    return (ShorkBool)new ShorkBool(this.value != ((ShorkBool)other).value).SetContext(context);
            }
        }
        public override ShorkBool AndWith(ShorkObject other)
        {
            switch (other)
            {
                default:
                    return base.CompareEquals(other);
                case ShorkBool:
                    return (ShorkBool)new ShorkBool(this.value && ((ShorkBool)other).value).SetContext(context);
            }
        }
        public override ShorkBool OredWith(ShorkObject other)
        {
            switch (other)
            {
                default:
                    return base.CompareEquals(other);
                case ShorkBool:
                    return (ShorkBool)new ShorkBool(this.value || ((ShorkBool)other).value).SetContext(context);
            }
        }
        public override dynamic Notted()
        {
            return (ShorkBool)new ShorkBool(!this.value).SetContext(context);
        }
    }
}
