using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace SlidingBlockPlatformer
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Player
    {
        public Vector2 position;
        public Vector2 velocity;
        public float speed;
        public Texture2D sprite;

        public Player(IServiceProvider serviceProvider, Vector2 position)
        {
            ContentManager content = new ContentManager(serviceProvider, "Content");
            // TODO: Construct any child components here
            this.position = position;
            velocity = new Vector2();
            speed = .25f;
            sprite = content.Load<Texture2D>("Textures/dog");
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
            Vector2 dP = new Vector2(gameTime.ElapsedGameTime.Milliseconds*speed,gameTime.ElapsedGameTime.Milliseconds*speed);
            KeyboardState ks = Keyboard.GetState();
            // TODO: Add your update code here

            if (ks.IsKeyDown(Keys.Left))
            {
                position.X -= dP.X;
            }
            if (ks.IsKeyDown(Keys.Right))
            {
                position.X += dP.X;
            }
            if (ks.IsKeyDown(Keys.Up))
            {
                position.Y -= dP.Y;
            }
            if (ks.IsKeyDown(Keys.Down))
            {
                position.Y += dP.Y;
            }
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            spriteBatch.Draw(sprite, position, Color.White);
        }
    }
}
