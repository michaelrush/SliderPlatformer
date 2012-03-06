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
        public List<Vector2> impulses;
        public List<Vector2> forces;
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
        public Vector2 setForce(MovableEntity other, float t, Vector2 times)
        {
            Vector2 force = Vector2.Zero;

            if (t < 0 || t > 1)
                throw new ArgumentException("rewind percent must be between 0 and 1");

            if (t == 0)
            {
                force = AABB.minimumTranslation(this, other);
                forces.Add(force);
                return force;
            }

            // find first pixel where they were not colliding
            Vector2 relV = velocity - other.velocity;
            Vector2 safePoint = relV * t;

            if (times.Y == t)
            {
                if (relV.Y > 0)
                    safePoint -= new Vector2(0, 1);
                else if (relV.Y < 0)
                    safePoint += new Vector2(0, 1);
            }

            if (times.X == t)
            {
                if (relV.X > 0)
                    safePoint -= new Vector2(1, 0);
                else if (relV.X < 0)
                    safePoint += new Vector2(1, 0);
            }

            force = safePoint - relV;
            forces.Add(force);
            return force;
        }

        public void setImpulse(Vector2 force, float t, Vector2 times)
        {
            Vector2 projVF = VectorUtil.projection(velocity, force);
            if (times.Y == t)
                projVF = VectorUtil.projection(velocity, new Vector2(0, force.Y));
            else if (times.X == t)
                projVF = VectorUtil.projection(velocity, new Vector2(force.X, 0));
            else if (times.X == times.Y)
                projVF = Vector2.Zero;
            Vector2 impulse = (velocity - projVF) * (1 - t);
            impulses.Add(impulse);
        }
    }
}
