using Microsoft.Xna.Framework;

namespace NinjaGame.Common.Extensions
{
    public static class RectangleExtensions
    {
        /// <summary>
        /// Creates a Vector2of a rectangle's position.
        /// </summary>
        /// <param name="rectangle">The rectangle from which the position vector will be created from.</param>
        /// <returns>The position vector.</returns>
        public static Vector2 GetPosition(this Rectangle rectangle)
        {
            return new Vector2(rectangle.X, rectangle.Y);
        }

        /// <summary>
        /// Creates a Vector2 of a rectangle's dimensions.
        /// </summary>
        /// <param name="rectangle">The rectangle from which the dimensions vector will be created from.</param>
        /// <returns>The dimensions vector.</returns>
        public static Vector2 GetDimensions(this Rectangle rectangle)
        {
            return new Vector2(rectangle.Width, rectangle.Height);
        }

        /// <summary>
        /// Creates a copy of a rectangle.
        /// </summary>
        /// <param name="rectangle">The rectangle to be copied.</param>
        /// <returns>The copy Rectangle.</returns>
        public static Rectangle Copy(this Rectangle rectangle)
        {
            return new Rectangle(rectangle.Location, rectangle.Size);
        }
    }
}