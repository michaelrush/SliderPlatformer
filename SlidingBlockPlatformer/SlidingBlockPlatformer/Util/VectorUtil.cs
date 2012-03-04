using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;


namespace SlidingBlockPlatformer
{
    public static class VectorUtil
    {
        public static float dotProduct(Vector2 a, Vector2 b)
        {
            return a.X * b.X + a.Y * b.Y;
        }

        public static Vector2 projection(Vector2 a, Vector2 b)
        {
            if (b.LengthSquared() == 0)
                return Vector2.Zero;
            return (dotProduct(a, b) / b.LengthSquared()) * b;
        }

        public static Vector2 scale(Vector2 a, float b)
        {
            if (a == Vector2.Zero)
                return Vector2.Zero;
            Vector2 unit = a;
            unit.Normalize();
            return unit * b;
        }
    }
}
