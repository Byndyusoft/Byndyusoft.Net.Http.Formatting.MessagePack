using System.IO;
using MessagePack;

namespace System.Net.Http.Tests.Models
{
    [MessagePackObject]
    public class SimpleType
    {
        [Key(1)] public string Field;

        [Key(0)] public int Property { get; set; }

        [Key(2)] public SeekOrigin Enum { get; set; }

        [Key(3)] public int? Nullable { get; set; }

        [Key(4)] public int[] Array { get; set; }
    }
}