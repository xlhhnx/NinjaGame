using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NinjaGame;
using NinjaGame.Graphics2D.Assets;
using NinjaGame.Graphics2D.Extensions;
using NinjaGame.Scenes;

namespace AdventureGame.Menus
{
    public class MainMenu : IScene
    {
        protected Image _exitButtonImage;
        protected SpriteBatch _spriteBatch;

        public MainMenu()
        {
            _spriteBatch = new SpriteBatch(MainGame.Instance.GraphicsDevice);
            _exitButtonImage = MainGame.Instance.GraphicsManager.GetImage("000000000001");
            _exitButtonImage.Center();
        }

        public void Update(GameTime gameTime)
        {
            // No op
        }

        public void Draw()
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(_exitButtonImage, MainGame.Instance.GetScreenCenter());
            _spriteBatch.End();
        }
    }
}
