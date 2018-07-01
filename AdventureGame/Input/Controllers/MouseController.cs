using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace NinjaGame.Input.Controllers
{
    public class MouseController : IController
    {
        public event Action<Point> MouseMoveEvent = delegate { };
        public event Action<Point, MouseButtons, ButtonStates> MouseClickEvent = delegate { };

        protected MouseState _previousState;
        protected Dictionary<MouseButtons, ButtonStates> _buttonStates;

        protected readonly List<MouseButtons> _allButtons;

        public MouseController()
        {
            _previousState = Mouse.GetState();
            _allButtons = new List<MouseButtons>();
            _buttonStates = new Dictionary<MouseButtons, ButtonStates>();

            foreach (MouseButtons mb in Enum.GetValues(typeof(MouseButtons)).Cast<MouseButtons>())
            {
                _buttonStates.Add(mb, ButtonStates.Up);
                _allButtons.Add(mb);
            }
        }

        public void Update()
        {
            var currentState = Mouse.GetState();
            var currentButtonStates = GetButtonStates(currentState);
            var currentPos = RelativePosition(new Point(currentState.X, currentState.Y));
            var newButtonStates = new Dictionary<MouseButtons, ButtonStates>();

            if (currentState.Position != _previousState.Position)
                MouseMoveEvent(currentState.Position);

            foreach (var mb in _allButtons)
            {
                switch (_buttonStates[mb]) 
                {
                    case (ButtonStates.Up):
                        {
                            if (currentButtonStates[mb] == ButtonState.Pressed)
                            {
                                newButtonStates.Add(mb, ButtonStates.Pressed);
                                MouseClickEvent(currentPos, mb, ButtonStates.Pressed);
                            }
                            else
                                newButtonStates.Add(mb, ButtonStates.Up);
                        }
                        break;
                    case (ButtonStates.Pressed):
                        {
                            if (currentButtonStates[mb] == ButtonState.Pressed)
                            {
                                newButtonStates.Add(mb, ButtonStates.Down);
                                MouseClickEvent(currentPos, mb, ButtonStates.Down);
                            }
                            else
                            {
                                newButtonStates.Add(mb, ButtonStates.Released);
                                MouseClickEvent(currentPos, mb, ButtonStates.Released);
                            }
                        }
                        break;
                    case (ButtonStates.Down):
                        {
                            if (currentButtonStates[mb] == ButtonState.Pressed)
                                newButtonStates.Add(mb, ButtonStates.Down);
                            else
                            {
                                newButtonStates.Add(mb, ButtonStates.Released);
                                MouseClickEvent(currentPos, mb, ButtonStates.Released);
                            }
                        }
                        break;
                    case (ButtonStates.Released):
                        {
                            if (currentButtonStates[mb] == ButtonState.Pressed)
                            {
                                newButtonStates.Add(mb, ButtonStates.Pressed);
                                MouseClickEvent(currentPos, mb, ButtonStates.Pressed);
                            }
                            else
                            {
                                newButtonStates.Add(mb, ButtonStates.Up);
                                MouseClickEvent(currentPos, mb, ButtonStates.Up);
                            }
                        }
                        break;
                }
            }
            _buttonStates = newButtonStates;
        }

        private Point RelativePosition(Point absolutePosition)
        {
            var screenPos = new Point(MainGame.Instance.GraphicsDevice.Viewport.X, MainGame.Instance.GraphicsDevice.Viewport.Y);
            var gamePos = absolutePosition - screenPos;
            return gamePos;
        }

        private Dictionary<MouseButtons, ButtonState> GetButtonStates(MouseState state) 
        {
            var result = new Dictionary<MouseButtons, ButtonState>
            {
                { MouseButtons.Left, state.LeftButton },
                { MouseButtons.Middle, state.MiddleButton },
                { MouseButtons.Right, state.RightButton },
                { MouseButtons.X1, state.XButton1 },
                { MouseButtons.X2, state.XButton2 }
            };

            return result;
        }
    }
}

