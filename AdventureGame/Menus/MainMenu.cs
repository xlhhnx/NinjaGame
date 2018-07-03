using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NinjaGame.Graphics2D.Assets;
using NinjaGame.Graphics2D.Extensions;
using NinjaGame.Scenes;
using NinjaGame.UI.Controls;

namespace NinjaGame.Menus
{
    public class MainMenu : IScene
    {
        protected Button _button;
        protected SpriteBatch _spriteBatch;

        public MainMenu()
        {
            _spriteBatch = new SpriteBatch(MainGame.Instance.GraphicsDevice);
            var bText = MainGame.Instance.GraphicsManager.GetText("000000000002");
            var blurredImage = MainGame.Instance.GraphicsManager.GetImage("000000000001");
            var focusedImage = blurredImage.Copy() as Image;
            var clickedImage = blurredImage.Copy() as Image;

            focusedImage.Color = Color.Yellow;
            clickedImage.Color = Color.Red;
            bText.Color = Color.Black;
            bText.DisabledColor = Color.DarkGray;

            _button = new Button(MainGame.Instance.GetScreenCenter(), new Vector2(75,25), MainGame.Instance.UnloadAndExit, blurredImage, focusedImage, clickedImage, bText);
        }

        public void Update(GameTime gameTime)
        {
            _button.Update(gameTime);
        }

        public void Draw()
        {
            _spriteBatch.Begin();
            _button.Draw(_spriteBatch);
            _spriteBatch.End();
        }

        public void Dispose()
        {
            // No op
        }
    }
}
