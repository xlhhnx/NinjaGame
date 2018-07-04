using NinjaGame.Assets.Batches;
using NinjaGame.Loading;
using System;
using System.Collections.Generic;

namespace NinjaGame.Assets.Loading
{
    public interface IAssetLoader
    {
        string RootDirectory { get; set; }
        IServiceProvider ServiceProvider { get; set; }
        
        List<IAssetBatch> LoadBatches(string filePath);
        List<IAsset> LoadAssets(IAssetBatch batch);
    }
}
