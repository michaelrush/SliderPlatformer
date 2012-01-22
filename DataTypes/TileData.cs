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
    [Serializable]
    public class TileData
    {
        public String texturePath;
        public TileCollision collision;

        public int width;
        public int height;
        public int blockX;
        public int blockY;

        public TileData()
        {
        }

        public TileData(String texPath, TileCollision collision, int blockX, int blockY, int width, int height)
        {
            this.texturePath = texPath;
            this.collision = collision;
            this.blockX = blockX;
            this.blockY = blockY;
            this.width = width;
            this.height = height;
        }
    }

}
