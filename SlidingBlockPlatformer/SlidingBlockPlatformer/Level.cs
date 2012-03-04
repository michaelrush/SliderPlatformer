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
        private List<Entity> entities
        {
            get
            {
                List<Entity> entities = new List<Entity>();
                entities.Add(player);
                entities.AddRange(tilemap.tiles);
                return entities;
            }
        }

        /// <summary>
        /// Constructs a new level.
        /// </summary>
        /// <param name="game">
        /// The game object that will be used to contruct the level components
        /// </param>
        /// <param name="levelIndex">
        /// The name of the level file to be loaded
        /// </param>
        public Level(IServiceProvider serviceProvider, Tilemap tilemap)
        {
            this.tilemap = tilemap;
            player = new Player(serviceProvider, Vector2.Zero);
        }

        /// <summary>
        /// Updates all objects in the world and performs collision between them
        /// </summary>
        public void Update(GameTime gameTime)
        {
            player.Update(gameTime);
            tilemap.Update(gameTime);
            CollisionManager.updateCollisions(entities);
        }

        /// <summary>
        /// Draw everything in the level from background to foreground.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            spriteBatch.Begin();
            tilemap.Draw(spriteBatch, graphics);
            player.Draw(spriteBatch, graphics);
            spriteBatch.End();
        }
    }
}
