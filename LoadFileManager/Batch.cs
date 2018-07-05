using System.Collections.Generic;

namespace LoadFileManager
{
    public class Batch
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public BatchType Type { get; set; }
        public List<string> Files { get; set; }

        public Batch()
        {
            Id = "";
            Name = "";
            Files = new List<string>();
        }
    }
}
