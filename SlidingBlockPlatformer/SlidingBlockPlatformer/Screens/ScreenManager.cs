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
using DataTypes;

namespace SlidingBlockPlatformer
{
    class ScreenManager : GameComponent
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
            game.Components.Add(startScreen);
            startScreen.Hide();

            //string path = StorageContainer.TitleLocation;
            actionScreen = new ActionScreen(game, spriteBatch);
            game.Components.Add(actionScreen);
            actionScreen.Hide();

            editScreen = new EditScreen(game, spriteBatch);
            game.Components.Add(editScreen);
            editScreen.Hide();

            activeScreen = startScreen;
            activeScreen.Show();

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            handleInput();
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
            }

            oldKeyboardState = keyboardState;
        }

        private bool CheckKey(Keys theKey)
        {
            return keyboardState.IsKeyUp(theKey) &&
                oldKeyboardState.IsKeyDown(theKey);
        }
    }
}

