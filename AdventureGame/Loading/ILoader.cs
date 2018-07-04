using NinjaGame.Assets;
using NinjaGame.Batches.Loading;
using System.Collections.Generic;

namespace NinjaGame.Loading
{
    public interface ILoader<T, Type>
    {
        List<ILoadBatch<T>> LoadBatches(string filePath);
        List<IAsset> LoadAssets(ILoadBatch<T> batch);
    }
}
