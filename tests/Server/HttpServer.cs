namespace System.Net.Http.Server
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Hosting;
    using Sockets;

    public class HttpServer : IDisposable
    {
        private static int GetAvailablePort()
        {
            using var udp = new UdpClient(0, AddressFamily.InterNetwork);
            return ((IPEndPoint) udp.Client.LocalEndPoint).Port;
        }

        internal class Startup
        {
            public void Configure(IApplicationBuilder app)
            {
                app.Run(async context => { await context.Response.WriteAsync(string.Empty); });
            }
        }

        private IHost _host;
        private static readonly object Lock = new object();

        public string Start()
        {
            lock (Lock)
            {
                var uri = $"http://localhost:{GetAvailablePort()}";
                _host = Host.CreateDefaultBuilder()
                    .ConfigureWebHostDefaults(
                        webBuilder =>
                        {
                            webBuilder.UseStartup<Startup>();
                            webBuilder.UseUrls(uri);
                        })
                    .Build();
                _host.Start();
                return uri;
            }
        }

        public void Dispose()
        {
            _host.StopAsync().GetAwaiter().GetResult();
            _host.Dispose();
        }
    }
}