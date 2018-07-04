using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NinjaGame.Exceptions;
using NinjaGame.Graphics2D.Assets;
using NinjaGame.Graphics2D.Extensions;
using NinjaGame.Input;
using NinjaGame.Input.Controllers;
using NinjaGame.Assets;

namespace NinjaGame.UI.Controls
{
    public class Button : IButton
    {
        public bool Focused { get; set; }
        public bool Clicked { get; set; }
        public bool Visible { get; set; }
        public bool Enabled { get; set; }
        public bool Centered
        {
            get { return _centered; }
            set
            {
                _centered = value;
                _buttonText.Center(Dimensions);

                if (_centered)
                {
                    _blurredImage.Center(Dimensions);
                    _focusedImage.Center(Dimensions);
                    _clickedImage.Center(Dimensions);
                }
            }
        }
        public string Text
        {
            get { return _buttonText.FullText; }
            set { _buttonText.FullText = value; }
        }
        public string DisplayText { get { return _buttonText.DrawText; } }
        public Action Action { get; set; }
        public Vector2 Position
        {
            get { return _position; }
            set
            {
                _boundingBox = null;
                _position = value;
            }
        }
        public Vector2 Dimensions
        {
            get { return _currentImage.Dimensions; }
            set
            {
                _boundingBox = null;

                _blurredImage.Dimensions = value;
                _focusedImage.Dimensions = value;
                _clickedImage.Dimensions = value;
                _buttonText.Dimensions = value;

                Centered = Centered;
            }
        }
        public AssetType Type => AssetType.None;

        protected Rectangle BoundingBox
        {
            get
            {
                if (_boundingBox is null && _centered)
                    _boundingBox = new Rectangle((Position - Dimensions / 2).ToPoint(), Dimensions.ToPoint());
                else if (_boundingBox is null)
                    _boundingBox = new Rectangle(Position.ToPoint(), Dimensions.ToPoint());

                return _boundingBox.Value;
            }
        }

        protected bool _centered;
        protected Image _blurredImage;
        protected Image _focusedImage;
        protected Image _clickedImage;
        protected Image _currentImage;
        protected Text _buttonText;
        protected Vector2 _position;
        protected Rectangle? _boundingBox;

        private GamepadController _gamepadController;
        private KeyboardController _keyboardController;
        private MouseController _mouseController;

        public Button(Image blurredImage, Image focusedImage, Image clickedImage, Text buttonText)
        {
            if (blurredImage is null || focusedImage is null || clickedImage is null || buttonText is null)
                throw new NullParameterException();
            
            _blurredImage = blurredImage;
            _focusedImage = focusedImage;
            _clickedImage = clickedImage;
            _buttonText = buttonText;
            _currentImage = _blurredImage;
            Position = Vector2.Zero;
            Dimensions = Vector2.Zero;
            Centered = false;

            _gamepadController = MainGame.Instance.InputManager.FirstGetGamepadController(1);
            _keyboardController = MainGame.Instance.InputManager.FirstKeyboardController();
            _mouseController = MainGame.Instance.InputManager.FirstMouseController();
            ResetEventHandlers();
        }

        public void Update(GameTime gameTime)
        {
            // No op
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_currentImage, Position);
            spriteBatch.DrawString(_buttonText, Position);
        }

        public void Blur()
        {
            Focused = false;
            Clicked = false;
            _currentImage = _blurredImage;
        }

        public void ResetEventHandlers()
        {
            Dispose();

            if (!(_gamepadController is null))
                _gamepadController.ButtonPressEvent += HandleGamepadButtonPress;

            if (!(_keyboardController is null))
                _keyboardController.KeyStateChangeEvent += HandleKeyPress;

            if (!(_mouseController is null))
            {
                _mouseController.MouseClickEvent += HandleMouseClick;
                _mouseController.MouseMoveEvent += HandleMouseMove;
            }
        }

        public void Dispose()
        {
            if (!(_gamepadController is null))
                _gamepadController.ButtonPressEvent -= HandleGamepadButtonPress;

            if (!(_keyboardController is null))
                _keyboardController.KeyStateChangeEvent -= HandleKeyPress;

            if (!(_mouseController is null))
            {
                _mouseController.MouseClickEvent -= HandleMouseClick;
                _mouseController.MouseMoveEvent -= HandleMouseMove;
            }
        }

        public void Focus()
        {
            Focused = true;
            Clicked = false;
            _currentImage = _focusedImage;
        }

        public void Select()
        {
            if(!(Action is null))
                Action();
        }

        public void HandleKeyPress(Keys key, ButtonStates buttonState)
        {
            if (Focused && key == Keys.Enter && buttonState == ButtonStates.Pressed)
            {
                Click();
            }
            else if (Clicked && buttonState == ButtonStates.Released)
            {
                Blur();
                Select();
            }
        }

        public void HandleMouseMove(Point position)
        {
            if (Clicked)
                return;

            if (BoundingBox.Contains(position))
                Focus();
            else
                Blur();
        }

        public void HandleMouseClick(Point position, MouseButtons mouseButton, ButtonStates buttonState)
        {
            if (mouseButton != MouseButtons.Left)
                return;

            if (buttonState == ButtonStates.Pressed && BoundingBox.Contains(position))
            {
                Click();
            }
            else if (Clicked && buttonState == ButtonStates.Released && BoundingBox.Contains(position))
            {
                Blur();
                Select();
            }
        }

        public void HandleGamepadButtonPress(GamepadButtons button, ButtonStates buttonState)
        {
            if (Focused && button == GamepadButtons.A && buttonState == ButtonStates.Pressed)
            {
                Click();
            }
            else if (Clicked && buttonState == ButtonStates.Released)
            {
                Blur();
                Select();
            }
        }

        public void Click()
        {
            Focused = true;
            Clicked = true;
            _currentImage = _clickedImage;
        }

        public void Unload()
        {
            // No op
        }
    }
}
