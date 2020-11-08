using System.Net.Http.Headers;
using MessagePack;
using MessagePack.Resolvers;

namespace System.Net.Http.Formatting.MessagePack
{
    public static class MessagePackConstants
    {
        public static readonly MediaTypeWithQualityHeaderValue DefaultMediaTypeHeader =
            MediaTypeHeaders.ApplicationXMessagePack;

        public static readonly MessagePackSerializerOptions DefaultSerializerOptions =
            MessagePackSerializerOptions.Standard
                .WithResolver(ContractlessStandardResolverAllowPrivate.Instance);

        public static class MediaTypes
        {
            public const string ApplicationMessagePack = "application/msgpack";
            public const string ApplicationXMessagePack = "application/x-msgpack";
        }

        public static class MediaTypeHeaders
        {
            public static readonly MediaTypeWithQualityHeaderValue ApplicationMessagePack =
                new MediaTypeWithQualityHeaderValue(MediaTypes.ApplicationMessagePack);

            public static readonly MediaTypeWithQualityHeaderValue ApplicationXMessagePack =
                new MediaTypeWithQualityHeaderValue(MediaTypes.ApplicationXMessagePack);
        }
    }
}