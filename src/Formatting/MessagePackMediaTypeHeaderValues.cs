using System.Net.Http.Headers;

namespace System.Net.Http.Formatting
{
    public static class MessagePackMediaTypeHeaderValues
    {
        public static readonly MediaTypeWithQualityHeaderValue ApplicationMessagePack =
            new MediaTypeWithQualityHeaderValue(MessagePackMediaTypes.ApplicationMessagePack);

        public static readonly MediaTypeWithQualityHeaderValue ApplicationXMessagePack =
            new MediaTypeWithQualityHeaderValue(MessagePackMediaTypes.ApplicationXMessagePack);
    }
}