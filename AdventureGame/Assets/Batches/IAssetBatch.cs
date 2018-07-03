using Microsoft.Xna.Framework.Content;
using NinjaGame.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaGame.Assets.Batches
{
    public interface IAssetBatch : ILoadBatch<IAsset>
    {
        ContentManager Content { get; }
    }
}
