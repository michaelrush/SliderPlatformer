using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace SlidingBlockPlatformer
{
    class ActionScreen : GameScreen
    {
        private KeyboardState keyboardState;
        private KeyboardState oldKeyboardState;
        private ScreenManager screenManager;
        private ContentLoader contentLoader;

        private Level level;

        public ActionScreen(Game game, SpriteBatch spriteBatch) : base(game, spriteBatch)
        {
            screenManager = (ScreenManager)game.Services.GetService(typeof(ScreenManager));
            loadLevel("1");
        }

        public void loadLevel(string levelIndex)
        {
            AsyncCallback callback = new AsyncCallback(loadCallback); 
            contentLoader = new ContentLoader(game, callback, levelIndex);
        }

        public void loadCallback(IAsyncResult result)
        {
            Stream s = (Stream)result.AsyncState;
            Console.Out.WriteLine("done");
        }

        public override void Update(GameTime gameTime)
        {
            /*
            base.Update(gameTime);
            level.Update(gameTime);
            handleInput();*/
            Console.Out.WriteLine(contentLoader.loadingState);
        }

        /// <summary>
        /// Escapes to start screen if espace key is pressed
        /// </summary>
        private void handleInput()
        {
            if (CheckKey(Keys.Escape))
            {
                this.Hide();
                screenManager.activeScreen = screenManager.startScreen;
                screenManager.activeScreen.Show();
            }
        }

        /// <summary>
        /// Checks if a key is being released
        /// </summary>
        /// <param name="theKey">The key value to be evaluated</param>
        private bool CheckKey(Keys theKey)
        {
            return keyboardState.IsKeyUp(theKey) && oldKeyboardState.IsKeyDown(theKey);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            //level.Draw(spriteBatch, GraphicsDevice);
        }
    }
}

