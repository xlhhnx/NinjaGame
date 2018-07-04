using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaGame.Common.Loading
{
    public interface IDefinition<T>
    {
        string Id { get; }
        string Name { get; }
        string FilePath { get; }
        T Type { get; }
    }
}
