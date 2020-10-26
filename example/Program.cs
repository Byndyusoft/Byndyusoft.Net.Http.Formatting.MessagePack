namespace Byndyusoft.Net.Http.Formatting.MessagePack.Example
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Threading;
    using System.Threading.Tasks;
    using Models;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;
    using Newtonsoft.Json;

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

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls("http://*:8080");
                });

        private static async Task MakeCallsAsync()
        {
            var people = new People
            {
                Id = 1,
                Name = "Donald Trump",
                DateOfBirth = new DateTime(1946, 6, 14)
            };

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(MessagePackMediaTypeFormatter.DefaultMediaType);

            var post = await httpClient.PostAsMessagePackAsync("http://localhost:8080/peoples", people);
            post.EnsureSuccessStatusCode();

            var get = await httpClient.GetAsync("http://localhost:8080/peoples/1");
            get.EnsureSuccessStatusCode();
            var data = await get.Content.ReadAsAsync<People>(new[]
            {
                new MessagePackMediaTypeFormatter()
            });

            Console.WriteLine($"Received {JsonConvert.SerializeObject(data, Formatting.Indented)}");
        }
    }
}
