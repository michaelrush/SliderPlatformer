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
    public abstract class MovableEntity : Entity
    {
        public Vector2 prevPosition;
        public Vector2 velocity;
        public float speed;

        /// <summary>
        /// The rectangle that represents the exact polygon that the entity traversed in one timestep
        /// </summary>
        /// <param name="gameTime"></param>
        /// <returns></returns>
        public Rectangle timestepRectangle
        {
            get
            {
                return new Rectangle(
                    (int)prevPosition.X,
                    (int)prevPosition.Y,
                    (int)(position.X - prevPosition.X + boundingRectangle.Width),
                    (int)(position.Y - prevPosition.Y + boundingRectangle.Height));
            }
        }

        /// <summary>
        /// The rectangle that represents that maximum rectangle that the entity traversed in one timestep
        /// </summary>
        /// <param name="gameTime"></param>
        /// <returns></returns>
        public Polygon timestepPolygon
        {
            get
            {
                Vector2 prevTopRight = new Vector2(prevPosition.X + boundingRectangle.Width, prevPosition.Y);
                Vector2 prevTopLeft = prevPosition;
                Vector2 prevBottomLeft = new Vector2(prevPosition.X, prevPosition.Y + boundingRectangle.Height);
                Vector2 prevBottomRight = new Vector2(prevPosition.X + boundingRectangle.Width, prevPosition.Y + boundingRectangle.Height);
                Vector2 topRight = new Vector2(position.X + boundingRectangle.Width, position.Y);
                Vector2 topLeft = position;
                Vector2 bottomLeft = new Vector2(position.X, position.Y + boundingRectangle.Height);
                Vector2 bottomRight = new Vector2(position.X + boundingRectangle.Width, position.Y + boundingRectangle.Height);

                if (velocity.X > 0 && velocity.Y > 0)
                    return new Polygon(prevTopLeft, prevTopRight, topRight, bottomRight, bottomLeft, prevBottomLeft);
                else if (velocity.X > 0 && velocity.Y < 0)
                    return new Polygon(prevTopLeft, topLeft, topRight, bottomRight, prevBottomRight, prevBottomLeft);
                else if (velocity.X < 0 && velocity.Y > 0)
                    return new Polygon(prevTopLeft, prevTopRight, prevBottomRight, bottomRight, bottomLeft, topLeft);
                else if (velocity.X < 0 && velocity.Y < 0)
                    return new Polygon(prevBottomRight, prevTopRight, topRight, topLeft, bottomLeft, prevBottomLeft);
                else if (velocity.X == 0 && velocity.Y > 0)
                    return new Polygon(prevTopRight, prevTopLeft, bottomLeft, bottomRight);
                else if (velocity.X == 0 && velocity.Y < 0)
                    return new Polygon(topRight, topLeft, prevBottomLeft, prevBottomRight);
                else if (velocity.X < 0 && velocity.Y == 0)
                    return new Polygon(prevTopRight, topLeft, bottomLeft, prevBottomRight);
                else if (velocity.X > 0 && velocity.Y == 0)
                    return new Polygon(topRight, prevTopLeft, prevBottomLeft, bottomRight);
                else
                    return new Polygon(topRight, topLeft, bottomLeft, bottomRight);
            }
        }
    }
}
