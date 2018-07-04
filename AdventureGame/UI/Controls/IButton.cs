using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NinjaGame.Input;
using System;

namespace NinjaGame.UI.Controls
{
    public interface IButton : IControl,IDisposable
    {
        string Text { get; set; }
        string DisplayText { get; }
        Action Action { get; set; }
        
        void Select();
        void HandleMouseClick(Point position, MouseButtons mouseButton, ButtonStates buttonState);
    }
}
