using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Relay
{
    public abstract class Channel
    {
        public Type Type { get; private set; }

        public Channel(Type type)
        {
            Type = type;
        }
        public virtual void Bind(Channel channel)
        {

        }
    }

    /// <summary>
    /// Represents a communication channel that can be in process or across the world.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Channel<T> : Channel
    {
        public List<Channel<T>> Links { get; private set; }

        public void Emit(T value)
        {
            foreach (var link in Links)
            {
                Dispatcher.Default.Process(() => link.Recieved(value));
            }
        }

        public event Action<T> Recieved;

        public override void Bind(Channel channel)
        {
            var typedChannel = channel as Channel<T>;

            if (typedChannel != null)
            {
                Links.Add(typedChannel);
            }
        }

        // suspend

        // serialize

        // migrate to endpoint


        public Channel() : base(typeof(T))
        {
            Links = new List<Channel<T>>();
        }
    }

    public class Method<T, TResult> where TResult : class
    {
        public Method()
        {
            
        }

        async Task<TResult> Call(T value)
        {

            return null;
        }
    }


    public interface IOutputChannel<T>
    {
        
    }

    public interface IInputChannel<T>
    {
        // buffer size
        // pending
        // immediate or waiting
    }

    public interface IConnection<T>
    {
        // buffer size
        // block or drop
        // drop strategy (last or random)
    }

    public interface IServiceChannel<U, V>
    {

    }
}
