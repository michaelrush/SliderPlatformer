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
    class EditScreen : GameScreen
    {
        public List<DataTypes.TileData> tilemap;
        private KeyboardState keyboardState;
        private KeyboardState oldKeyboardState;
        private MouseState mouseState;
        private MouseState oldMouseState;
        private Game game;
        private DataTypes.TileData brownTemplate;
        private DataTypes.TileData redTemplate;
        private DataTypes.TileData greenTemplate;
        private DataTypes.TileData blueTemplate;
        private DataTypes.TileData tileTemplate;
        private SpriteBatch spriteBatch;

        private IAsyncResult result;
        private Object stateobj;
        private bool GameSaveRequested = false;

        public EditScreen(Game game, SpriteBatch spriteBatch) : base(game, spriteBatch)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;
            brownTemplate = new DataTypes.TileData(0, 0, "Textures/brownBlock", 0); //DataTypes.TileCollision.Impassable);
            redTemplate = new DataTypes.TileData(0, 0, "Textures/redBlock", 0); //DataTypes.TileCollision.Impassable);
            greenTemplate = new DataTypes.TileData(0, 0, "Textures/greenBlock", 0); //DataTypes.TileCollision.Impassable);
            blueTemplate = new DataTypes.TileData(0, 0, "Textures/blueBlock", 0); //DataTypes.TileCollision.Impassable);
            tileTemplate = brownTemplate;
            tilemap = new List<DataTypes.TileData>();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            handleKey();
            handleClick();
        }

        private void handleKey()
        {
            keyboardState = Keyboard.GetState();
            // Allows the game to exit
            if (CheckKey(Keys.D1))
            {
                tileTemplate = brownTemplate;
            }
            if (CheckKey(Keys.D2))
            {
                tileTemplate = redTemplate;
            }
            if (CheckKey(Keys.D3))
            {
                tileTemplate = blueTemplate;
            }
            if (CheckKey(Keys.D4))
            {
                tileTemplate = greenTemplate;
            }
            if (CheckKey(Keys.S))
            {
                saveMap();
            }
            checkSave();
            

            oldKeyboardState = keyboardState;
        }

        private bool CheckKey(Keys theKey)
        {
            return keyboardState.IsKeyUp(theKey) &&
                oldKeyboardState.IsKeyDown(theKey);
        }

        private void handleClick()
        {
            mouseState = Mouse.GetState();

            if (CheckClick("left"))
            {
                Vector2 position = new Vector2(mouseState.X, mouseState.Y);
                foreach (DataTypes.TileData t in tilemap)
                {
                    if (GameConversions.toTilePosition(new Vector2(t.posX, t.posY)) == GameConversions.toTilePosition(position))
                    {
                        tilemap.Remove(t);
                        break;
                    }
                }

                tilemap.Add(new DataTypes.TileData((int)position.X, (int)position.Y, tileTemplate.texturePath, tileTemplate.collision));
            }
            if (CheckClick("right"))
            {
                Vector2 position = new Vector2(mouseState.X, mouseState.Y);
                foreach (DataTypes.TileData t in tilemap)
                {
                    if (GameConversions.toTilePosition(new Vector2(t.posX, t.posY)) == GameConversions.toTilePosition(position))
                    {
                        tilemap.Remove(t);
                        break;
                    }
                }
            }
            
            oldMouseState = mouseState;
        }

        private bool CheckClick(String button)
        {
            if (button == "left")
            {
                return mouseState.LeftButton == ButtonState.Pressed;
            }
            else if (button == "right")
            {
                return mouseState.RightButton == ButtonState.Pressed;
            }
            return false;
        }

        private void saveMap()
        {
            // Set the request flag
            if ((!Guide.IsVisible) && (GameSaveRequested == false))
            {
                GameSaveRequested = true;
                result = StorageDevice.BeginShowSelector(
                        PlayerIndex.One, null, null);
            }
        }

        private void checkSave()
        {
            if ((GameSaveRequested) && (result.IsCompleted)) 
            {
                StorageDevice device = StorageDevice.EndShowSelector(result);
                if (device != null && device.IsConnected)
                {
                    doSaveGame(device);
                }
                GameSaveRequested = false;
            }
        }

        /// <summary>
        /// This method serializes a data object into
        /// the StorageContainer for this game.
        /// </summary>
        /// <param name="device"></param>
        private void doSaveGame(StorageDevice device)
        {
            // Create the data to save.
            DataTypes.TileData[] tiles = new DataTypes.TileData[tilemap.Count()];

            // Open a storage container.
            IAsyncResult result =
                device.BeginOpenContainer("StorageDemo", null, null);

            // Wait for the WaitHandle to become signaled.
            result.AsyncWaitHandle.WaitOne();

            StorageContainer container = device.EndOpenContainer(result);

            // Close the wait handle.
            result.AsyncWaitHandle.Close();

            string filename = "leveltest.xml";

            // Check to see whether the save exists.
            if (container.FileExists(filename))
                // Delete it so that we can create one fresh.
                container.DeleteFile(filename);

            // Create the file.
            Stream stream = container.CreateFile(filename);

            // Convert the object to XML data and put it in the stream.      
            XmlSerializer serializer = new XmlSerializer(typeof(DataTypes.TileData));
            serializer.Serialize(Console.Out, tilemap);

            // Close the file.
            stream.Close();

            // Dispose the container, to commit changes.
            container.Dispose();
        }
        
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            foreach (DataTypes.TileData t in tilemap)
            {
                spriteBatch.Draw(game.Content.Load<Texture2D>(t.texturePath), new Vector2(t.posX, t.posY), Color.White);
            }
            spriteBatch.DrawString(game.Content.Load<SpriteFont>("menufont"), tileTemplate.texturePath, Vector2.Zero, Color.Black);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

