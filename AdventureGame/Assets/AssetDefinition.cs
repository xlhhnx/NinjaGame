using Microsoft.Xna.Framework.Content;
using NinjaGame.Common.Loading;

namespace NinjaGame.Assets
{
    public class AssetDefinition : Definition<AssetType>
    {
        public ContentManager Content { get; protected set; }
     
        public AssetDefinition(string id, string name, string filePath, AssetType type, ContentManager content) 
            : base(id, name, filePath, type)
        {
            Content = content;
        }   
    }
}
