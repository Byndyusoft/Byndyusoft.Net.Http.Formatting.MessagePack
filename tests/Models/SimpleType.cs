namespace System.Net.Http.Models
{
    using IO;
    using MessagePack;

    [MessagePackObject]
    public class SimpleType
    {
        [Key(0)]
        public int Property { get; set; }
        [Key(1)]
        public string Field;
        [Key(2)]
        public SeekOrigin Enum { get; set; }
        [Key(3)]
        public int? Nullable { get; set; }
        [Key(4)]
        public int[] Array { get; set; }
    }
}