using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NinjaGame.Input.Controllers
{
    public class KeyboardController : IController
    {        
        public event Action<Keys, ButtonStates> KeyStateChangeEvent = delegate { };
        
        public List<Keys> AllKeys { get { return _allKeys; } }
        
        protected Dictionary<Keys, ButtonStates> _keyStates;
        protected readonly List<Keys> _allKeys;

        public KeyboardController()
        {
            _keyStates = new Dictionary<Keys, ButtonStates>();
            _allKeys = new List<Keys>();

            foreach (Keys k in Enum.GetValues(typeof(Keys)).Cast<Keys>())
            {
                _keyStates.Add(k, ButtonStates.Up);
                _allKeys.Add(k);
            }
        }

        public void Update()
        {
            var currentKeyboardState = Keyboard.GetState();
            var newKeyStates = new Dictionary<Keys, ButtonStates>();

            foreach (Keys k in _allKeys)
            {
                switch (_keyStates[k])
                {
                    case (ButtonStates.Up):
                        {
                            if (currentKeyboardState.IsKeyDown(k))
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
                            if (currentKeyboardState.IsKeyDown(k))
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
                            if (currentKeyboardState.IsKeyDown(k))
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
                            if (currentKeyboardState.IsKeyDown(k))
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