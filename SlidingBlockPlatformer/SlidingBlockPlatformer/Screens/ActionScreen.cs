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
    public class ActionScreen : GameScreen
    {
        private KeyboardState keyboardState;
        private KeyboardState oldKeyboardState;
        private Tilemap tilemap;
        private Level level;
        private string levelIndex;

        public ActionScreen(Game game, SpriteBatch spriteBatch) : base(game, spriteBatch)
        {
            tilemap = new Tilemap(game.Services);
        }

        public void RequestLoadLevel(string levelIndex)
        {
            this.levelIndex = levelIndex;
            screenManager.LoadScreenContent(this, new Action<string>(LoadLevel), levelIndex);
        }

        private void LoadLevel(string levelIndex)
        {
            tilemap.LoadContent(levelIndex);
            level = new Level(game.Services, tilemap);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            level.Update(gameTime);
            handleInput();
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
            level.Draw(spriteBatch, GraphicsDevice);
        }
    }
}

