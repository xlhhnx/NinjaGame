using System;
using System.Collections.Generic;

namespace NinjaGame.Batches.Loading
{
    public interface ILoadBatch<T>
    {
        string Id { get; }
        string Name { get; }
        List<T> Values { get; set; }
        List<string> Files { get; set; }
        
        void Unload();
    }
}
