using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Relay.Services
{
    public class HttpService : Service
    {
        private HttpListener listener;

        public HttpService()
        {
            listener = new HttpListener();
            listener.Prefixes.Add("http://+:8080/");
            listener.Start();
        }

        public async void Run()
        {
            while (listener.IsListening)
            {
                var context = await listener.GetContextAsync();

                var response = Encoding.Default.GetBytes("Hello World");
                context.Response.OutputStream.Write(response, 0, response.Length);

                context.Response.StatusCode = (int)HttpStatusCode.OK;
                context.Response.OutputStream.Flush();
                context.Response.OutputStream.Close();
                
            }
        }
    }
}
