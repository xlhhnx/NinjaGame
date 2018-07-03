using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NinjaGame.Assets;

namespace NinjaGame.UI.Controls
{
    public interface IControl :IAsset
    {
        bool Focused { get; set; }
        bool Clicked { get; set; }
        bool Visible { get; set; }
        bool Enabled { get; set; }
        Vector2 Position { get; set; }
        Vector2 Dimensions { get; set; }
        
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
        void Focus();
        void Blur();
        void Click();
    }
}
