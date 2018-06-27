using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace NinjaGame.Common.Bounding
{
    public class CompoundBounds : IBounds
    {
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }


        protected Vector2 _position;
        protected List<IBounds> _subBounds;


        public CompoundBounds(Vector2 position)
        {
            _position = position;
            _subBounds = new List<IBounds>();
        }

        public bool Contains(Point point)
        {
            bool contains = false;

            foreach (var b in _subBounds)
            {
                var relativePoint = point.ToVector2() - _position;
                contains = b.Contains(relativePoint);
                if (contains)
                    break;
            }
            return contains;
        }

        public bool Contains(Vector2 point)
        {
            bool contains = false;

            foreach (var b in _subBounds)
            {
                var relativePoint = point - _position;
                contains = b.Contains(relativePoint);
                if (contains)
                    break;
            }
            return contains;
        }

        public void Center()
        {
            throw new NotImplementedException();
        }
    }
}