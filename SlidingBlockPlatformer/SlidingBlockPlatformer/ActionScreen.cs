using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SlidingBlockPlatformer
{
    class ActionScreen : GameScreen
    {
        private Level level;

        public ActionScreen(Game game, SpriteBatch spriteBatch) : base(game, spriteBatch)
        {
            level = new Level(game, "1.0");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            level.Update(gameTime);

            //Check for pause or exit
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            level.Draw(spriteBatch, GraphicsDevice);
        }
    }
}

