using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Relay
{
    /// <summary>
    /// Represents an endpoint in both absolute and relative terms
    /// </summary>
    public class Address
    {
        // ipaddress

        // instance id

        // server address
        public Guid Identifier { get; set; }

        public Type Type { get; set; }
    }

    public enum AddressType
    {
        Input,
        Output,
    }
}
