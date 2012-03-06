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
    public class Player : Actor
    {
        public Texture2D sprite, spriteLeft, spriteRight;
        public Player(IServiceProvider serviceProvider, Vector2 position)
        {
            ContentManager content = new ContentManager(serviceProvider, "Content");
            this.position = position;
            velocity = new Vector2();
            speed = .25f;
            impulses = new List<Vector2>();
            forces = new List<Vector2>();
            spriteLeft = content.Load<Texture2D>("Textures/dog_left");
            spriteRight = content.Load<Texture2D>("Textures/dog_right");
            sprite = spriteLeft;
            collisionType = DataTypes.CollisionType.Actor;
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
            prevPosition = position;
            velocity = Vector2.Zero;
            CollisionManager.applyImpulses(this);
            float timeMod = gameTime.ElapsedGameTime.Milliseconds;
            KeyboardState ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.Left))
            {
                velocity.X -= speed * timeMod;
                sprite = spriteLeft;
            }
            if (ks.IsKeyDown(Keys.Right))
            {
                velocity.X += speed * timeMod;
                sprite = spriteRight;
            }
            if (ks.IsKeyDown(Keys.Up))
            {
                velocity.Y -= speed * timeMod;
            }
            if (ks.IsKeyDown(Keys.Down))
            {
                velocity.Y += speed * timeMod;
            }

            position += velocity;
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            spriteBatch.Draw(sprite, boundingRectangle, Color.White);
        }
    }
}
