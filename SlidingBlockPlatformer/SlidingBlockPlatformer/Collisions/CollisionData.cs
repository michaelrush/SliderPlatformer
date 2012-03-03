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
    public class CollisionData : IComparable
    {
        public float time;
        public Vector2 times;
        public MovableEntity a;
        public MovableEntity b;

        public CollisionData(float time, Vector2 times, MovableEntity actor, MovableEntity mover)
        {
            this.time = time;
            this.times = times;
            this.a = mover;
            this.b = actor;
        }

        public int CompareTo(object o)
        {
            return (int) this.time;
        }
    }
}
