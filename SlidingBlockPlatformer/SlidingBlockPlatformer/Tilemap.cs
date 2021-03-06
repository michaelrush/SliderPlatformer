using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

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
                Texture2D texture = content.Load<Texture2D>(tdata[i].texturePath);
                texture.Name = tdata[i].texturePath;
                tiles.Add(new Tile(new Vector2(tdata[i].posX, tdata[i].posY), texture, tdata[i].collision));
            }
        }

        private float count = 0.0f;
        public void Update(GameTime gameTime)
        {
            count++;
            foreach (Tile t in tiles)
            {
                t.prevPosition = t.position;
                if (t.texture.Name.Equals("Textures/greenBlock"))
                    t.position += new Vector2((float) Math.Sin(count / 50.0f) * 5, 0);
                t.velocity = t.position - t.prevPosition;
                t.colliding = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            Texture2D blank = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Color.Red });

            if (tiles != null)
            {
                foreach (Tile t in tiles)
                {
                    if (t.colliding)
                        spriteBatch.Draw(blank, t.boundingRectangle, Color.Red);
                    else
                        spriteBatch.Draw(t.texture, t.boundingRectangle, Color.White);
                }
            }
        }
    }
}
