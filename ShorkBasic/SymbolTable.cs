using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShorkBasic
{
    public class SymbolTable
    {
        Dictionary<string, ShorkObject> symbols {get; set; }
        public SymbolTable parent { get; protected set; }

        public SymbolTable(SymbolTable parent = null)
        {
            symbols = new Dictionary<string, ShorkObject>();
            this.parent = parent;
        }

        public ShorkObject Get(string key)
        {
            if (symbols.ContainsKey(key))
                return symbols[key];
            else if (parent != null)
                return parent.Get(key);
            else
                return null;
        }

        public void Set(string key, ShorkObject value)
        {
            symbols[key] = value;
        }

        public void UnSet(string key)
        {
            symbols.Remove(key);
        }
    }
}
