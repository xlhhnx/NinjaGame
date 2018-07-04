using Microsoft.Xna.Framework.Content;
using NinjaGame.Common.Loading;

namespace NinjaGame.Assets.Batches
{
    public interface IAssetBatch : ILoadBatch<IAsset>
    {
        ContentManager Content { get; }
    }
}
