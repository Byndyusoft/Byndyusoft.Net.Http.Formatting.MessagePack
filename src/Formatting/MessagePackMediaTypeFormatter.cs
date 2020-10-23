namespace System.Net.Http.Formatting
{
    using System;
    using Http;
    using IO;
    using Net;
    using MessagePack;
    using Threading;
    using Threading.Tasks;
    using Headers;


    /// <summary>
    /// <see cref="MediaTypeFormatter"/> class to handle MessagePack.
    /// </summary>
    public class MessagePackMediaTypeFormatter : MediaTypeFormatter
    {
        public static readonly MediaTypeWithQualityHeaderValue DefaultMediaTypeHeaderValue = MessagePackMediaTypeHeaderValues.ApplicationXMessagePack;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagePackMediaTypeFormatter"/> class.
        /// </summary>
        public MessagePackMediaTypeFormatter() : this(MessagePackSerializer.DefaultOptions)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagePackMediaTypeFormatter"/> class.
        /// </summary>
        /// <param name="formatter">The <see cref="MessagePackMediaTypeFormatter"/> instance to copy settings from.</param>
        protected internal MessagePackMediaTypeFormatter(MessagePackMediaTypeFormatter formatter)
            : base(formatter)
        {
            Options = formatter.Options;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagePackMediaTypeFormatter"/> class.
        /// </summary>
        /// <param name="options">Options for running serialization.</param>
        public MessagePackMediaTypeFormatter(MessagePackSerializerOptions options)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
            SupportedMediaTypes.Add(MessagePackMediaTypeHeaderValues.ApplicationXMessagePack);
            SupportedMediaTypes.Add(MessagePackMediaTypeHeaderValues.ApplicationMessagePack);
        }

        /// <summary>
        /// Options for running the serialization.
        /// </summary>
        public MessagePackSerializerOptions Options { get; }

        /// <inheritdoc />
        public override async Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger,
            CancellationToken cancellationToken)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));
            if (readStream is null) throw new ArgumentNullException(nameof(readStream));

            if (readStream.Length == 0)
            {
                return null;
            }

            return await MessagePackSerializer.DeserializeAsync(type, readStream, Options, cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            return ReadFromStreamAsync(type, readStream, content, formatterLogger, CancellationToken.None);
        }

        /// <inheritdoc />
        public override async Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext,
            CancellationToken cancellationToken)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));
            if (writeStream is null) throw new ArgumentNullException(nameof(writeStream));

            if (value is null)
            {
                return;
            }

            await MessagePackSerializer.SerializeAsync(type, writeStream, value, Options, cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
        {
            return WriteToStreamAsync(type, value, writeStream, content, transportContext, CancellationToken.None);
        }

        /// <inheritdoc />
        public override bool CanReadType(Type type) => CanSerialize(type);

        /// <inheritdoc />
        public override bool CanWriteType(Type type) => CanSerialize(type);

        private bool CanSerialize(Type type)
        {
            return !type.IsAbstract && !type.IsInterface && type.IsPublic;
        }
    }
}
