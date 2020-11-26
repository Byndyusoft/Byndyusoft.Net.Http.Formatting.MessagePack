using System.Net.Http.Formatting.Models;
using System.Net.Http.MessagePack;
using System.Threading.Tasks;
using MessagePack;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace System.Net.Http.Formatting.Functional
{
    public class MessagePackMediaTypeFormatterFunctionalTest : MvcTestFixture
    {
        private readonly MessagePackMediaTypeFormatter _formatter;
        private readonly MessagePackSerializerOptions _options;

        public MessagePackMediaTypeFormatterFunctionalTest()
        {
            _options = MessagePackSerializerOptions.Standard;
            _formatter = new MessagePackMediaTypeFormatter(_options);
        }

        protected override void ConfigureMvc(IMvcCoreBuilder builder)
        {
            builder.AddMessagePackFormatters(
                options =>
                {
                    options.SerializerOptions = _options;
                });
        }

        protected override void ConfigureHttpClient(HttpClient client)
        {
            client.DefaultRequestHeaders.Accept.Add(MessagePackDefaults.MediaTypeHeader);
        }

        [Fact]
        public async Task PostAsMessagePackAsync()
        {
            // Arrange
            var input = SimpleType.Create();

            // Act
            var response = await Client.PostAsMessagePackAsync("/msgpack-formatter", input, _options);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsAsync<SimpleType>(new[] {_formatter});

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsType<SimpleType>(result);
            model.Verify();
        }

        [Fact]
        public async Task PutAsMessagePackAsync()
        {
            // Arrange
            var input = SimpleType.Create();

            // Act
            var response = await Client.PutAsMessagePackAsync("/msgpack-formatter", input, _options);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsAsync<SimpleType>(new[] {_formatter});

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsType<SimpleType>(result);
            model.Verify();
        }
    }
}
