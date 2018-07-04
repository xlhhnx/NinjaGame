using Microsoft.Xna.Framework;
using NinjaGame.Loading.Definitions;

namespace NinjaGame.Graphics2D.Definitions
{
    public class TextDefinition : Definition<GraphicType>
    {
        public string SpriteFontId { get; set; }
        public string FullText { get; set; }
        public Color Color { get; set; }
        public Vector2 Dimensions { get; set; }

        public TextDefinition(string id, string name, string assetFile, string spriteFontId, string fullText, GraphicType type) 
            : base(id, name, assetFile, type)
        {
            SpriteFontId = spriteFontId;
            FullText = fullText;
        }
    }
}
