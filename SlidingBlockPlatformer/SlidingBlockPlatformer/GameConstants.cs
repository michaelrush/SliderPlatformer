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
    public struct GameConstants
    {
        public const int BlockSize = 20; // Smallest division of the environment. Each block in 20x20 pixels
        public const int TileSize = 10; // Smallest sliding tile size is 10x10 blocks
        public const int GameSizeX = 3; // Smallest game size is 3 tiles wide 
        public const int GameSizeY = 3; // Smallest game size is 3 tiles tall 
        public const int BackBufferWidth = 960;
        public const int BackBufferHeight = 600;
    }
}
