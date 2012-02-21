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
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Tilemap
    {
        public List<Tile> tiles = new List<Tile>();
        private ContentManager content;

        /// <summary>
        /// Loads the given level from the storage device
        /// </summary>
        /// <param name="serviceProvider">Provides the content reference</param>
        public Tilemap(IServiceProvider serviceProvider)
        {
            content = new ContentManager(serviceProvider, "Content");
        }

        /// <summary>
        /// Deserializes the .xnb file at the given path into a TileData array and populates the tiles list
        /// </summary>
        /// <param name="filename"></param>
        public void LoadContent(string path)
        {
            DataTypes.TileData[] tdata = content.Load<DataTypes.TileData[]>(path);

            tiles.Clear();
            for (int i = 0; i < tdata.Length; i++)
            {
                tiles.Add(new Tile(new Vector2(tdata[i].posX, tdata[i].posY), content.Load<Texture2D>(tdata[i].texturePath), tdata[i].collision));
            }
        }

        public void Update(GameTime gameTime)
        {
        }
    }
}
