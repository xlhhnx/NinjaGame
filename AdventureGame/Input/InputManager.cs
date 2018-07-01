using NinjaGame.Input.Controllers;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public KeyboardController FirstKeyboardController()
        {
            var kbc = _controllers.Where(c => c is KeyboardController)
                               .Select(c => c as KeyboardController)
                               .FirstOrDefault();
            return kbc;
        }

        public MouseController FirstMouseController()
        {
            var mc = _controllers.Where(c => c is MouseController)
                               .Select(c => c as MouseController)
                               .FirstOrDefault();
            return mc;
        }

        public GamepadController FirstGetGamepadController(int index)
        {
            var gpc = _controllers.Where(c => c is GamepadController)
                               .Select(c => c as GamepadController)
                               .Where(gp => gp.Index == index)
                               .FirstOrDefault();
            return gpc;
        }
    }
}
