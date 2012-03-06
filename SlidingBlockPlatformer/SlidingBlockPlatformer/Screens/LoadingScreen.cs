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
        SpriteFont font;
        private string loadingString;
        private Vector2 loadingStringSize;
        private Vector2 loadingStringPosition;


        public LoadingScreen(Game game, SpriteBatch spriteBatch) : base(game, spriteBatch) { }

        /// <summary>
        /// Runs the given delegates with the given arguments in a new thread
        /// </summary>
        public void LoadScreenContent(Delegate loadMethod, Delegate completeMethod, object[] loadArgs)
        {
            // Invokes load then close delegates in a new thread.
            // TODO: Remove thread sleep once loading testing is complete
            loadingThread = new Thread(() => { /* Thread.Sleep(1000); */ loadMethod.DynamicInvoke(loadArgs); completeMethod.DynamicInvoke(); });
            loadingThread.Priority = ThreadPriority.Highest;
            loadingThread.Start();
        }

        /// <summary>
        /// Calculates the loading string size and position
        /// </summary>
        protected override void LoadContent()
        {
            loadingString = "Loading...";
            font = game.Content.Load<SpriteFont>("loadfont");
            loadingStringSize = font.MeasureString(loadingString);
            loadingStringPosition = new Vector2(
                game.GraphicsDevice.Viewport.Width / 2 - loadingStringSize.X / 2,
                game.GraphicsDevice.Viewport.Height / 2 - loadingStringSize.Y / 2);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.DrawString(
                font,
                loadingString,
                loadingStringPosition, 
                Color.White);
            base.Draw(gameTime);
            spriteBatch.End();
        }
    }
}

