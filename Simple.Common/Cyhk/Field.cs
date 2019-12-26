using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simple.Common.Cyhk
{
    public class Field
    {
        public Range Range { get; private set; }

        public Type ValueType { get; private set; }
        public string Name { get; private set; }
        public string Desc { get; private set; }
        public Field(Range range, Type valueType, string name, string desc = null)
        {
            this.Range = range;
            this.ValueType = valueType;
            this.Name = name;
            this.Desc = desc;
        }
    }
}
