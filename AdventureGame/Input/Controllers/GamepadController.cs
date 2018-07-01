using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace NinjaGame.Input.Controllers
{
    public class GamepadController : IController
    {
        public event Action<GamepadButtons, ButtonStates> ButtonPressEvent = delegate { };

        public int Index { get; protected set; }
        
        protected GamePadState _previousState;
        protected Dictionary<GamepadButtons, ButtonStates> _buttonStates;
        protected Dictionary<GamepadTriggers, float> _triggerStates;
        protected Dictionary<GamepadAnalogSticks, Vector2> _analogStickStates;

        protected readonly List<GamepadButtons> _allButtons;
        protected readonly List<GamepadAnalogSticks> _allSticks;
        protected readonly List<GamepadTriggers> _allTriggers;

        public GamepadController(int index)
        {
            Index = index;

            _previousState = GamePad.GetState(Index);
        }

        public void Update()
        {

        }
    }
}
