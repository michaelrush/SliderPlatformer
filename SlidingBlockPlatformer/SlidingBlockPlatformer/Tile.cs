using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SlidingBlockPlatformer
{
    public struct Tile
    {
        public Texture2D texture;
        public DataTypes.TileCollision collision;
        public Vector2 position;

        public const int width = 20;
        public const int height = 20;
        public static readonly Vector2 Size = new Vector2(width, height);

        /// <summary>
        /// Constructs a new tile
        /// </summary>
        /// <param name="position">Initial position of the tile</param>
        /// <param name="texture">Texture content to render</param>
        /// <param name="collision">Collision type used for collision resolution</param>
        public Tile(Vector2 position, Texture2D texture, DataTypes.TileCollision collision)
        {
            this.position = position;
            this.texture = texture;
            this.collision = collision;
        }
    }
}
