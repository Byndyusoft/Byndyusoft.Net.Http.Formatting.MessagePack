using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.MessagePack;
using System.Threading;
using System.Threading.Tasks;
using Byndyusoft.Net.Http.Formatting.MessagePack.Example.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace Byndyusoft.Net.Http.Formatting.MessagePack.Example
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            using var host = CreateHostBuilder(args).Build();
            var cts = new CancellationTokenSource();
            await host.StartAsync(cts.Token);
            await MakeCallsAsync();
            cts.Cancel();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls("http://localhost:8080");
                });
        }

        private static async Task MakeCallsAsync()
        {
            var people = new People
            {
                Id = 1,
                Name = "Donald Trump",
                DateOfBirth = new DateTime(1946, 6, 14)
            };

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(MessagePackDefaults.MediaTypeHeader);

            var post = await httpClient.PostAsMessagePackAsync("http://localhost:8080/peoples", people);
            post.EnsureSuccessStatusCode();

            var get = await httpClient.GetAsync("http://localhost:8080/peoples/1");
            get.EnsureSuccessStatusCode();
            var data = await get.Content.ReadAsAsync<People>(new[]
            {
                new MessagePackMediaTypeFormatter()
            });

            Console.WriteLine($"Received {JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented)}");
        }
    }
}