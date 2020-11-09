using System.ComponentModel;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using MessagePack;

namespace System.Net.Http
{
    /// <summary>
    ///     Extension methods that aid in making formatted PUT requests using <see cref="HttpClient" />.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class HttpClientPutExtensions
    {
        /// <summary>
        ///     Sends a Put request as an asynchronous operation to the specified Uri with the given <paramref name="value" />
        ///     serialized as MessagePack.
        /// </summary>
        /// <remarks>
        ///     This method uses a default instance of <see cref="MessagePackMediaTypeFormatter" />.
        /// </remarks>
        /// <typeparam name="T">The type of <paramref name="client" />.</typeparam>
        /// <param name="client">The client used to make the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="value">The value that will be placed in the request's entity body.</param>
        /// <returns>A task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PutAsMessagePackAsync<T>(
            this HttpClient client,
            string requestUri,
            T value)
        {
            return client.PutAsMessagePackAsync(requestUri, value, CancellationToken.None);
        }

        /// <summary>
        ///     Sends a PUT request as an asynchronous operation to the specified Uri with the given <paramref name="value" />
        ///     serialized as MessagePack.
        /// </summary>
        /// <remarks>
        ///     This method uses a default instance of <see cref="MessagePackMediaTypeFormatter" />.
        /// </remarks>
        /// <typeparam name="T">The type of <paramref name="client" />.</typeparam>
        /// <param name="client">The client used to make the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="value">The value that will be placed in the request's entity body.</param>
        /// <param name="options">Options for running the serialization.</param>
        /// <returns>A task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PutAsMessagePackAsync<T>(
            this HttpClient client,
            string requestUri,
            T value,
            MessagePackSerializerOptions options)
        {
            return client.PutAsMessagePackAsync(requestUri, value, options, CancellationToken.None);
        }

        /// <summary>
        ///     Sends a PUT request as an asynchronous operation to the specified Uri with the given <paramref name="value" />
        ///     serialized as MessagePack.
        /// </summary>
        /// <remarks>
        ///     This method uses a default instance of <see cref="MessagePackMediaTypeFormatter" />.
        /// </remarks>
        /// <typeparam name="T">The type of <paramref name="value" />.</typeparam>
        /// <param name="client">The client used to make the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="value">The value that will be placed in the request's entity body.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PutAsMessagePackAsync<T>(
            this HttpClient client,
            string requestUri,
            T value,
            CancellationToken cancellationToken)
        {
            return client.PutAsMessagePackAsync(requestUri, value, MessagePackSerializerOptions.Standard,
                cancellationToken);
        }

        /// <summary>
        ///     Sends a PUT request as an asynchronous operation to the specified Uri with the given <paramref name="value" />
        ///     serialized as MessagePack.
        /// </summary>
        /// <remarks>
        ///     This method uses a default instance of <see cref="MessagePackMediaTypeFormatter" />.
        /// </remarks>
        /// <typeparam name="T">The type of <paramref name="value" />.</typeparam>
        /// <param name="client">The client used to make the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="value">The value that will be placed in the request's entity body.</param>
        /// <param name="options">Options for running the serialization.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PutAsMessagePackAsync<T>(
            this HttpClient client,
            string requestUri,
            T value,
            MessagePackSerializerOptions options,
            CancellationToken cancellationToken)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            return client.PutAsync(requestUri, value, new MessagePackMediaTypeFormatter(options), cancellationToken);
        }

        /// <summary>
        ///     Sends a PUT request as an asynchronous operation to the specified Uri with the given <paramref name="value" />
        ///     serialized as MessagePack.
        /// </summary>
        /// <remarks>
        ///     This method uses a default instance of <see cref="MessagePackMediaTypeFormatter" />.
        /// </remarks>
        /// <typeparam name="T">The type of <paramref name="value" />.</typeparam>
        /// <param name="client">The client used to make the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="value">The value that will be placed in the request's entity body.</param>
        /// <returns>A task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PutAsMessagePackAsync<T>(
            this HttpClient client,
            Uri requestUri,
            T value)
        {
            return client.PutAsMessagePackAsync(requestUri, value, CancellationToken.None);
        }

        /// <summary>
        ///     Sends a PUT request as an asynchronous operation to the specified Uri with the given <paramref name="value" />
        ///     serialized as MessagePack.
        /// </summary>
        /// <remarks>
        ///     This method uses a default instance of <see cref="MessagePackMediaTypeFormatter" />.
        /// </remarks>
        /// <typeparam name="T">The type of <paramref name="value" />.</typeparam>
        /// <param name="client">The client used to make the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="value">The value that will be placed in the request's entity body.</param>
        /// <param name="options">Options for running the serialization.</param>
        /// <returns>A task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PutAsMessagePackAsync<T>(
            this HttpClient client,
            Uri requestUri,
            T value,
            MessagePackSerializerOptions options)
        {
            return client.PutAsMessagePackAsync(requestUri, value, options, CancellationToken.None);
        }

        /// <summary>
        ///     Sends a PUT request as an asynchronous operation to the specified Uri with the given <paramref name="value" />
        ///     serializeds as MessagePack.
        /// </summary>
        /// <remarks>
        ///     This method uses a default instance of <see cref="MessagePackMediaTypeFormatter" />.
        /// </remarks>
        /// <typeparam name="T">The type of <paramref name="value" />.</typeparam>
        /// <param name="client">The client used to make the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="value">The value that will be placed in the request's entity body.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PutAsMessagePackAsync<T>(
            this HttpClient client,
            Uri requestUri,
            T value,
            CancellationToken cancellationToken)
        {
            return client.PutAsMessagePackAsync(requestUri, value, MessagePackSerializerOptions.Standard,
                cancellationToken);
        }

        /// <summary>
        ///     Sends a PUT request as an asynchronous operation to the specified Uri with the given <paramref name="value" />
        ///     serialized as MessagePack.
        /// </summary>
        /// <remarks>
        ///     This method uses a default instance of <see cref="MessagePackMediaTypeFormatter" />.
        /// </remarks>
        /// <typeparam name="T">The type of <paramref name="value" />.</typeparam>
        /// <param name="client">The client used to make the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="value">The value that will be placed in the request's entity body.</param>
        /// <param name="options">Options for running the serialization.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PutAsMessagePackAsync<T>(
            this HttpClient client,
            Uri requestUri,
            T value,
            MessagePackSerializerOptions options,
            CancellationToken cancellationToken)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            return client.PostAsync(requestUri, value, new MessagePackMediaTypeFormatter(options), cancellationToken);
        }
    }
}