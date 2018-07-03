using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaGame.Common
{
    public class LoadBatch<T> : ILoadBatch<T>
    {
        public virtual string Id { get; private set; }
        public virtual List<T> Values { get; set; }
        public virtual Dictionary<string, List<string>> FileIdDict { get; set; }

        public LoadBatch(string id)
        {
            Id = id;
            Values = new List<T>();
            FileIdDict = new Dictionary<string, List<string>>();
        }

        public virtual void AddDefinition(string filePath, string id)
        {
            if (!FileIdDict.ContainsKey(filePath))
                FileIdDict.Add(filePath, new List<string>());

            if (FileIdDict[filePath] is null)
                FileIdDict[filePath] = new List<string>();

            FileIdDict[filePath].Add(id);
        }

        public virtual void AddValue(T value)
        {
            if (!Values.Contains(value))
                Values.Add(value);
        }

        public virtual List<Tuple<string, string>> GetAllFileIdPairs()
        {
            var pairs = new List<Tuple<string, string>>();
            foreach (var file in FileIdDict.Keys)
            {
                foreach (var id in FileIdDict[file])
                    pairs.Add(new Tuple<string, string>(file, id));
            }
            return pairs;
        }

        public virtual void Unload()
        {
            Values = new List<T>();
        }
    }
}
