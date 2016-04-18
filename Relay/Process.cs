using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Relay
{
    public class Process
    {
        public Guid Identifier { get; private set; }
        public Service Service { get; private set; }

        public Dispatcher Scheduler { get; private set; }

        public ProcessState State { get; private set; }

        public Dictionary<string, Channel> Inputs { get; private set; }

        public Dictionary<string, Channel> Outputs { get; private set; }

        public void Migrate(Address endpoint)
        {
            Service.Pack();

            // migrate all channels
        }

        public void Close()
        {
            // close this service and unbind all
        }

        public Process(Dispatcher scheduler)
        {
            Scheduler = scheduler;

            Identifier = Guid.NewGuid();

            // create all channels
            Inputs = new Dictionary<string, Channel>();
            Outputs = new Dictionary<string, Channel>();

            // register this process
            Arbiter.Register(this);
        }

        public static Process CreateProcess<T>() where T : Service, new()
        {
            return CreateProcess(new T());
        }

        public static Process CreateProcess(Service service)
        {
            var process = new Process(null);
            Type type = service.GetType();
            List<InputBinding> inputs = GetInputBindings(type);
            List<OutputBinding> outputs = GetOutputBindings(type);

            foreach (var input in inputs)
            {
                process.Inputs.Add(input.Name, input.Bind(service));
            }

            foreach (var output in outputs)
            {
                process.Outputs.Add(output.Name, output.Bind(service));
            }

            return process;
        }

        private static List<InputBinding> GetInputBindings(Type type)
        {
            var bindings = new List<InputBinding>();
            var methods = type.GetMethods();

            foreach (var method in methods)
            {
                var attribute = method.GetCustomAttribute<InputAttribute>();
                
                if (attribute != null)
                {
                    // is marked as a service input

                    if (method.ReturnType != typeof(void))
                    {
                        throw new ArgumentException("Input must return void");
                    }

                    var parameters = method.GetParameters();
                    
                    if (parameters.Length != 1)
                    {
                        throw new ArgumentException("Input must take one value");
                    }

                    var methodType = parameters[0].ParameterType;
                    var delegateType = typeof(Action<>).MakeGenericType(methodType);
                    var binding = new InputBinding();

                    binding.InputType = methodType;
                    binding.Name = (method.Name.StartsWith("On")) ? method.Name.Substring(2) : method.Name;
                    binding.OnMethod = method;
                    binding.ChannelType = typeof(Channel<>).MakeGenericType(methodType);
                    binding.AddMethod = binding.ChannelType.GetEvent("Recieved").AddMethod;
                    binding.DelegateType = typeof(Action<>).MakeGenericType(methodType);

                    bindings.Add(binding);
                }
            }

            return bindings;
        }

        private static List<OutputBinding> GetOutputBindings(Type type)
        {
            var bindings = new List<OutputBinding>();
            var events = type.GetEvents();

            foreach (var ev in events)
            {
                var attribute = ev.GetCustomAttribute<OutputAttribute>();

                if (attribute != null)
                {
                    // is marked as a service output

                    if (ev.EventHandlerType.GetGenericTypeDefinition() != typeof(Action<>))
                    {
                        throw new ArgumentException("Output must be of type Channel<T>");
                    }

                    var eventType = ev.EventHandlerType.GetGenericArguments()[0];
                    var binding = new OutputBinding();

                    binding.OutputType = eventType;
                    binding.Name = ev.Name;
                    binding.ChannelType = typeof(Channel<>).MakeGenericType(eventType);
                    binding.OnMethod = binding.ChannelType.GetMethod("Emit");
                    binding.AddMethod = ev.AddMethod;
                    binding.DelegateType = typeof(Action<>).MakeGenericType(eventType);

                    bindings.Add(binding);
                }
            }

            return bindings;
        }
    }

    public class OutputBinding
    {
        public Type OutputType { get; set; }
        public string Name { get; set; }
        public Type ChannelType { get; set; }
        public Type DelegateType { get; set; }
        public MethodInfo OnMethod { get; set; }
        public MethodInfo AddMethod { get; set; }

        public Channel Bind(object service)
        {
            var channel = (Channel)Activator.CreateInstance(ChannelType);
            var handler = Delegate.CreateDelegate(DelegateType, channel, OnMethod);
            AddMethod.Invoke(service, new object[] { handler });

            return channel;
        }
    }

    public class InputBinding
    {
        public Type InputType { get; set; }
        public string Name { get; set; }
        public Type ChannelType { get; set; }
        public Type DelegateType { get; set; }
        public MethodInfo OnMethod { get; set; }
        public MethodInfo AddMethod { get; set; }

        public Channel Bind(object service)
        {
            var channel = (Channel)Activator.CreateInstance(ChannelType);
            var action = Delegate.CreateDelegate(DelegateType, service, OnMethod);
            AddMethod.Invoke(channel, new object[] { action });

            return channel;
        }
    }

    public enum ProcessState
    {
        Created,
        Active,
        Migrating,
        Closed
    }
}
