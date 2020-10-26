namespace System.Net.Http.Formatting
{
    using Collections.Generic;
    using IO;
    using Linq;
    using MessagePack;
    using Models;
    using Threading.Tasks;
    using Xunit;

    public class MessagePackMediaTypeFormatterTest
    {
        private interface IInterface
        {
        }

        private abstract class AbstractClass
        {
        }

        private class NonPublicClass
        {
        }

        private readonly MessagePackMediaTypeFormatter _formatter = new MessagePackMediaTypeFormatter();
        private readonly TransportContext _context = null;
        private readonly IFormatterLogger _logger = null;
        private readonly HttpContent _content;

        public MessagePackMediaTypeFormatterTest()
        {
            _content = new StreamContent(new MemoryStream());
        }

        [Fact]
        public void DefaultMediaType_ReturnsApplicationXMsgPack()
        {
            var mediaType = MessagePackMediaTypeFormatter.DefaultMediaType;
            Assert.NotNull(mediaType);
            Assert.Equal("application/x-msgpack", mediaType.MediaType);
        }

        [Fact]
        public void DefaultConstructor()
        {
            // Act
            var formatter = new MessagePackMediaTypeFormatter();

            // Assert
            Assert.NotNull(formatter.Options);
        }

        [Fact]
        public void CopyConstructor()
        {
            // Act
            var copy = new MessagePackMediaTypeFormatter(_formatter);

            // Assert
            Assert.Same(_formatter.Options, copy.Options);
        }

        [Fact]
        public void ConstructorWithOptions()
        {
            // Arrange
            var options = MessagePackSerializerOptions.Standard;

            // Act
            var formatter = new MessagePackMediaTypeFormatter(options);

            // Assert
            Assert.Same(options, formatter.Options);
        }

        [Theory]
        [InlineData(typeof(IInterface), false)]
        [InlineData(typeof(AbstractClass), false)]
        [InlineData(typeof(NonPublicClass), false)]
        [InlineData(typeof(Dictionary<string, object>), true)]
        [InlineData(typeof(string), true)]
        [InlineData(typeof(SimpleType), true)]
        [InlineData(typeof(ComplexType), true)]
        public void CanReadType_ReturnsFalse_ForAnyUnsupportedModelType(Type modelType, bool expectedCanRead)
        {
            // Act
            var result = _formatter.CanReadType(modelType);

            // Assert
            Assert.Equal(expectedCanRead, result);
        }

        [Theory]
        [InlineData(typeof(IInterface), false)]
        [InlineData(typeof(AbstractClass), false)]
        [InlineData(typeof(NonPublicClass), false)]
        [InlineData(typeof(Dictionary<string, object>), true)]
        [InlineData(typeof(string), true)]
        [InlineData(typeof(SimpleType), true)]
        [InlineData(typeof(ComplexType), true)]
        public void CanWriteType_ReturnsFalse_ForAnyUnsupportedModelType(Type modelType, bool expectedCanRead)
        {
            // Act
            var result = _formatter.CanWriteType(modelType);

            // Assert
            Assert.Equal(expectedCanRead, result);
        }

        [Theory]
        [InlineData("application/msgpack")]
        [InlineData("application/x-msgpack")]
        public void HasProperSupportedMediaTypes(string mediaType)
        {
            // Assert
            Assert.Contains(mediaType, _formatter.SupportedMediaTypes.Select(content => content.ToString()));
        }

        [Fact]
        public async Task ReadFromStreamAsync_WhenTypeIsNull_ThrowsException()
        {
            // Assert
            var stream = new MemoryStream();

            // Act
            var exception =
                await Assert.ThrowsAsync<ArgumentNullException>(
                    () => _formatter.ReadFromStreamAsync(null, stream, _content, _logger));

            // Assert
            Assert.Equal("type", exception.ParamName);
        }

        [Fact]
        public async Task ReadFromStreamAsync_WhenStreamIsNull_ThrowsException()
        {
            // Act
            var exception =
                await Assert.ThrowsAsync<ArgumentNullException>(
                    () => _formatter.ReadFromStreamAsync(typeof(object), null, _content, _logger));

            // Assert
            Assert.Equal("readStream", exception.ParamName);
        }

        [Fact]
        public async Task ReadFromStreamAsync_ReadsNullObject()
        {
            // Assert
            var stream = WriteModel<object>(null);

            // Act
            var result = await _formatter.ReadFromStreamAsync(typeof(object), stream, _content, _logger);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task ReadFromStreamAsync_ReadsBasicType()
        {
            // Arrange
            var expectedInt = 10;
            var stream = WriteModel(expectedInt);

            // Act
            var result = await _formatter.ReadFromStreamAsync(typeof(int), stream, _content, _logger);

            // Assert
            Assert.Equal(expectedInt, result);
        }

        [Fact]
        public async Task ReadFromStreamAsync_ReadsSimpleTypes()
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

            var stream = WriteModel(input);

            // Act
            var result = await _formatter.ReadFromStreamAsync(typeof(SimpleType), stream, _content, _logger);

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
        public async Task ReadFromStreamAsync_ReadsComplexTypes()
        {
            // Arrange
            var input = new ComplexType {Inner = new SimpleType {Property = 10}};

            var stream = WriteModel(input);

            // Act
            var result = await _formatter.ReadFromStreamAsync(typeof(ComplexType), stream, _content, _logger);

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsType<ComplexType>(result);
            Assert.Equal(input.Inner.Property, model.Inner.Property);
        }


        [Fact]
        public async Task WriteToStreamAsync_WhenTypeIsNull_ThrowsException()
        {
            // Assert
            var stream = new MemoryStream();

            // Act
            var exception =
                await Assert.ThrowsAsync<ArgumentNullException>(
                    () => _formatter.WriteToStreamAsync(null, new object(), stream, _content, _context));

            // Assert
            Assert.Equal("type", exception.ParamName);
        }

        [Fact]
        public async Task WriteToStreamAsync__WhenStreamIsNull_ThrowsException()
        {
            // Act
            var exception =
                await Assert.ThrowsAsync<ArgumentNullException>(
                    () => _formatter.WriteToStreamAsync(typeof(object), new object(), null, _content, _context));

            // Assert
            Assert.Equal("writeStream", exception.ParamName);
        }

        [Fact]
        public async Task WriteToStreamAsync_WritesNullObject()
        {
            // Assert
            var stream = new MemoryStream();

            // Act
            await _formatter.WriteToStreamAsync(typeof(object), null, stream, _content, _context);

            // Assert
            var result = ReadModel<object>(stream);
            Assert.Null(result);
        }

        [Fact]
        public async Task WriteToStreamAsync_WritesBasicType()
        {
            // Arrange
            var expectedInt = 10;
            var stream = new MemoryStream();

            // Act
            await _formatter.WriteToStreamAsync(typeof(int), expectedInt, stream, _content, _context);

            // Assert
            var result = ReadModel<int>(stream);
            Assert.Equal(expectedInt, result);
        }

        [Fact]
        public async Task WriteToStreamAsync_WritesSimplesType()
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

            var stream = new MemoryStream();

            // Act
            await _formatter.WriteToStreamAsync(typeof(SimpleType), input, stream, _content, _context);

            // Assert
            var result = ReadModel<SimpleType>(stream);
            Assert.Equal(input.Property, result.Property);
            Assert.Equal(input.Field, result.Field);
            Assert.Equal(input.Enum, result.Enum);
            Assert.Equal(input.Array, result.Array);
            Assert.Equal(input.Nullable, result.Nullable);
        }

        [Fact]
        public async Task WriteToStreamAsync_WritesComplexType()
        {
            // Arrange
            var input = new ComplexType {Inner = new SimpleType {Property = 10}};
            var stream = new MemoryStream();

            // Act
            await _formatter.WriteToStreamAsync(typeof(ComplexType), input, stream, _content, _context);

            // Assert
            var result = ReadModel<ComplexType>(stream);
            Assert.Equal(input.Inner.Property, result.Inner.Property);
        }

        private T ReadModel<T>(Stream stream)
        {

            if (stream.Length == 0)
                return default;

            stream.Position = 0;
            return MessagePackSerializer.Deserialize<T>(stream, _formatter.Options);
        }

        private Stream WriteModel<T>(T model)
        {
            var stream = new MemoryStream();
            if (model != null)
            {
                MessagePackSerializer.Serialize(stream, model, _formatter.Options);
            }

            stream.Position = 0;
            return stream;
        }
    }
}
