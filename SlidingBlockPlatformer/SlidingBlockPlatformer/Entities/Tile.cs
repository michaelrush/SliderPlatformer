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
    public class Tile : MovableEntity
    {
        /// <summary>
        /// Constructs a new tile
        /// </summary>
        /// <param name="position">Initial position of the tile</param>
        /// <param name="texture">Texture content to render</param>
        /// <param name="collision">Collision type used for collision resolution</param>
        public Tile(Vector2 position, Texture2D texture, DataTypes.CollisionType collision)
        {
            colliding = false;
            this.position = position;
            prevPosition = position;
            this.texture = texture;
            this.collisionType = collision;
        }
    }
}