using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Relay
{
    public class Dispatcher
    {
        // local cluster, bind to external clusters
        public bool IsRunning { get; private set; }

        public Dispatcher()
        {
            m_events = new Queue<Action>();
            m_thread = new Thread(RunLoop);
            m_trigger = new ManualResetEvent(false);
        }

        public void Run()
        {
            IsRunning = true;
            m_thread.Start();
        }

        public void Stop()
        {
            IsRunning = false;
            m_trigger.Set();
        }

        public void Process(Action action)
        {
            m_events.Enqueue(action);
            m_trigger.Set();
        }

        private void RunLoop()
        {
            while (IsRunning)
            {
                m_trigger.WaitOne();

                while (m_events.Count > 0)
                {
                    Action action = m_events.Dequeue();
                    ThreadPool.QueueUserWorkItem((o) => action());
                }

                m_trigger.Reset();
            }
        }

        private Queue<Action> m_events;
        private Thread m_thread;
        private ManualResetEvent m_trigger;

        public static readonly Dispatcher Default = new Dispatcher();
    }
}
