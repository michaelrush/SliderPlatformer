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
            if (t < 0 || t > 1)
                throw new ArgumentException("rewind percent must be between 0 and 1");

            if (t == 0 || velocity == Vector2.Zero)
                return minCollisionResolution(other);

            // find first pixel where they were not colliding
            Vector2 safePoint = velocity * t;

            if (times.Y == t)
            {
                if (velocity.Y > 0)
                    safePoint -= new Vector2(0, 1);
                else if (velocity.Y < 0)
                    safePoint += new Vector2(0, 1);
            }

            if (times.X == t)
            {
                if (velocity.X > 0)
                    safePoint -= new Vector2(1, 0);
                else if (velocity.X < 0)
                    safePoint += new Vector2(1, 0);
            }

            Vector2 force = safePoint - velocity;
            forces.Add(force);
            return force;
        }

        public void applyForce(Vector2 force)
        {
            position = position + force;
        }

        public void setImpulse(Vector2 force, float t, Vector2 times)
        {
            /*
            Vector2 scaleVF = VectorUtil.scale(-1 * force, velocity.Length());
            if (times.Y == t)
                scaleVF = VectorUtil.scale(new Vector2(-1 * force.X, 0), velocity.Length());
            else if (times.X == t)
                scaleVF = VectorUtil.scale(new Vector2(0, -1 * force.Y), velocity.Length());
            if (times.X == times.Y)
                scaleVF = Vector2.Zero;
            impulses.Add(scaleVF * (1 - t));
            */

            Vector2 projVF = VectorUtil.projection(velocity, force);
            if (times.Y == t)
                projVF = VectorUtil.projection(velocity, new Vector2(0, force.Y));
            else if (times.X == t)
                projVF = VectorUtil.projection(velocity, new Vector2(force.X, 0));
            else if (times.X == times.Y)
                projVF = Vector2.Zero;
            impulses.Add((velocity - projVF) * (1 - t));
        }

        /// <summary>
        /// Min vector to remove a from b
        /// </summary>
        private Vector2 minCollisionResolution(MovableEntity other)
        {
            if (this.collisionType == DataTypes.CollisionType.Impassable)
                return Vector2.Zero;

            Rectangle a = this.prevBoundingRectangle;
            Rectangle b = other.prevBoundingRectangle; 

            Vector2 dist = new Vector2(b.Center.X - a.Center.X, b.Center.Y - a.Center.Y);
            float minX = b.Width - Math.Abs(dist.X);
            float minY = b.Height - Math.Abs(dist.Y);
            if (minX < minY)
            {
                if (dist.X < 0)
                    return new Vector2(minX, 0);
                else
                    return new Vector2(-1 * minX, 0);
            }
            else
            {
                if (dist.Y < 0)
                    return new Vector2(0, minY);
                else
                    return new Vector2(0, -1 * minY);
            }
        }
    }
}
