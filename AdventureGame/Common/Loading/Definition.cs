using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaGame.Common.Loading
{
    public class Definition<T> : IDefinition<T>
    {
        public string Id { get; protected set; }
        public string Name { get; protected set; }
        public string FilePath { get; protected set; }
        public T Type { get; protected set; }

        public Definition(string id, string name, string filePath, T type)
        {
            Id = id;
            Name = name;
            FilePath = filePath;
            Type = type;
        }
    }
}
