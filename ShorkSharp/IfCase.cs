using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShorkSharp
{
    public struct IfCase
    {
        public NodeBase condition { get; private set; }
        public NodeBase body { get; private set; }
        public bool shouldReturnNull { get; private set; }

        public IfCase(NodeBase condition, NodeBase body, bool shouldReturnNull)
        {
            this.condition = condition;
            this.body = body;
            this.shouldReturnNull = shouldReturnNull;
        }
    }
}
