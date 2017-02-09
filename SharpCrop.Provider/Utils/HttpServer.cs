using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Threading;

// ReSharper disable FunctionNeverReturns

namespace SharpCrop.Provider.Utils
{
    /// <summary>
    /// Simple HTTP server - written by 'aksakalli'.
    /// </summary>
    public class HttpServer
    {
        private readonly IDictionary<string, string> mimeTypeMappings = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase) {
            {".html", "text/html"}
        };

        private readonly string[] indexFiles = {
            "index.html"
        };

        private readonly Action<HttpListenerRequest> onRequest;
        private readonly HttpListener listener;
        private readonly Thread thread;
        private readonly string root;

        /// <summary>
        /// Construct a HTTP server with given port.
        /// </summary>
        /// <param name="root">Directory path to serve.</param>
        /// <param name="port">Port of the server.</param>
        /// <param name="onRequest">Callback function for incoming requests.</param>
        public HttpServer(string root, int port, Action<HttpListenerRequest> onRequest)
        {
            this.onRequest = onRequest;
            this.root = root;

            listener = new HttpListener();
            listener.Prefixes.Add($"http://*:{port}/");
            listener.Start();

            thread = new Thread(Listen);
            thread.Start();
        }

        /// <summary>
        /// Start listening for ever and ever and ever...
        /// </summary>
        private void Listen()
        {
            while (true)
            {
                try
                {
                    Process(listener.GetContext());
                }
                catch
                {
                    // Ignored
                }
            }
        }

        /// <summary>
        /// Process a request.
        /// </summary>
        /// <param name="context"></param>
        private void Process(HttpListenerContext context)
        {
            onRequest(context.Request);

            var filename = context.Request.Url.AbsolutePath.Substring(1);

            if (string.IsNullOrEmpty(filename))
            {
                foreach (var indexFile in indexFiles)
                {
                    if (!File.Exists(Path.Combine(root, indexFile)))
                    {
                        continue;
                    }

                    filename = indexFile;
                    break;
                }
            }

            filename = Path.Combine(root, filename);

            string mime;

            if (File.Exists(filename) && mimeTypeMappings.TryGetValue(Path.GetExtension(filename), out mime))
            {
                try
                {
                    Stream input = new FileStream(filename, FileMode.Open);

                    context.Response.ContentType = mime;
                    context.Response.ContentLength64 = input.Length;
                    context.Response.AddHeader("Date", DateTime.Now.ToString("r"));
                    context.Response.AddHeader("Last-Modified", File.GetLastWriteTime(filename).ToString("r"));

                    int nbytes;
                    byte[] buffer = new byte[1024 * 16];

                    while ((nbytes = input.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        context.Response.OutputStream.Write(buffer, 0, nbytes);
                    }

                    input.Close();

                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    context.Response.OutputStream.Flush();
                }
                catch
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                }
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }

            context.Response.OutputStream.Close();
        }

        /// <summary>
        /// Stop server and dispose everything.
        /// </summary>
        public void Stop()
        {
            listener.Stop();
            thread.Abort();
        }
    }
}