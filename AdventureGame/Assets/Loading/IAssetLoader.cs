using NinjaGame.Assets.Batches;
using Microsoft.Xna.Framework.Content;
using System;

namespace NinjaGame.Assets.Loading
{
    public interface IAssetLoader
    {
        IAsset LoadAsset(string filePath, string id, ContentManager contentManager);
        IAsset LoadAssetByName(string filePath, string name, ContentManager contentManager);
        IAsset LoadAsset(AssetDefinition stagedAsset);
        IAssetBatch LoadBatch(string filePath, string id, IServiceProvider serviceProvider);
        IAssetBatch LoadBatchByName(string filePath, string name, IServiceProvider serviceProvider);
        AssetDefinition StageAsset(string filePath, string id, ContentManager contentManager);
        AssetDefinition StageAssetByName(string filePath, string name, ContentManager contentManager);
    }
}
