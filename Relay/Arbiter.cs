using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NetMQ;
using NetMQ.Sockets;

namespace Relay
{
    public class Peer
    {
        public RequestSocket Connection { get; set; }
    }

    public class Arbiter
    {
        public ResponseSocket Listener { get; set; }

        public Peer Master { get; set; }

        public List<Peer> Children { get; set; }

        public Arbiter(bool isHost)
        {
            if (isHost)
            {
                Listener = new ResponseSocket("@tcp://*:2650");
                Listener.ReceiveReady += OnRecieveReady;

                RunServer();
            }
            else
            {
                var peer = new Peer();
                peer.Connection = new RequestSocket(">tcp://127.0.0.1:2650");
                //peer.Connection.Connect("tcp://127.0.0.1:2650");
                
                Master = peer;
                RunClient();
            }
        }

        public void OnRecieveReady(object sender, NetMQSocketEventArgs e)
        {
            Console.WriteLine(Listener.ReceiveFrameString());
            Listener.SendFrame("Bye");
        }

        public void Send(string value)
        {
            Master.Connection.SendFrame(value);
        }

        public async void RunServer()
        {
            while (true)
            {
                await Task.Delay(1000);
                var message = Listener.ReceiveFrameString();
                Console.WriteLine(message);
                Listener.SendFrame("Back");
            }
        }

        public async void RunClient()
        {
            while (true)
            {
                Master.Connection.SendFrame("Yo");

                await Task.Delay(1000);
                var message = Master.Connection.ReceiveFrameString();
                Console.WriteLine(message);
            }
        }




        // outbound channels

        // inbound channels

        // external interfaces

        // internal interfaces

        // ServiceInfo for all services.

        // TypeInfo for all services. Is compatable, etc.
        public static Dictionary<Guid, Process> Processes = new Dictionary<Guid, Process>();
        public static void Register(Process process)
        {
            Processes.Add(process.Identifier, process);
        }

        public static void Bind(Guid source, string output, Guid destination, string input)
        {
            var sourceProcess = Processes[source];
            var destinationProcess = Processes[destination];

            var outputChannel = sourceProcess.Outputs[output];
            var inputChannel = destinationProcess.Inputs[input];

            outputChannel.Bind(inputChannel);
        }

        public static void Serve()
        {

        }

        public static void Host()
        {

        }
    }
}
