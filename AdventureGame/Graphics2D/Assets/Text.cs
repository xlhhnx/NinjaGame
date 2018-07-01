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

        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        public Color DisabledColor
        {
            get { return _disabledColor; }
            set { _disabledColor = value; }
        }


        protected string _fullText;
        protected string _drawText;
        protected Color _color;
        protected Color _disabledColor;
        protected SpriteFontAsset _spriteFontAsset;


        public Text(string id, SpriteFontAsset spriteFontAsset, Color color, Color disabledColor, Vector2 positionOffset, Vector2 dimensions, string fullText, bool visible = true, bool enabled = true)
            : base(id)
        {
            _spriteFontAsset = spriteFontAsset;
            _color = color;
            _disabledColor = disabledColor;
            _positionOffset = positionOffset;
            _dimensions = dimensions;
            _fullText = fullText;
            _drawText = TrimText(fullText, _dimensions);
            _visible = true;
            _enabled = true;
        }

        public override IGraphic2D Copy()
        {
            return new Text(Id, _spriteFontAsset, _color, _disabledColor, _positionOffset, _dimensions, _fullText);
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

        public override void Center()
        {
            PositionOffset = -(TextDimensions / 2);
        }
    }
}