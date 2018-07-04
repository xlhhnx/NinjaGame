using System;

namespace NinjaGame.Common.Loading
{
    public interface IDefinition<T> : IEquatable<IDefinition<T>>
    {
        string Id { get; }
        string Name { get; }
        string FilePath { get; }
        T Type { get; }
    }
}
