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
    public static class CollisionManager
    {
        public static void updateCollisions(List<Entity> entities)
        {
            List<MovableEntity> movers = new List<MovableEntity>();
            foreach (Entity e in entities)
            {
                MovableEntity me = e as MovableEntity;
                if (me != null)
                    movers.Add(me);
            }

            List<CollisionData> contacts = findContacts(movers, entities);

            foreach (CollisionData cd in contacts)
            {
                resolveCollision(cd);
            }
        }

        /// <summary>
        /// Use AABB sweep test to find all contact pairs
        /// </summary>
        public static List<CollisionData> findContacts(List<MovableEntity> movers, List<Entity> entities)
        {
            List<CollisionData> contacts = new List<CollisionData>();

            foreach (Entity e in entities)
            {
                foreach (MovableEntity m in movers)
                {
                    if (e != m)
                    {
                        CollisionData cd = AABB.AABBSweep(m, e as MovableEntity);
                        if (cd != null)
                            contacts.Add(cd);
                    }
                }
            }
            contacts.Sort();
            return contacts;
        }

        public static void resolveCollision(CollisionData cd)
        {
            cd.a.rewindToPercent(cd.time);
            cd.a.applyImpulse(cd);
            cd.b.rewindToPercent(cd.time);
            cd.b.applyImpulse(cd);
        }
    }
}
