
namespace Byndyusoft.Net.Http.Formatting.MessagePack.Example.Models
{
    using System;
    using global::MessagePack;

    [MessagePackObject]
    public class People
    {
        [Key(0)]
        public int Id { get; set; }
        [Key(1)]
        public string Name { get; set; }
        [Key(2)]
        public DateTime DateOfBirth { get; set; }
    }
}