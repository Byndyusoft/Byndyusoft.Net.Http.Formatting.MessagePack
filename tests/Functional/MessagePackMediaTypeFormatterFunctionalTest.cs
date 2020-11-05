namespace System.Net.Http.Functional
{
    using Formatting;
    using IO;
    using MessagePack;
    using MessagePack.AspNetCoreMvcFormatter;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Threading.Tasks;
    using Xunit;

    public class MessagePackMediaTypeFormatterFunctionalTest : MvcTestFixture
    {
        private readonly MessagePackSerializerOptions _options;
        private readonly MessagePackMediaTypeFormatter _formatter;

        public MessagePackMediaTypeFormatterFunctionalTest()
        {
            _options = MessagePackSerializerOptions.Standard;
            _formatter = new MessagePackMediaTypeFormatter(_options);
        }

        protected override void ConfigureMvc(MvcOptions options)
        {
            options.OutputFormatters.Add(new MessagePackOutputFormatter(_options));
            options.InputFormatters.Add(new MessagePackInputFormatter(_options));
        }

        [Fact]
        public async Task PostAsMessagePackAsync()
        {
            // Arrange
            var input = new SimpleType
                        {
                            Property = 10,
                            Enum = SeekOrigin.Current,
                            Field = "string",
                            Array = new[] {1, 2},
                            Nullable = 100
                        };

            // Act
            var response = await Client.PostAsMessagePackAsync("/msgpack-formatter", input, _options);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsAsync<SimpleType>(new[] {_formatter});

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsType<SimpleType>(result);

            Assert.Equal(input.Property, model.Property);
            Assert.Equal(input.Field, model.Field);
            Assert.Equal(input.Enum, model.Enum);
            Assert.Equal(input.Array, model.Array);
            Assert.Equal(input.Nullable, model.Nullable);
        }

        [Fact]
        public async Task PutAsMessagePackAsync()
        {
            // Arrange
            var input = new SimpleType
                        {
                            Property = 10,
                            Enum = SeekOrigin.Current,
                            Field = "string",
                            Array = new[] { 1, 2 },
                            Nullable = 100
                        };

            // Act
            var response = await Client.PutAsMessagePackAsync("/msgpack-formatter", input, _options);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsAsync<SimpleType>(new[] { _formatter });

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsType<SimpleType>(result);

            Assert.Equal(input.Property, model.Property);
            Assert.Equal(input.Field, model.Field);
            Assert.Equal(input.Enum, model.Enum);
            Assert.Equal(input.Array, model.Array);
            Assert.Equal(input.Nullable, model.Nullable);
        }
    }
}