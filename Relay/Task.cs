using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Relay
{
    public class Task
    {
        [Output("Mute")]
        public Channel<Pass> Mute { get; set; }
        
        [Input("Tick")]
        public void OnTick(Pass value)
        {

        }

        [Service("Name")]
        public int Name(Pass value)
        {
            return 0;
        }
    }

    public struct Pass
    {
        // hash structure signature
        // size
    }
}
