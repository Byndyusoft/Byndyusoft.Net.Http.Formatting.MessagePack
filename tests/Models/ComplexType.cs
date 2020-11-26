using MessagePack;

namespace System.Net.Http.Formatting.Models
{
    [MessagePackObject]
    public class ComplexType
    {
        [Key(0)] public SimpleType Inner { get; set; }
    }
}
