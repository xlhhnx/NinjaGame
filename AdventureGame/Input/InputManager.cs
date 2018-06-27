using NinjaGame.Input.Controllers;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace NinjaGame.Input
{
    public class InputManager
    {
        public List<IController> Controllers
        {
            get { return _controllers; }
            protected set { _controllers = value; }
        }

        protected List<IController> _controllers;

        public InputManager()
        {
            _controllers = new List<IController>();
        }

        public void Update()
        {
            foreach (var ctrl in _controllers)
                ctrl.Update();
        }

        public void Subscribe_KeyChangeEvent(Action<Keys, ButtonStates> action)
        {
            if (ReferenceEquals(null, action))
                return;

            foreach (var ctrl in _controllers)
            {
                var kbc = ctrl as KeyboardController;
                if (ReferenceEquals(null, kbc))
                    continue;

                kbc.KeyStateChangeEvent += action;
            }
        }

        public void Unsubscribe_KeyChangeEvent(Action<Keys, ButtonStates> action)
        {
            if (ReferenceEquals(null, action))
                return;

            foreach (var ctrl in _controllers)
            {
                var kbc = ctrl as KeyboardController;
                if (ReferenceEquals(null, kbc))
                    continue;

                kbc.KeyStateChangeEvent -= action;
            }
        }

        // TODO: Create other Sub/Unsub methods
    }
}
