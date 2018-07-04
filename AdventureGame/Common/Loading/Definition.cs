using System;

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

        public bool Equals(IDefinition<T> other)
        {
            var equal = FilePath == other.FilePath;

            if (equal && Id != null)
                equal = Id == other.Id;
            else if (equal)
                equal = Name == other.Name;

            return equal;
        }
    }
}
