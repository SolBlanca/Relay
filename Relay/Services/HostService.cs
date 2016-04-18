using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Relay.Services
{
    public class HostService
    {

    }

    public class ProcessRequest
    {
        public string Class { get; private set; }
        public Dictionary<string, Address> Inputs { get; private set; }
        public Dictionary<string, Address> Outputs { get; private set; }

    }
}
