using Microsoft.Xna.Framework;
using NinjaGame.Assets;

namespace NinjaGame.Graphics2D.Assets
{
    public class Text : BaseGraphic2D
    {
        public override bool Loaded { get { return _spriteFontAsset.Loaded; } }
        public string DrawText { get { return _drawText; } }

        public string FullText
        {
            get { return _fullText; }
            set
            {
                _fullText = value;
                _drawText = TrimText(_fullText, _dimensions);
            }
        }

        public override GraphicType GraphicType { get { return GraphicType.Text; } }
        public SpriteFontAsset SpriteFontAsset { get { return _spriteFontAsset; } }

        public Vector2 TextDimensions { get { return _spriteFontAsset.SpriteFont.MeasureString(_drawText); } }
        public override Vector2 Dimensions
        {
            get => base.Dimensions; 
            set
            {
                base.Dimensions = value;
                FullText = TrimText(FullText, Dimensions);
            }
        }

        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }


        protected string _fullText;
        protected string _drawText;
        protected Color _color;
        protected Color _disabledColor;
        protected SpriteFontAsset _spriteFontAsset;


        public Text(string id, string name, SpriteFontAsset spriteFontAsset)
            : base(id, name)
        {
            _spriteFontAsset = spriteFontAsset;
            _color = Color.White;
            _positionOffset = Vector2.Zero;
            _dimensions = Vector2.Zero;
            _fullText = "";
            _drawText = TrimText(FullText, _dimensions);
            _visible = true;
            _enabled = true;
        }

        public override IGraphic2D Copy()
        {
            var text = new Text(Id, Name, _spriteFontAsset)
            {
                Color = Color,
                PositionOffset = PositionOffset,
                Dimensions = Dimensions,
                FullText = FullText,
                Visible = Visible,
                Enabled = Enabled
            };
            return text;
        }

        public string TrimText(string text, Vector2 dimensions)
        {
            if (_spriteFontAsset.Loaded)
            {
                string ellipsis = "...";
                string workingString = string.Empty;

                for (int i = 0; i < text.Length; i++)
                {
                    Vector2 stringSize = _spriteFontAsset.SpriteFont.MeasureString(workingString + text[i] + ellipsis);
                    if (stringSize.X < dimensions.X || stringSize.Y < dimensions.Y)
                    {
                        workingString += text[i];
                    }
                    else
                    {
                        break;
                    }
                }
                if (workingString.Length != _fullText.Length)
                    workingString += ellipsis;
                return workingString;
            }
            return null;
        }

        public override void Center(Vector2 dimensions)
        {
            PositionOffset = (dimensions / 2) - (TextDimensions / 2);
        }
    }
}