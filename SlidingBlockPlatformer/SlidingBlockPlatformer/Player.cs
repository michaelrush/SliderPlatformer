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
    public class Player : MovableEntity
    {
        public Texture2D sprite, spriteLeft, spriteRight;
        public Player(IServiceProvider serviceProvider, Vector2 position)
        {
            ContentManager content = new ContentManager(serviceProvider, "Content");
            this.position = position;
            velocity = new Vector2();
            speed = .125f;
            spriteLeft = content.Load<Texture2D>("Textures/dog_left");
            spriteRight = content.Load<Texture2D>("Textures/dog_right");
            sprite = spriteLeft;
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
            prevPosition = position;
            Vector2 dP = new Vector2(gameTime.ElapsedGameTime.Milliseconds*speed,gameTime.ElapsedGameTime.Milliseconds*speed);
            KeyboardState ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.Left))
            {
                position.X -= dP.X;
                sprite = spriteLeft;
            }
            if (ks.IsKeyDown(Keys.Right))
            {
                position.X += dP.X;
                sprite = spriteRight;
            }
            if (ks.IsKeyDown(Keys.Up))
            {
                position.Y -= dP.Y;
            }
            if (ks.IsKeyDown(Keys.Down))
            {
                position.Y += dP.Y;
            }

            velocity = new Vector2((position.X - prevPosition.X) / gameTime.ElapsedGameTime.Milliseconds, (position.Y - prevPosition.Y) / gameTime.ElapsedGameTime.Milliseconds);
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            spriteBatch.Draw(sprite, boundingRectangle, Color.White);
        }
    }
}
