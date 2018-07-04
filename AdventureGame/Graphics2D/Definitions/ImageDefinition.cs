using Microsoft.Xna.Framework;
using NinjaGame.Loading.Definitions;

namespace NinjaGame.Graphics2D.Definitions
{
    public class ImageDefinition : Definition<GraphicType>
    {
        public string Texture2DId { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        public Color Color { get; set; }
        public Vector2 Dimensions { get; set; }
        public Vector2 SourcePosition { get; set; }
        public Vector2 SourceDimensions { get; set; }

        public ImageDefinition(string id, string name, string assetFile, string texture2DId, GraphicType type, Vector2 sourcePosition, Vector2 sourceDimensions) 
            : base(id, name, assetFile, type)
        {
            Texture2DId = texture2DId;
            SourcePosition = sourcePosition;
            SourceDimensions = sourceDimensions;

            Rows = 1;
            Columns = 1;
        }
    }
}
