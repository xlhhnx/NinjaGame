using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;

namespace NinjaGame.Assets.Batches
{
    public interface IAssetBatch
    {
        string Id { get; }
        ContentManager Content { get; }
        Dictionary<string, List<string>> FileIdDictionary { get; set; }
        List<IAsset> Assets { get; set; }

        void AddAssetDefinition(string filePath, string assetId);
        void AddAsset(IAsset asset);
        List<Tuple<string, string>> GetAllFileIdPairs();
        void Unload();
    }
}