using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace SlidingBlockPlatformer
{
    public class ScreenManager : DrawableGameComponent
    {
        private KeyboardState keyboardState;
        private KeyboardState oldKeyboardState;
        private Game game;

        public GameScreen activeScreen;
        public StartScreen startScreen;
        public ActionScreen actionScreen;
        public EditScreen editScreen;
        public LoadingScreen loadingScreen;

        private SpriteBatch spriteBatch;

        public ScreenManager(Game game, SpriteBatch spriteBatch) : base(game)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;
        }

        public override void Initialize()
        {
            startScreen = new StartScreen(game, spriteBatch, game.Content.Load<SpriteFont>("menufont"), game.Content.Load<Texture2D>("alienmetal"));
            startScreen.Initialize();
            startScreen.Hide();

            actionScreen = new ActionScreen(game, spriteBatch);
            actionScreen.Initialize();
            actionScreen.Hide();

            editScreen = new EditScreen(game, spriteBatch);
            editScreen.Initialize();
            editScreen.Hide();
            
            loadingScreen = new LoadingScreen(game, spriteBatch);
            loadingScreen.Initialize();
            loadingScreen.Hide();

            activeScreen = startScreen;
            activeScreen.Show();

            base.Initialize();
        }

        /// <summary>
        /// Accepts a load request from a screen. Transitions from the screen to the loading screen while loading. 
        /// Transitions back to the original screen after loading is complete
        /// </summary>
        /// <param name="screenToLoad">The screen requesting the load operation</param>
        /// <param name="loadMethod">The method containing the loading logic for the screen</param>
        /// <param name="loadArgs">All arguments required for the loadMethod</param>
        public void LoadScreenContent(GameScreen screenToLoad, Delegate loadMethod, params object[] loadArgs)
        {
            // Transitions from the loading screen to the original screen
            Action FinalizeLoad = () =>
            {
                loadingScreen.Hide();
                activeScreen = screenToLoad;
                screenToLoad.Show();
            };

            screenToLoad.Hide();
            activeScreen = loadingScreen;
            loadingScreen.LoadScreenContent(loadMethod, FinalizeLoad, loadArgs);
            loadingScreen.Show();
        }

        public override void Update(GameTime gameTime)
        {
            handleInput();
            if (activeScreen.Enabled) activeScreen.Update(gameTime);
        }

        private void handleInput()
        {
            keyboardState = Keyboard.GetState();
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                game.Exit();

            if (activeScreen == startScreen)
            {
                if (CheckKey(Keys.Enter))
                {
                    if (startScreen.SelectedIndex == 0)
                    {
                        activeScreen.Hide();
                        activeScreen = actionScreen;
                        actionScreen.RequestLoadLevel("Tilemaps/Level-1.0");
                    }
                    if (startScreen.SelectedIndex == 1)
                    {
                        activeScreen.Hide();
                        activeScreen = editScreen;
                        activeScreen.Show();
                    }
                    if (startScreen.SelectedIndex == 2)
                    {
                        game.Exit();
                    }
                }
                if (CheckKey(Keys.Escape))
                {
                    game.Exit();
                }
            }

            oldKeyboardState = keyboardState;
        }

        private bool CheckKey(Keys theKey)
        {
            return keyboardState.IsKeyUp(theKey) &&
                oldKeyboardState.IsKeyDown(theKey);
        }

        public override void Draw(GameTime gameTime)
        {
            if (activeScreen.Visible) activeScreen.Draw(gameTime);
        }
    }
}

