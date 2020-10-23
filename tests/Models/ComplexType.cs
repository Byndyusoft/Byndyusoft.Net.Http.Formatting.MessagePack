namespace System.Net.Http.Models
{
    using MessagePack;

    [MessagePackObject]
    public class ComplexType
    {
        [Key(0)]
        public SimpleType Inner { get; set; }
    }
}