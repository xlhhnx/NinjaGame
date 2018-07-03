using Microsoft.Xna.Framework.Content;
using NinjaGame.Common;
using System;
using System.Collections.Generic;

namespace NinjaGame.Assets.Batches
{
    public class AssetBatch : LoadBatch<IAsset>, IAssetBatch
    {
        public ContentManager Content { get; private set; }

        public AssetBatch(string id, IServiceProvider serviceProvider)
            : base(id)
        {
            Content = new ContentManager(serviceProvider);
        }

        public AssetBatch(string id, IServiceProvider serviceProvider, string rootDirectory)
            : base(id)
        {
            Content = new ContentManager(serviceProvider, rootDirectory);
        }
    }
}