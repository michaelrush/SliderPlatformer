using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SlidingBlockPlatformer
{
    public class StartScreen : GameScreen
    {
        MenuComponent menuComponent;
        Texture2D image;
        Rectangle imageRectangle;

        public int SelectedIndex
        {
            get { return menuComponent.SelectedIndex; }
            set { menuComponent.SelectedIndex = value; }
        }

        public StartScreen(Game game,
        SpriteBatch spriteBatch,
        SpriteFont spriteFont,
        Texture2D image)
            : base(game, spriteBatch)
        {
            string[] menuItems = { "Start Game", "Level Editor", "End Game" };
            menuComponent = new MenuComponent(game,
                spriteBatch,
                spriteFont,
                menuItems);
            Components.Add(menuComponent);
            this.image = image;
            imageRectangle = new Rectangle(
                0,
                0,
                Game.Window.ClientBounds.Width,
                Game.Window.ClientBounds.Height);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Texture2D pixel = new Texture2D(GraphicsDevice, 1, 1);
            Color[] colorData = { Color.Gray };
            pixel.SetData<Color>(colorData);
            spriteBatch.Begin();
            spriteBatch.Draw(pixel, imageRectangle, Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

