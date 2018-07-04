using System;
using NinjaGame.Assets;
using NinjaGame.Common.Extensions;
using NinjaGame.Graphics2D.Config;
using Microsoft.Xna.Framework;

namespace NinjaGame.Graphics2D.Assets
{
    public class Sprite : Image
    {
        public override bool Loaded { get { return _texture2DAsset.Loaded; } }

        public bool Looping
        {
            get { return _looping; }
            set { _looping = value; }
        }

        public override GraphicType GraphicType { get { return GraphicType.Sprite; } }

        public TimeSpan ElapsedTime
        {
            get { return _elapsedTime; }
            set { _elapsedTime = value; }
        }

        public int FrameTime
        {
            get { return _frameTime; }
            set { _frameTime = value; }
        }


        protected int _rows;
        protected int _currentRow;
        protected int _columns;
        protected int _currentColumn;
        protected int _frameTime;
        protected bool _looping;
        protected Vector2 _sourcePosition;
        protected Vector2 _sourceDimensions;
        protected TimeSpan _elapsedTime;


        public Sprite(string id, string name, int rows, int columns, Texture2DAsset texture2DAsset, Vector2 sourcePosition, Vector2 sourceDimensions)
            : base(id, name, texture2DAsset, sourcePosition, sourceDimensions)
        {
            _sourcePosition = sourcePosition;
            _sourceDimensions = sourceDimensions;
            _rows = rows;
            _columns = columns;
            _currentRow = 0;
            _currentColumn = 0;
            _elapsedTime = new TimeSpan();

            _frameTime = Graphics2DConfig.DefaultFrameTime;
        }

        public virtual void ChangeFrame()
        {
            _currentColumn++;
            if (_currentColumn >= _columns)
            {
                _currentColumn = 0;
                _currentRow++;
                if (_currentRow >= _rows && _looping)
                    _currentRow = 0;
            }

            _elapsedTime = new TimeSpan();
            _sourceRectangle = new Rectangle(
                (int)(_sourcePosition.X + (_currentColumn * _sourceDimensions.X)) // X position
                , (int)(_sourcePosition.Y + (_currentRow * _sourceDimensions.Y)) // Y Position
                , (int)(_sourceDimensions.X) // Width
                , (int)(_sourceDimensions.Y) // Height
                );
        }

        public virtual void ResetFrame()
        {
            _currentColumn = 0;
            _currentRow = 0;
            
            _elapsedTime = new TimeSpan();
            _sourceRectangle = new Rectangle(
                (int)(_sourcePosition.X + (_currentColumn * _sourceDimensions.X)) // X position
                , (int)(_sourcePosition.Y + (_currentRow * _sourceDimensions.Y)) // Y Position
                , (int)(_sourceDimensions.X) // Width
                , (int)(_sourceDimensions.Y) // Height
                );
        }

        public override IGraphic2D Copy()
        {
            var sprite = new Sprite(Id, Name, _rows, _columns, _texture2DAsset, _sourceRectangle.GetPosition(), _sourceRectangle.GetDimensions())
            {
                Color = Color,
                PositionOffset = PositionOffset,
                Dimensions = Dimensions,
                FrameTime = FrameTime,
                Looping = Looping,
                Enabled = Enabled,
                Visible = Visible
            };
            return sprite;
        }
    }
}