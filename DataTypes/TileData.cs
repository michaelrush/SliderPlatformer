using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace DataTypes
{

    public enum CollisionType
    {
        /// <summary>
        /// An passable tile is one which does allow the player to move through it
        /// </summary>
        Passable = 0,

        /// <summary>
        /// An impassable tile is one which does not allow the player to move through
        /// it at all. It is completely solid.
        /// </summary>
        Impassable = 1,

        
        /// <summary>
        /// An actor who can be displaced by forces caused by other entities
        /// </summary>
        Actor = 2
    }

    [Serializable]
    public class TileData
    {
        public String texturePath;
        public CollisionType collision;
        public int posX;
        public int posY;

        public TileData(int posX, int posY, String texturePath, CollisionType collision)
        {
            this.texturePath = texturePath;
            this.collision = collision;
            this.posX = posX;
            this.posY = posY;
        }

        public TileData()
        {
            this.texturePath = "Textures/blueBlock";
            this.collision = 0;
            this.posX = 0;
            this.posY = 0;
        }
    }
}
