using Microsoft.Xna.Framework;
using NinjaGame.Graphics2D.Config;
using System;

namespace NinjaGame.Graphics2D.Definitions
{
    public class SpriteDefinition : ImageDefinition
    {
        public bool Looping { get; set; }
        public int FrameTime { get; set; }

        public SpriteDefinition(string id, string name, string assetFile, string texture2DId, GraphicType type, Vector2 sourcePosition, Vector2 sourceDimensions) 
            : base(id, name, assetFile, texture2DId, type, sourcePosition, sourceDimensions)
        {
            Looping = true;
            FrameTime = Graphics2DConfig.DefaultFrameTime;
        }
    }
}
