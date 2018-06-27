using NinjaGame.Assets.Batches;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;

namespace NinjaGame.Assets.Loading
{
    public interface IAssetLoader
    {
        IAsset LoadAsset(string filePath, string id, ContentManager contentManager);
        IAsset LoadAsset(StagedAsset stagedAsset);
        IAssetBatch LoadBatch(string filePath, string id, IServiceProvider serviceProvider);
        StagedAsset StageAsset(string filePath, string id, ContentManager contentManager);
    }
}
