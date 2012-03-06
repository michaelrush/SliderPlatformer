using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;


namespace SlidingBlockPlatformer
{
    public static class VectorUtil
    {
        /// <summary>
        /// Returns the dot product a * b
        /// </summary>
        public static float dotProduct(Vector2 a, Vector2 b)
        {
            return a.X * b.X + a.Y * b.Y;
        }

        /// <summary>
        /// Returns the projection of a onto b
        /// </summary>
        public static Vector2 projection(Vector2 a, Vector2 b)
        {
            if (b.LengthSquared() == 0)
                return Vector2.Zero;
            return (dotProduct(a, b) / b.LengthSquared()) * b;
        }
    }
}
