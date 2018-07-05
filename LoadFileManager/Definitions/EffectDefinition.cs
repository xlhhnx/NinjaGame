using Microsoft.Xna.Framework;
using LoadFileManager.Definitions;

namespace LoadFileManager.Graphics2D.Definitions
{
    public class EffectDefinition : Definition<GraphicType>
    {
        public EffectDefinition(string id, string name, string assetFile, GraphicType type) 
            : base(id, name, assetFile, type)
        {
        }
    }
}
