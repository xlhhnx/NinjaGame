using Microsoft.Xna.Framework;
using NinjaGame.Loading.Definitions;

namespace NinjaGame.Graphics2D.Definitions
{
    public class EffectDefinition : Definition<GraphicType>
    {
        public EffectDefinition(string id, string name, string assetFile, GraphicType type) 
            : base(id, name, assetFile, type)
        {
        }
    }
}
