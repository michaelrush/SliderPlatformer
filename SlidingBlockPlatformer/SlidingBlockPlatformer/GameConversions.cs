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
        public static Vector2 toTilePosition(Vector2 pos)
        {
            return new Vector2((int)Math.Round(pos.X / Tile.Size.X) * Tile.Size.X, (int)Math.Round(pos.Y / Tile.Size.Y) * Tile.Size.Y);
        }
    }
}
