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
    class ScreenManager : DrawableGameComponent
    {
        private KeyboardState keyboardState;
        private KeyboardState oldKeyboardState;
        private Game game;

        public GameScreen activeScreen;
        public StartScreen startScreen;
        public ActionScreen actionScreen;
        public EditScreen editScreen;

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

            //string path = StorageContainer.TitleLocation;
            actionScreen = new ActionScreen(game, spriteBatch);
            actionScreen.Initialize();
            actionScreen.Hide();

            editScreen = new EditScreen(game, spriteBatch);
            editScreen.Initialize();
            editScreen.Hide();

            activeScreen = startScreen;
            activeScreen.Show();

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            handleInput();
            activeScreen.Update(gameTime);
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
                        activeScreen.Show();
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
            activeScreen.Draw(gameTime);
        }
    }
}

