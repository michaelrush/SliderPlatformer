using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;

namespace SlidingBlockPlatformer
{
    public abstract class MovableEntity : Entity
    {
        public Vector2 prevPosition;
        public Vector2 velocity;
        public Vector2 impulse;
        public float speed;

        // The rectangle is calculated from the tile's previous position
        public Rectangle prevBoundingRectangle
        {
            get { return new Rectangle((int)prevPosition.X, (int)prevPosition.Y, width, height); }
        }

        /// <summary>
        /// Rewinds the current step to a specified percentage of the calculated distance
        /// </summary>
        /// <param name="t">Percent of timestep to rewind to</param>
        public void rewindToPercent(float t) {
            if (t < 0 || t > 1)
                throw new ArgumentException("rewind percent must be between 0 and 1");

            Vector2 dp = position - prevPosition;
            Vector2 rdp = dp * t;
            position = prevPosition + rdp;
        }

        public void applyImpulse(CollisionData cd)
        {
            Vector2 v = cd.b.velocity- cd.a.velocity;
            Vector2 rem = v * (1 - cd.time);
            if(cd.times.X < cd.times.Y)
                impulse = new Vector2(0, rem.Y);
            else if (cd.times.Y < cd.times.X)
                impulse = new Vector2(rem.X, 0);
        }
    }
}
