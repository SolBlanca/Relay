using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Relay
{
    public class OutputAttribute : Attribute
    {
        public string Name { get; private set; }

        public OutputAttribute()
        {

        }

        public OutputAttribute(string name)
        {
            Name = name;
        }
    }
}
