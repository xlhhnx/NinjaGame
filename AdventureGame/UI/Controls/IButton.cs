using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NinjaGame.Input;
using System;

namespace NinjaGame.UI.Controls
{
    public interface IButton : IDisposable
    {
        bool Focused { get; set; }
        bool Clicked { get; set; }
        bool Visible { get; set; }
        bool Enabled { get; set; }
        string Text { get; set; }
        string DisplayText { get; }
        Action Action { get; set; }
        Vector2 Position { get; set; }
        Vector2 Dimensions { get; set; }

        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
        void Select();
        void Focus();
        void Blur();
        void Click();
        void HandleMouseClick(Point position, MouseButtons mouseButton, ButtonStates buttonState);
    }
}
