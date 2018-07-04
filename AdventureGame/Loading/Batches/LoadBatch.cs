using System;
using System.Collections.Generic;

namespace NinjaGame.Batches.Loading
{
    public class LoadBatch<T> : ILoadBatch<T>
    {
        public string Id { get; }
        public string Name { get; }
        public List<T> Values { get; set; }
        public List<string> Files { get; set; }

        public LoadBatch(string id, string name)
        {
            Id = id;
            Name = name;
            Values = new List<T>();
            Files = new List<string>();
        }

        public void Unload()
        {
            Values = new List<T>();
        }
    }
}
