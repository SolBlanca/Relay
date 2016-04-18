using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Relay
{
    public class InputAttribute : Attribute
    {
        public string Name { get; private set; }

        public InputAttribute()
        {

        }
        public InputAttribute(string name)
        {
            Name = name;
        }
    }
}
