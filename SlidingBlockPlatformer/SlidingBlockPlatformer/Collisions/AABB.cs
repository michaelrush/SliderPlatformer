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
    public static class AABB
    {
        /// <summary>
        /// AABB Sweep in two dimensions
        /// </summary>
        public static CollisionData AABBSweep(MovableEntity a, MovableEntity b)
        {
            //the problem is solved in A's frame of reference
            //relative velocity (in normalized time)
            Vector2 v = b.velocity - a.velocity;
            float t0, t1;

            //first times of overlap along each axis
            Vector2 u0 = new Vector2(0.0f, 0.0f);

            //last times of overlap along each axis
            Vector2 u1 = new Vector2(0.0f, 0.0f);

            //check if they were overlapping on the previous frame
            if (overlaps(a, b))
            {
                //a.colliding = true;
                //b.colliding = true;
                //return new CollisionData(0.0f, u0, a, b);
            }

            //find the possible first and last times of overlap along each axis
            for( long i=0 ; i<2 ; i++ )
            {
                float aMax = max(a, i);
                float aMin = min(a, i);
                float bMax = max(b, i);
                float bMin = min(b, i);
                float vi = get(v, i);
                
                // if axis is colliding and velocity along this axis is 0, this axis will always be colliding
                if (((aMax >= bMin && aMax <= bMax) || (aMin >= bMin && aMin <= bMax)) && vi == 0)
                    set(ref u1, i, float.MaxValue);
                // If axis is not colliding and no velocity on this axis, this axis will never be colliding
                else if (vi == 0) 
                    set(ref u1, i, 0);

                if (aMax < bMin && vi < 0)
                    set(ref u0, i, (aMax - bMin) / vi);
                else if (bMax < aMin && vi > 0)
                    set(ref u0, i, (aMin - bMax) / vi);

                if (bMax > aMin && vi < 0)
                    set(ref u1, i, (aMin - bMax) / vi);
                else if (aMax > bMin && vi > 0)
                    set(ref u1, i, (aMax - bMin) / vi);
            }

            //possible first time of overlap
            t0 = Math.Max(u0.X, u0.Y);

            //possible last time of overlap
            t1 = Math.Min(u1.X, u1.Y);

            // they could have only collided if the first time of overlap occurred before the last time of overlap
            // t0 must not be zero because that implies an initial overlap which we already checked
            // t0 must be less than 1  for the collision to occur during this timestep
            if (t0 <= t1 && t0 != 0 && t0 < 1)
            {
                a.colliding = true;
                b.colliding = true;
                return new CollisionData(t0, u0, a, b);
            }

            return null;
        }

        /// <summary>
        /// returns true if a was overlapping b in the previous frame
        /// </summary>
        private static Boolean overlaps(MovableEntity a, MovableEntity b)
        {
            //vector from A to B
            Vector2 dist = b.prevPosition - a.prevPosition;

            // If either axis is disjoint, there is no overlap
            return Math.Abs(dist.X) < (a.prevBoundingRectangle.Width / 2 + b.prevBoundingRectangle.Width / 2) &&
                Math.Abs(dist.Y) < (a.prevBoundingRectangle.Height / 2 + b.prevBoundingRectangle.Height / 2);
        }

        /// <summary>
        ///  The top left extent of the entity
        /// </summary>
        /// <param name="i">The nth dimension of the entities position</param>
        private static float min(MovableEntity e, long i )
        {
            switch (i) {
                case 0: return e.prevPosition.X;
                case 1: return e.prevPosition.Y;
                default: return 0; 
            }
        }

        /// <summary>
        /// The bottom right extent of the entity
        /// </summary>
        /// <param name="i">The nth dimension of the entities position</param>
        private static float max(MovableEntity e, long i)
        {
            switch (i)
            {
                case 0: return e.prevPosition.X + e.boundingRectangle.Width - 1;
                case 1: return e.prevPosition.Y + e.boundingRectangle.Height - 1;
                default: return 0;
            }
        }

        /// <summary>
        /// Gets the nth dimension of the vector
        /// </summary>
        private static float get(Vector2 v, long i)
        {
            switch (i)
            {
                case 0: return v.X;
                case 1: return v.Y;
                default: return 0;
            }
        }

        /// <summary>
        /// Sets the nth dimension of the vector to the given values
        /// </summary>
        private static void set(ref Vector2 v, long i, float r)
        {
            switch (i)
            {
                case 0: v.X = r; break;
                case 1: v.Y = r; break;
                default: return;
            }
        }
    }
}
