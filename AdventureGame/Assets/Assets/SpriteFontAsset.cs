using Microsoft.Xna.Framework.Graphics;

namespace NinjaGame.Assets
{
    public class SpriteFontAsset : BaseAsset
    {
        public virtual SpriteFont SpriteFont { get { return _spriteFont; } }
        public override bool Loaded { get { return _loaded; } }


        protected bool _loaded;
        protected SpriteFont _spriteFont;


        public SpriteFontAsset(string assetId, string name, SpriteFont spriteFont)
            : base(assetId, name)
        {
            _spriteFont = spriteFont;
            _loaded = _spriteFont != null;
            _type = AssetType.SpriteFontAsset;
        }

        public override void Unload()
        {
            _loaded = false;
        }
    }
}