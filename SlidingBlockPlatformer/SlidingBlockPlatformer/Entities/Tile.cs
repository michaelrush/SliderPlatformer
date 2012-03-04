using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
            this.forces = new List<Vector2>();
            this.impulses = new List<Vector2>();
        }
    }
}