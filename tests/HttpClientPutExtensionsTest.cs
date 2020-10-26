﻿namespace System.Net.Http
{
    using Formatting;
    using MessagePack;
    using Server;
    using Threading;
    using Threading.Tasks;
    using Xunit;

    public class HttpClientPutExtensionsTest : IDisposable
    {
        private readonly HttpClient _client;
        private readonly HttpServer _server;
        private readonly string _uri;
        private readonly MessagePackSerializerOptions _options;

        public HttpClientPutExtensionsTest()
        {
            _options = MessagePackSerializerOptions.Standard;
            _client = new HttpClient();
            _server = new HttpServer();
            _uri = _server.Start();
        }

        public void Dispose()
        {
            _server.Dispose();
        }

        [Fact]
        public async Task PutAsMessagePackAsync_String_WhenClientIsNull_ThrowsException()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => ((HttpClient)null).PutAsMessagePackAsync(_uri, new object()));
            Assert.Equal("client", exception.ParamName);
        }

        [Fact]
        public async Task PutAsMessagePackAsync_String_WhenUriIsNull_ThrowsException()
        {
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _client.PutAsMessagePackAsync((string)null, new object()));
            Assert.Equal("An invalid request URI was provided. The request URI must either be an absolute URI or BaseAddress must be set.", exception.Message);
        }

        [Fact]
        public async Task PutAsMessagePackAsync_String_WhenOptionsIsNull_ThrowsException()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _client.PutAsMessagePackAsync(_uri, new object(), null, CancellationToken.None));
            Assert.Equal("options", exception.ParamName);
        }

        [Fact]
        public async Task PutAsMessagePackAsync_String_UsesMessagePackMediaTypeFormatter()
        {
            var response = await _client.PutAsMessagePackAsync(_uri, new object(), _options);

            var content = Assert.IsType<ObjectContent<object>>(response.RequestMessage.Content);
            var formatter = Assert.IsType<MessagePackMediaTypeFormatter>(content.Formatter);
            Assert.Same(_options, formatter.Options);
        }

        [Fact]
        public async Task PutAsMessagePackAsync_Uri_WhenClientIsNull_ThrowsException()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => ((HttpClient)null).PutAsMessagePackAsync(new Uri(_uri), new object()));
            Assert.Equal("client", exception.ParamName);
        }

        [Fact]
        public async Task PutAsMessagePackAsync_Uri_WhenOptionsIsNull_ThrowsException()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _client.PutAsMessagePackAsync(new Uri(_uri), new object(), null, CancellationToken.None));
            Assert.Equal("options", exception.ParamName);
        }

        [Fact]
        public async Task PutAsMessagePackAsync_Uri_WhenUriIsNull_ThrowsException()
        {
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _client.PutAsMessagePackAsync((Uri)null, new object()));
            Assert.Equal("An invalid request URI was provided. The request URI must either be an absolute URI or BaseAddress must be set.", exception.Message);
        }

        [Fact]
        public async Task PutAsMessagePackAsync_Uri_UsesMessagePackMediaTypeFormatter()
        {
            var response = await _client.PutAsMessagePackAsync(new Uri(_uri), new object(), _options);

            var content = Assert.IsType<ObjectContent<object>>(response.RequestMessage.Content);
            var formatter = Assert.IsType<MessagePackMediaTypeFormatter>(content.Formatter);
            Assert.Same(_options, formatter.Options);
        }
    }
}