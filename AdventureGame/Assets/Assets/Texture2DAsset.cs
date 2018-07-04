using Microsoft.Xna.Framework.Graphics;

namespace NinjaGame.Assets
{
    public class Texture2DAsset : BaseAsset
    {
        public virtual Texture2D Texture { get { return _texture; } }
        public override bool Loaded { get { return _loaded; } }


        protected bool _loaded;
        protected Texture2D _texture;


        public Texture2DAsset(string assetId, string name, Texture2D texture)
            : base(assetId, name)
        {
            _texture = texture;
            _loaded = texture != null;
            _type = AssetType.Texture2DAsset;
        }

        public override void Unload()
        {
            _loaded = false;
        }
    }
}