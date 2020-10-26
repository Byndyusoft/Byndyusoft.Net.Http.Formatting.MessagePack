namespace System.Net.Http
{
    using Formatting;
    using MessagePack;
    using Threading;
    using Threading.Tasks;
    using Xunit;

    public class HttpClientPostExtensionsTest
    {
        private readonly HttpClient _client;
        private readonly string _uri = "http://localhost/";
        private readonly MessagePackSerializerOptions _options;

        public HttpClientPostExtensionsTest()
        {
            _client = new HttpClient(FakeHttpMessageHandler.Instance);
            _options = MessagePackSerializerOptions.Standard;
        }

        [Fact]
        public async Task PostAsMessagePackAsync_String_WhenClientIsNull_ThrowsException()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => ((HttpClient)null).PostAsMessagePackAsync(_uri, new object()));
            Assert.Equal("client", exception.ParamName);
        }

        [Fact]
        public async Task PostAsMessagePackAsync_String_WhenOptionsIsNull_ThrowsException()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _client.PostAsMessagePackAsync(_uri, new object(), null, CancellationToken.None));
            Assert.Equal("options", exception.ParamName);
        }

        [Fact]
        public async Task PostAsMessagePackAsync_String_WhenUriIsNull_ThrowsException()
        {
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _client.PostAsMessagePackAsync((string)null, new object()));
            Assert.Equal("An invalid request URI was provided. The request URI must either be an absolute URI or BaseAddress must be set.", exception.Message);
        }

        [Fact]
        public async Task PostAsMessagePackAsync_String_UsesMessagePackMediaTypeFormatter()
        {
            var response = await _client.PostAsMessagePackAsync(_uri, new object(), _options);

            var content = Assert.IsType<ObjectContent<object>>(response.RequestMessage.Content);
            var formatter = Assert.IsType<MessagePackMediaTypeFormatter>(content.Formatter);
            Assert.Same(_options, formatter.Options);
        }

        [Fact]
        public async Task PostAsMessagePackAsync_Uri_WhenClientIsNull_ThrowsException()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => ((HttpClient)null).PostAsMessagePackAsync(new Uri(_uri), new object()));
            Assert.Equal("client", exception.ParamName);
        }

        [Fact]
        public async Task PostAsMessagePackAsync_Uri_WhenOptionsIsNull_ThrowsException()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _client.PostAsMessagePackAsync(new Uri(_uri), new object(), null, CancellationToken.None));
            Assert.Equal("options", exception.ParamName);
        }

        [Fact]
        public async Task PostAsMessagePackAsync_Uri_WhenUriIsNull_ThrowsException()
        {
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _client.PostAsMessagePackAsync((Uri)null, new object()));
            Assert.Equal("An invalid request URI was provided. The request URI must either be an absolute URI or BaseAddress must be set.", exception.Message);
        }

        [Fact]
        public async Task PostAsMessagePackAsync_Uri_UsesMessagePackMediaTypeFormatter()
        {
            var response = await _client.PostAsMessagePackAsync(new Uri(_uri), new object(), _options);

            var content = Assert.IsType<ObjectContent<object>>(response.RequestMessage.Content);
            var formatter = Assert.IsType<MessagePackMediaTypeFormatter>(content.Formatter);
            Assert.Same(_options, formatter.Options);
        }
    }
}