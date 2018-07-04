using Microsoft.Xna.Framework.Content;
using NinjaGame.Batches.Loading;
using System;

namespace NinjaGame.Assets.Batches
{
    public class AssetBatch : LoadBatch<IAsset>, IAssetBatch
    {
        public ContentManager Content { get; private set; }

        public AssetBatch(string id, string name, IServiceProvider serviceProvider)
            : base(id, name)
        {
            Content = new ContentManager(serviceProvider);
        }

        public AssetBatch(string id, string name, IServiceProvider serviceProvider, string rootDirectory)
            : base(id, name)
        {
            Content = new ContentManager(serviceProvider, rootDirectory);
        }
    }
}