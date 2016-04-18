using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

namespace Relay
{
    public abstract class Service
    {
        public Service()
        {
            
        }


        public virtual byte[] Pack()
        {
            return new byte[0];
        }

        public virtual void Unpack(byte[] buffer)
        {

        }
    }

    public class TickService : Service, IMethod
    {
        [Interface]
        public IMethod Activator { get; set; }
                
        [Output]
        public event Action<int> Tick;
        
        [Input]
        public void OnTick(int value)
        {
            Console.WriteLine(value);
        }

        [Method]
        public async Task Call()
        {
            //Ticking(101);
            Tick(99);

            //await Activator.Call();

            Tick(100);

            await Task.Delay(1000);
        }

        public TickService()
        {
            // default constructor
        }

        public TickService(int i)
        {
            // custom constructor
        }
    }

    public class LogService : Service
    {
        [Input]
        public void OnWrite(int value)
        {
            Console.WriteLine(value);
        }
    }

    public interface IMethod
    {
        Task Call();
    }

    public interface IMethod<T>
    {
        Task Call(T value);
    }

    public interface IMethod<T, TResult>
    {
        Task<TResult> Call(T value);
    }

    public class Interface<T>
    {
        public T Service { get; set; }
    }
}
