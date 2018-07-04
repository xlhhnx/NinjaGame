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
        public Color Color { get { return _color; } set { _color = value; } }


        protected Texture2DAsset _texture2DAsset;
        protected Rectangle _sourceRectangle;
        protected Color _color;

        public Image(string id, string name, Texture2DAsset texture2DAsset, Vector2 sourcePosition, Vector2 sourceDimensions)
            : base(id, name)
        {
            _texture2DAsset = texture2DAsset;
            _sourceRectangle = new Rectangle(sourcePosition.ToPoint(), sourceDimensions.ToPoint());
            _positionOffset = Vector2.Zero;
            _dimensions = Vector2.Zero;
            _color = Color.White;
            _enabled = true;
            _visible = true;
        }

        public override IGraphic2D Copy()
        {
            var image = new Image(Id, Name, _texture2DAsset, _sourceRectangle.GetPosition(), _sourceRectangle.GetDimensions())
            {
                PositionOffset = PositionOffset,
                Dimensions = Dimensions,
                Color = Color,
                Enabled = Enabled,
                Visible = Visible
            };
            return image;
        }
    }
}