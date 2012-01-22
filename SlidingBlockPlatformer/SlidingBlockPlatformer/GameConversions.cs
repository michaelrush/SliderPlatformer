using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace SlidingBlockPlatformer
{
    public static class GameConversions
    {
        public static Vector2 positionToIndex(Vector2 pos)
        {
            return new Vector2((int) (pos.X / GameConstants.TileSize), (int) (pos.Y / GameConstants.TileSize));
        }

        public static Vector2 indexToPosition(Vector2 index)
        {
            return new Vector2(index.X * GameConstants.TileSize, index.Y * GameConstants.TileSize);
        }
    }
}
