namespace NinjaGame.Assets.Config
{
    public static class AssetConfig
    {
        public static SpriteFontAsset DefaultSpriteFontAsset
        {
            get { return _defaultSpriteFontAsset; }
            set { _defaultSpriteFontAsset = value; }
        }

        public static Texture2DAsset DefaultTexture2DAsset
        {
            get { return _defaultTexture2DAsset; }
            set { _defaultTexture2DAsset = value; }
        }

        public static AudioAsset DefaultAudioAsset
        {
            get { return _defaultAudioAsset; }
            set { _defaultAudioAsset = value; }
        }

        private static SpriteFontAsset _defaultSpriteFontAsset = null;
        private static Texture2DAsset _defaultTexture2DAsset = null;
        private static AudioAsset _defaultAudioAsset = null;
    }
}