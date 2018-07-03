using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaGame.Common
{
    public interface ILoadBatch<T>
    {
        string Id { get; }
        List<T> Values { get; set; }
        Dictionary<string, List<string>> FileIdDict { get; set; }

        void AddDefinition(string filePath, string id);
        void AddValue(T value);
        List<Tuple<string, string>> GetAllFileIdPairs();
        void Unload();
    }
}
