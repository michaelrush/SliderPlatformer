using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace SlidingBlockPlatformer
{
    class Level
    {
        private Player player;
        private Tilemap tilemap;

        private String levelIndex;

        /// <summary>
        /// Constructs a new level.
        /// </summary>
        /// <param name="game">
        /// The game object that will be used to contruct the level components
        /// </param>
        /// <param name="levelIndex">
        /// The name of the level file to be loaded
        /// </param>
        public Level(IServiceProvider serviceProvider, String levelIndex)
        {
            this.levelIndex = levelIndex;
            tilemap = new Tilemap(serviceProvider, levelIndex);
            player = new Player(serviceProvider, Vector2.Zero); //tilemap.startPosition);
        }

        /// <summary>
        /// Updates all objects in the world and performs collision between them
        /// </summary>
        public void Update(GameTime gameTime)
        {
            player.Update(gameTime);
            //tilemap.Update(gameTime);
        }

        /// <summary>
        /// Draw everything in the level from background to foreground.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            spriteBatch.Begin();
            foreach (Tile t in tilemap.tiles)
            {
                spriteBatch.Draw(t.texture, t.position, Color.White);
            }
            player.Draw(spriteBatch, graphics);
            spriteBatch.End();
        }
    }
}
