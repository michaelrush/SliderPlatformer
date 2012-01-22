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
using DataTypes;

namespace SlidingBlockPlatformer
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Tilemap : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public Tile[] tilemap;
        public String levelIndex;
        private Game game;

        public Tilemap(Game game, String level) : base(game)
        {
            // TODO: Construct any child components here
            levelIndex = level;
            this.game = game;
            loadTilemap(levelIndex);
        }

        public void loadTilemap(String level)
        {
            TileData[] tiles = game.Content.Load<TileData[]>("Tilemaps/Level-" + level);
            tilemap = new Tile[tiles.Length];
            for (int i = 0; i < tiles.Length; i++)
            {
                tilemap[i] = new Tile(game, tiles[i]);
            }
        }


        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            
            base.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            spriteBatch.Begin();
            foreach (Tile t in tilemap) {
                spriteBatch.Draw(t.texture, t.position, Color.White);
            }
            spriteBatch.End();
        }
    }
}
