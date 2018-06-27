using System;
using NinjaGame.Common.Extensions;
using Microsoft.Xna.Framework;

namespace NinjaGame.Common.Bounding
{
    public class BoundingBox : IBounds
    {

       // TODO: Revisit how bounds position work. (make into position offset)
        public Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                _rectangle = new Rectangle(_position.ToPoint(), _dimensions.ToPoint());
            }
        }

        public Vector2 Dimensions
        {
            get { return _dimensions; }
            set
            {
                _dimensions = value;
                _rectangle = new Rectangle(_position.ToPoint(), _dimensions.ToPoint());
            }
        }


        protected Vector2 _position;
        protected Vector2 _dimensions;
        protected Rectangle _rectangle;


        public BoundingBox(Vector2 position, Vector2 dimensions)
        {
            _position = position;
            _dimensions = dimensions;
            _rectangle = new Rectangle(_position.ToPoint(), _dimensions.ToPoint());
        }

        public BoundingBox(Rectangle rectangle)
        {
            _rectangle = rectangle;
            _position = _rectangle.GetPosition();
            _dimensions = _rectangle.GetDimensions();
        }

        public bool Contains(Point point)
        {
            return _rectangle.Contains(point);
        }

        public bool Contains(Vector2 point)
        {
            return _rectangle.Contains(point);
        }

        public void Center()
        {
            throw new NotImplementedException();
        }
    }
}