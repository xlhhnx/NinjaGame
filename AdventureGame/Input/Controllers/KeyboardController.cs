using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NinjaGame.Input.Controllers
{
    public class KeyboardController : IController
    {        
        public event Action<Keys, ButtonStates> KeyStateChangeEvent = delegate { };

        public bool StateChanged { get { return _stateChanged; } }
        public List<Keys> AllKeys { get { return _allKeys; } }
        
        public ButtonStates? this[Keys key]
        {
            get
            {
                if (_keyStates.ContainsKey(key))
                {
                    return _keyStates[key];
                }
                else
                {
                    return null;
                }
            }
        }

        protected bool _stateChanged;
        protected Dictionary<Keys, ButtonStates> _keyStates;
        protected List<Keys> _allKeys;

        public KeyboardController()
        {
            _keyStates = new Dictionary<Keys, ButtonStates>();
            _allKeys = new List<Keys>();

            foreach (Keys k in Enum.GetValues(typeof(Keys)).Cast<Keys>())
            {
                _keyStates.Add(k, ButtonStates.Up);
                _allKeys.Add(k);
            }
            _stateChanged = false;
        }

        public void Update()
        {
            var _currentKeyboardState = Keyboard.GetState();
            var newKeyStates = new Dictionary<Keys, ButtonStates>();
            _stateChanged = false;

            foreach (Keys k in _allKeys)
            {
                switch (_keyStates[k])
                {
                    case (ButtonStates.Up):
                        {
                            if (_currentKeyboardState.IsKeyDown(k))
                            {
                                newKeyStates.Add(k,ButtonStates.Pressed);
                                KeyStateChangeEvent(k, ButtonStates.Pressed);
                            }
                            else
                                newKeyStates.Add(k,ButtonStates.Up);
                        }
                        break;
                    case (ButtonStates.Pressed):
                        {
                            if (_currentKeyboardState.IsKeyDown(k))
                            {
                                newKeyStates.Add(k,ButtonStates.Down);
                                KeyStateChangeEvent(k, ButtonStates.Down);
                            }
                            else
                            {
                                newKeyStates.Add(k,ButtonStates.Released);
                                KeyStateChangeEvent(k, ButtonStates.Released);
                            }
                        }
                        break;
                    case (ButtonStates.Down):
                        {
                            if (_currentKeyboardState.IsKeyDown(k))
                                newKeyStates.Add(k,ButtonStates.Down);
                            else
                            {
                                newKeyStates.Add(k,ButtonStates.Released);
                                KeyStateChangeEvent(k, ButtonStates.Released);
                            }
                        }
                        break;
                    case (ButtonStates.Released):
                        {
                            if (_currentKeyboardState.IsKeyDown(k))
                            {
                                newKeyStates.Add(k,ButtonStates.Pressed);
                                KeyStateChangeEvent(k, ButtonStates.Pressed);
                            }
                            else
                            {
                                newKeyStates.Add(k,ButtonStates.Up);
                                KeyStateChangeEvent(k, ButtonStates.Up);
                            }
                        }
                        break;
                }
            }
            _keyStates = newKeyStates;
        }
    }
}