using NinjaGame.Assets;
using NinjaGame.Common.Extensions;
using Microsoft.Xna.Framework;

namespace NinjaGame.Graphics2D.Assets
{
    public class Image : BaseGraphic2D
    {
        public override bool Loaded { get { return _texture2DAsset.Loaded; } }
        public override GraphicType GraphicType { get { return GraphicType.Image; } }
        public Texture2DAsset Texture2DAsset { get { return _texture2DAsset; } }
        public Rectangle SourceRectangle { get { return _sourceRectangle; } }
        public Color Color { get { return _color; } }


        protected Texture2DAsset _texture2DAsset;
        protected Rectangle _sourceRectangle;
        protected Color _color;


        public Image(Texture2DAsset texture2DAsset, Vector2 sourcePosition, Vector2 sourceDimensions, Color color, Vector2 positionOffset, Vector2 dimsensions, bool enabled = true, bool visible = true)
        {
            _texture2DAsset = texture2DAsset;
            _sourceRectangle = new Rectangle(sourcePosition.ToPoint(), sourceDimensions.ToPoint());
            _positionOffset = positionOffset;
            _dimensions = dimsensions;
            _color = color;
            _enabled = enabled;
            _visible = visible;
        }

        public override IGraphic2D Copy()
        {
            return new Image(_texture2DAsset, _sourceRectangle.GetPosition(), _sourceRectangle.GetDimensions(), _color, _positionOffset, _dimensions, _enabled, _visible);
        }
    }
}