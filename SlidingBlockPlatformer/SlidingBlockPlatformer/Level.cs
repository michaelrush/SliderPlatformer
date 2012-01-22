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
        //private FoeList foes;
        //private ObjectList objects;
        private String levelIndex;
        private Game game;

        public Level(Game game, String index)
        {
            levelIndex = index;
            this.game = game;
            player = new Player(game, new Vector2(0,8));
            tilemap = new Tilemap(game, levelIndex);
            game.Components.Add(tilemap);
        }

        public void Initialize(GameTime gameTime)
        {
        }

        public void Update(GameTime gameTime)
        {
            player.Update(gameTime);
            tilemap.Update(gameTime);
            //check for collisions through CollisionManager after each update?
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            tilemap.Draw(spriteBatch, graphics);
            player.Draw(spriteBatch, graphics);
        }
    }
}
