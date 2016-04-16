using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Relay
{
    public class Cluster
    {
        // bind parent


        public Signature Spawn(Task task)
        {
            return null;
        }

        public Signature Spawn(Template template)
        {
            return null;
        }
    }

    public class Signature
    {
        public IList<Address> Inputs { get; private set; }
        public IList<Address> Outputs { get; private set; }
        public IList<Address> Services { get; private set; }

    }

    public class Template
    {
        // processes

        // interconnects

        // inputs, outputs, services
    }
}
