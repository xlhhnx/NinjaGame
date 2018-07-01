using NinjaGame.Assets;
using Microsoft.Xna.Framework;

namespace NinjaGame.Graphics2D.Assets
{
    public interface IGraphic2D : IAsset
    {
        bool Visible { get; }
        bool Enabled { get; }
        GraphicType GraphicType { get; }
        Vector2 PositionOffset { get; set; }
        Vector2 Dimensions { get; set; }

        IGraphic2D Copy();
        void Center(Vector2 dimensions);
    }
}