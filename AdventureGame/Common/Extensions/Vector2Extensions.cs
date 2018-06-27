using Microsoft.Xna.Framework;

namespace NinjaGame.Common.Extensions
{
    public static class Vector2Extensions
    {
        /// <summary>
        /// Creates a copy of a vector.
        /// </summary>
        /// <param name="vector">The vector to copy.</param>
        /// <returns>The copy vector.</returns>
        public static Vector2 Copy(this Vector2 vector)
        {
            return new Vector2(vector.X, vector.Y);
        }
    }
}