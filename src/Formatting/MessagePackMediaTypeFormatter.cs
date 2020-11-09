﻿using System.IO;
using System.Net.Http.Formatting.MessagePack;
using System.Threading;
using System.Threading.Tasks;
using MessagePack;

namespace System.Net.Http.Formatting
{
    /// <summary>
    ///     <see cref="MediaTypeFormatter" /> class to handle MessagePack.
    /// </summary>
    public class MessagePackMediaTypeFormatter : MediaTypeFormatter
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="MessagePackMediaTypeFormatter" /> class.
        /// </summary>
        public MessagePackMediaTypeFormatter() : this(MessagePackConstants.DefaultSerializerOptions)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MessagePackMediaTypeFormatter" /> class.
        /// </summary>
        /// <param name="formatter">The <see cref="MessagePackMediaTypeFormatter" /> instance to copy settings from.</param>
        protected internal MessagePackMediaTypeFormatter(MessagePackMediaTypeFormatter formatter)
            : base(formatter)
        {
            Options = formatter.Options;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MessagePackMediaTypeFormatter" /> class.
        /// </summary>
        /// <param name="options">Options for running serialization.</param>
        public MessagePackMediaTypeFormatter(MessagePackSerializerOptions options)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
            SupportedMediaTypes.Add(MessagePackConstants.MediaTypeHeaders.ApplicationXMessagePack);
            SupportedMediaTypes.Add(MessagePackConstants.MediaTypeHeaders.ApplicationMessagePack);
        }

        /// <summary>
        ///     Options for running the serialization.
        /// </summary>
        public MessagePackSerializerOptions Options { get; }

        /// <inheritdoc />
        public override async Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content,
            IFormatterLogger formatterLogger, CancellationToken cancellationToken)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));
            if (readStream is null) throw new ArgumentNullException(nameof(readStream));

            if (readStream.Length == 0) return null;

            var memoryStrem = new MemoryStream();
            await readStream.CopyToAsync(memoryStrem).ConfigureAwait(false);

            if (memoryStrem.Length == 0) return null;

            memoryStrem.Position = 0;
            return await MessagePackSerializer.DeserializeAsync(type, memoryStrem, Options, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public override async Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content,
            IFormatterLogger formatterLogger)
        {
            return await ReadFromStreamAsync(type, readStream, content, formatterLogger, CancellationToken.None);
        }

        /// <inheritdoc />
        public override async Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content,
            TransportContext transportContext,
            CancellationToken cancellationToken)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));
            if (writeStream is null) throw new ArgumentNullException(nameof(writeStream));

            if (value is null) return;

            await MessagePackSerializer.SerializeAsync(type, writeStream, value, Options, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public override async Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content,
            TransportContext transportContext)
        {
            await WriteToStreamAsync(type, value, writeStream, content, transportContext, CancellationToken.None);
        }

        /// <inheritdoc />
        public override bool CanReadType(Type type)
        {
            return CanSerialize(type);
        }

        /// <inheritdoc />
        public override bool CanWriteType(Type type)
        {
            return CanSerialize(type);
        }

        private bool CanSerialize(Type type)
        {
            return !type.IsAbstract && !type.IsInterface;
        }
    }
}