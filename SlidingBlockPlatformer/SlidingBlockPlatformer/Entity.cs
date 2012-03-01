using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Media;

namespace SlidingBlockPlatformer
{
    public abstract class Entity
    {
        public Texture2D texture;
        public Vector2 position;
        public const int width = 20;
        public const int height = 20;
        public static readonly Vector2 Size = new Vector2(width, height);
        public DataTypes.CollisionType collision;

        // The bounding box is calculated from the tile's current position
        public BoundingBox boundingBox
        {
            get { return new BoundingBox(new Vector3(position.X, position.Y, 0), new Vector3(position.X + width, position.Y + height, 0)); }
        }
        
        // The rectangle is calculated from the tile's current position
        public Rectangle boundingRectangle
        {
            get { return new Rectangle((int)position.X, (int)position.Y, width, height); }
        }
    }
}
