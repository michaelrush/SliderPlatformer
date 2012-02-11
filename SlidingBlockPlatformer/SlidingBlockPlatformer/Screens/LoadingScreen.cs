using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System.Threading;

namespace SlidingBlockPlatformer
{
    public class LoadingScreen : GameScreen
    {
        private Thread loadingThread;
        private Level level;

        public LoadingScreen(Game game, SpriteBatch spriteBatch) : base(game, spriteBatch)
        {
        }

        public void LoadContent(GameScreen screenToLoad)
        {
            loadingThread = new Thread(screenToLoad.Load);
            loadingThread.Priority = ThreadPriority.Highest;
            loadingThread.Start();

            screenToLoad.Hide();
            this.Show();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.DrawString(game.Content.Load<SpriteFont>("menufont"), "Loading", new Vector2(0, 0), Color.Black);
            base.Draw(gameTime);
        }
    }
}

