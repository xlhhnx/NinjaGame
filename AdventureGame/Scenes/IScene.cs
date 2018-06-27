using Microsoft.Xna.Framework;

namespace NinjaGame.Scenes
{
    public interface IScene
    {
        void Update(GameTime gameTime);
        void Draw();
    }
}
