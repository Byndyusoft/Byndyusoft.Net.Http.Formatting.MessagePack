using System.Net.Http.MessagePack;
using MessagePack.AspNetCoreMvcFormatter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Byndyusoft.Net.Http.Formatting.MessagePack.Example
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddMvcCore(
                options =>
                {
                    options.EnableEndpointRouting = true;
                    options.OutputFormatters.Add(
                        new MessagePackOutputFormatter(MessagePackDefaults.SerializerOptions));
                    options.InputFormatters.Add(
                        new MessagePackInputFormatter(MessagePackDefaults.SerializerOptions));
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}