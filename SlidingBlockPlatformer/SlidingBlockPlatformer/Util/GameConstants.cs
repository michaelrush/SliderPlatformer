using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;


namespace SlidingBlockPlatformer
{
    public static class GameConstants
    {
        public const int BackBufferWidth = 960;
        public const int BackBufferHeight = 600;

        // used for float comparions
        // TODO: is this necessary?
        public static float epsilon = 0.000009f;
    }
}
