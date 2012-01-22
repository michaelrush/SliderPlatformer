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
    public class Player : Microsoft.Xna.Framework.GameComponent
    {
        public Vector2 position;
        public Vector2 velocity;
        public Vector2 blockIndex;
        public float speed;
        public Texture2D sprite;

        public Player(Game game, Vector2 blockIndex) : base(game)
        {
            // TODO: Construct any child components here
            position = GameConversions.indexToPosition(blockIndex);
            velocity = new Vector2();
            this.blockIndex = blockIndex; 
            speed = .25f;
            sprite = game.Content.Load<Texture2D>("Textures/purpleBlock");
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
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

            base.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(sprite, position, Color.White);
            spriteBatch.End();
        }
    }
}
