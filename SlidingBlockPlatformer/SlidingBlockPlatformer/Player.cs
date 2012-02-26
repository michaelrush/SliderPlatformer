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
        private int count = 0;
        public Player(IServiceProvider serviceProvider, Vector2 position)
        {
            ContentManager content = new ContentManager(serviceProvider, "Content");
            this.position = position;
            velocity = new Vector2();
            speed = .25f;
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
            count = (count + 1) % 10;
            if (count > 0)
                return;

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
            spriteBatch.Draw(sprite, position, Color.White);
            /*
            //Debug: Draw player polygon
            Texture2D blank = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Color.White });

            foreach (Line ln in timestepPolygon.Lines) {
                float angle = (float)Math.Atan2(ln.End.Y - ln.Start.Y, ln.End.X - ln.Start.X);
                float length = Vector2.Distance(new Vector2(ln.Start.X, ln.Start.Y), new Vector2(ln.End.X, ln.End.Y));

                spriteBatch.Draw(blank, new Vector2(ln.Start.X, ln.Start.Y), null, Color.Red,
                           angle, Vector2.Zero, new Vector2(length, 1),
                           SpriteEffects.None, 0);
            }

            //Debug: Draw player timestamp rectangle
            Polygon timestepPoly = new Polygon(new Vector2(timestepRectangle.X, timestepRectangle.Y), new Vector2(timestepRectangle.X + timestepRectangle.Width, timestepRectangle.Y),
                new Vector2(timestepRectangle.X + timestepRectangle.Width, timestepRectangle.Y + timestepRectangle.Height), new Vector2(timestepRectangle.X, timestepRectangle.Y + timestepRectangle.Height));
            foreach (Line ln in timestepPoly.Lines)
            {
                float angle = (float)Math.Atan2(ln.End.Y - ln.Start.Y, ln.End.X - ln.Start.X);
                float length = Vector2.Distance(new Vector2(ln.Start.X, ln.Start.Y), new Vector2(ln.End.X, ln.End.Y));

                spriteBatch.Draw(blank, new Vector2(ln.Start.X, ln.Start.Y), null, Color.Blue,
                           angle, Vector2.Zero, new Vector2(length, 1),
                           SpriteEffects.None, 0);
            }
             */
        }
    }
}
