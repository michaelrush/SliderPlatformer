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
        // The set of collision times along each axis
        public Vector2 times;
        public MovableEntity a;
        public MovableEntity b;

        public CollisionData(float time, Vector2 times, MovableEntity a, MovableEntity b)
        {
            this.time = time;
            this.times = times;
            this.a = a;
            this.b = b;
        }

        /// <summary>
        /// Positive if this is earlier than other
        /// Zero if this is equal to other
        /// Negative if this is later than other
        /// </summary>
        public int CompareTo(object o)
        {
            return (int) ((this.time - (o as CollisionData).time) * 1.0 / GameConstants.epsilon);
        }
    }
}
