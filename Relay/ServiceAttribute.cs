using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Relay
{
    public class ServiceAttribute : Attribute
    {
        public string Name { get; private set; }

        public ServiceAttribute(string name)
        {
            Name = name;
        }
    }
}
