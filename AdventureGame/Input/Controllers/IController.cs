using Microsoft.Xna.Framework.Input;
using System;

namespace NinjaGame.Input.Controllers
{
    public interface IController
    {
        event Action<Keys, ButtonStates> KeyStateChangeEvent;

        void Update();
    }
}
