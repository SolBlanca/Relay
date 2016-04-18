using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Relay;
using Relay.Services;
using MsgPack;
using ZMQ;
using System.Reflection;

namespace CLI
{
    public class Program
    {
        public static void Call(Action<int> action)
        {
            action(3);
        }

        static void Main(string[] args)
        {
            Arbiter host = new Arbiter(true);
            Arbiter client = new Arbiter(false);

            Dispatcher.Default.Run();

            var t = new Test()
            {
                Red = 1,
                Blue = "hello"
            };

            var s1 = MsgPack.Serialization.SerializationContext.Default.GetSerializer<Test>();
            var s2 = MsgPack.Serialization.SerializationContext.Default.GetSerializer<Test2>();
            
            byte[] b = s1.PackSingleObject(t);

            var t1 = s2.UnpackSingleObject(b);

            TickService ts = new TickService();

            Process tp = Process.CreateProcess(ts);
            Process lp = Process.CreateProcess<LogService>();

            
            Arbiter.Bind(tp.Identifier, "Tick", lp.Identifier, "Write");

            Task task = ts.Call();
            task.Wait();

            HttpService service = new HttpService();
            service.Run();

            Console.WriteLine("Complete");

            Console.ReadKey();

            // Arbiter.Bind("axiverse.com/Local.OnTap", "axiverse.com/Matrix.OnTap");
            // looks up host with default port (2950) and then requests endpoint OnTap for local object
            Dispatcher.Default.Stop();


        }

        public class Test
        {
            public int Red { get; set; }
            public string Blue { get; set; }
        }

        public class Test2
        {
            public int A1 { get; set; }
            public string A0 { get; set; }
        }

    }
}
