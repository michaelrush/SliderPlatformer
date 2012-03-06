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
    public static class CollisionManager
    {
        /// <summary>
        /// Finds the set of all collisions for all entities and resolves them in order of collision time
        /// TODO: Cannot restrict to only MovableEntities
        /// </summary>
        /// <param name="entities">All active collidable entities in the game</param>
        public static void resolveCollisions(List<Entity> entities)
        {
            Dictionary<MovableEntity, List<CollisionData>> contactMap = new Dictionary<MovableEntity, List<CollisionData>>();

            for (int i = 0; i < entities.Count; i++)
            {
                // Do not check against self and do not repeats pairs
                for (int j = i + 1; j < entities.Count; j++)
                {
                    MovableEntity a = entities[i] as MovableEntity;
                    MovableEntity b = entities[j] as MovableEntity;

                    // Check for collisions. Only add contacts with smaller or equal collision time to map
                    CollisionData cd = AABB.AABBSweep(a, b);
                    if (cd != null)
                    {
                        if (contactMap.ContainsKey(a))
                            contactMap[a].Add(cd);
                        else
                            contactMap.Add(a, new List<CollisionData>() { cd });
                    }
                }
            }

            // Sort each entity's contacts by time and resolve in order
            foreach (List<CollisionData> lcd in contactMap.Values)
            {
                lcd.Sort();
                float time = lcd[0].time;
                foreach (CollisionData cd in lcd)
                {
                    if (cd.time <= time)
                        applyCollision(cd);
                }
            }

            // apply aggragate of forces to entities
            // TODO: can limit this to only Actors, see resolveCollisions
            foreach (MovableEntity e in entities)
                applyForces(e);
        }

        /// <summary>
        /// Set force and impulse on Actors
        /// TODO: Use actor collision type or some finite mass
        /// </summary>
        public static void applyCollision(CollisionData cd)
        {
            Actor a = cd.a as Actor;
            Actor b = cd.b as Actor;
            if (a != null)
            {
                // Calculate the force caused by the collision
                Vector2 fa = a.setForce(cd.b, cd.time, cd.times);

                // Apply the impulse caused by orthogonal velocity
                a.setImpulse(fa, cd.time, cd.times);
            }
            if (b != null)
            {
                Vector2 fb = b.setForce(cd.a, cd.time, cd.times);
                b.setImpulse(fb, cd.time, cd.times);
            }
        }

        /// <summary>
        /// Applies all of the forces applied on this entity for this timestep
        /// </summary>
        public static void applyForces(MovableEntity e)
        {
            Vector2? force = applyVector(e.forces);
            if (force.HasValue)
                e.position += force.Value;
            e.forces = new List<Vector2>();
        }

        /// <summary>
        /// Applies all of the imuplses applied on this entity during the previous timestep
        /// </summary>
        public static void applyImpulses(MovableEntity e)
        {
            Vector2? impulse = applyVector(e.impulses);
            if (impulse.HasValue)
                e.velocity += impulse.Value;
            e.impulses = new List<Vector2>();
        }

        /// <summary>
        /// Returns the vector with the max value in each dimension from all vectors in the given list
        /// Zero will overwrite any value
        /// </summary>
        public static Vector2? applyVector(List<Vector2> vectors)
        {
            if (vectors == null)
                return null;

            Vector2? vector = null;
            foreach (Vector2 v in vectors)
            {
                if (v == Vector2.Zero)
                    continue;
                else if (!vector.HasValue && v != Vector2.Zero)
                    vector = v;
                else
                {
                    float fX = vector.Value.X;
                    float fY = vector.Value.Y;
                    if (vector.Value.X != 0 && Math.Abs(v.X) > Math.Abs(vector.Value.X))
                        fX = v.X;
                    if (vector.Value.Y != 0 && Math.Abs(v.Y) > Math.Abs(vector.Value.Y))
                        fY = v.Y;
                    vector = new Vector2(fX, fY);
                }
            }

            return vector;
        }
    }
}
