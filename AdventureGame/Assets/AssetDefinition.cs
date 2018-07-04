using Microsoft.Xna.Framework.Content;
using NinjaGame.Loading.Definitions;

namespace NinjaGame.Assets
{
    public class AssetDefinition : Definition<AssetType>
    {
        public ContentManager Content { get; protected set; }
     
        public AssetDefinition(string id, string name, string assetFile, AssetType type, ContentManager content) 
            :base(id, name, assetFile, type)
        {
            Content = content;
        }   
    }
}
