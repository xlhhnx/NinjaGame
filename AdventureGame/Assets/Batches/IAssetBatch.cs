using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;

namespace NinjaGame.Assets.Batches
{
    public interface IAssetBatch
    {
        string Id { get; }
        ContentManager Content { get; }
        List<IAsset> Assets { get; set; }
        Dictionary<string, List<string>> FileIdDict { get; set; }

        void AddAssetDefinition(string filePath, string assetId);
        void AddAsset(IAsset asset);
        List<Tuple<string, string>> GetAllFileIdPairs();
        void Unload();
    }
}