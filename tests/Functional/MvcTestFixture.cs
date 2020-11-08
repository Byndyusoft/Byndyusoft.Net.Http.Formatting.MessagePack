using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Net.Sockets;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace System.Net.Http.Tests.Functional
{
    public abstract class MvcTestFixture : IDisposable
    {
        private IHost _host;

        protected MvcTestFixture()
        {
            var url = $"http://localhost:{FreeTcpPort()}";
            _host =
                Host.CreateDefaultBuilder()
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseUrls(url);
                        webBuilder.ConfigureServices(ConfigureServices);
                        webBuilder.Configure(Configure);
                    })
                    .Build();
            _host.Start();
            Client = new HttpClient {BaseAddress = new Uri(url)};
            Client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue(MessagePackMediaTypes.ApplicationXMessagePack));
        }

        protected HttpClient Client { get; private set; }

        public virtual void Dispose()
        {
            _host?.Dispose();
            _host = null;

            Client?.Dispose();
            Client = null;
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(c => c.ClearProviders());
            services.AddControllers();
            services.AddMvcCore(ConfigureMvc);
        }

        protected abstract void ConfigureMvc(MvcOptions options);

        private static int FreeTcpPort()
        {
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            var port = ((IPEndPoint) listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }
    }
}