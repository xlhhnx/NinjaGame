using Microsoft.Xna.Framework;
using System;

namespace NinjaGame.Scenes
{
    public interface IScene : IDisposable
    {
        void Update(GameTime gameTime);
        void Draw();
    }
}
