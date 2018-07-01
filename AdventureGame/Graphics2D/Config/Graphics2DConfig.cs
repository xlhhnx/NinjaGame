using NinjaGame.Graphics2D.Assets;

namespace NinjaGame.Graphics2D.Config
{
    public static class Graphics2DConfig
    {
        public static int DefaultFrameTime
        {
            get { return _defaultFrameTime; }
            set { _defaultFrameTime = value; }
        }

        public static Image DefaultImage
        {
            get { return _defaulImage; }
            set { _defaulImage = value; }
        }

        public static Sprite DefaultSprite
        {
            get { return _defaultSprite; }
            set { _defaultSprite = value; }
        }

        public static Text DefaultText
        {
            get { return _defaultText; }
            set { _defaultText = value; }
        }

        private static int _defaultFrameTime = 200;
        private static Image _defaulImage = null;
        private static Sprite _defaultSprite = null;
        private static Text _defaultText = null;
    }
}