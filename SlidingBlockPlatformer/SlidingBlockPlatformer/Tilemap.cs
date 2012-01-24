using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SlidingBlockPlatformer
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Tilemap
    {
        public Tile[] tiles;

        public Tilemap(IServiceProvider serviceProvider, String levelIndex)
        {
            ContentManager content = new ContentManager(serviceProvider, "Content");
            // separate file with meta data?
            DataTypes.TileData[] tiledata = content.Load<DataTypes.TileData[]>("Tilemaps/Level-" + levelIndex);
            tiles = new Tile[tiledata.Length];
            for (int i = 0; i < tiledata.Length; i++) {
                tiles[i] = new Tile(new Vector2(tiledata[i].posX, tiledata[i].posY), content.Load<Texture2D>(tiledata[i].texturePath), tiledata[i].collision);
            }

        }
    }
}
