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
        public static void updateCollisions(List<Entity> entities)
        {
            foreach (MovableEntity e in entities)
                applyImpulses(e);

            List<CollisionData> contacts = findContacts(entities);
            foreach (CollisionData cd in contacts)
                resolveCollision(cd);

            foreach (MovableEntity e in entities)
                applyForces(e);
        }

        /// <summary>
        /// Use AABB sweep test to find all contact pairs
        /// </summary>
        public static List<CollisionData> findContacts(List<Entity> entities)
        {
            List<CollisionData> contacts = new List<CollisionData>();
            Dictionary<MovableEntity, CollisionData> contactMap = new Dictionary<MovableEntity, CollisionData>();
            
            foreach (MovableEntity a in entities)
            {
                foreach (MovableEntity b in entities)
                {
                    bool repeat = contactMap.ContainsKey(a) && contactMap[a].b == a || contactMap.ContainsKey(b) && contactMap[b].a == b;
                    if (a != b && !repeat)
                    {
                        CollisionData cd = AABB.AABBSweep(a, b);
                        if (cd != null)
                        {
                            if (!contactMap.ContainsKey(a))
                            {
                                contactMap.Add(a, cd);
                            }
                            else if (cd.time < contactMap[a].time)
                            {
                                contactMap.Remove(a);
                                contactMap.Add(a, cd);
                            }
                            else if (cd.time == contactMap[a].time)
                            {
                                // TODO: don't use a dict. Need multiple in case of two collisions at exact same time
                                contacts.Add(cd);
                            }
                        }
                    }
                }
            }
            contacts.AddRange(contactMap.Values.ToList<CollisionData>());
            return contacts;
        }

        public static void resolveCollision(CollisionData cd)
        {
            // Calculate the force caused by the collision
            Vector2 fa = cd.a.setForce(cd.b, cd.time, cd.times);
            Vector2 fb = cd.b.setForce(cd.a, cd.time, cd.times);

            // Apply the impulse caused by orthogonal velocity
            cd.a.setImpulse(fa, cd.time, cd.times);
            cd.b.setImpulse(fb, cd.time, cd.times);
        }

        public static void applyForces(MovableEntity e)
        {
            Vector2? force = applyVector(e.forces);
            if (force.HasValue)
                e.position += force.Value;
            e.forces = new List<Vector2>();
        }

        public static void applyImpulses(MovableEntity e)
        {
            Vector2? impulse = applyVector(e.impulses);
            if (impulse.HasValue)
            {
                e.velocity += impulse.Value;
                e.position += impulse.Value;
            }
            e.impulses = new List<Vector2>();
        }


        public static Vector2? applyVector(List<Vector2> vectors)
        {
            if (vectors == null)
                return null;

            Vector2? vector = null;
            foreach (Vector2 v in vectors)
            {
                if (!vector.HasValue && v != Vector2.Zero)
                    vector = v;
                else if (v != Vector2.Zero)
                {
                    float fX = vector.Value.X;
                    float fY = vector.Value.Y;
                    if (vector.Value.X != 0)
                    {
                        if (Math.Abs(v.X) > Math.Abs(vector.Value.X))
                            fX = v.X;
                    }
                    if (vector.Value.Y != 0)
                    {
                        if (Math.Abs(v.Y) > Math.Abs(vector.Value.Y))
                            fY = v.Y;
                    }
                    vector = new Vector2(fX, fY);
                }
            }

            return vector;
        }
    }
}
