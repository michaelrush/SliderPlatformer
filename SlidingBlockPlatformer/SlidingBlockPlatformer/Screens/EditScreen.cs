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
    public class EditScreen : GameScreen
    {
        private KeyboardState keyboardState;
        private KeyboardState oldKeyboardState;
        private MouseState mouseState;
        private MouseState oldMouseState;

        private List<DataTypes.TileData> tilemap;
        private IAsyncResult asyncResult;
        private StorageDevice storageDevice;
        private StorageContainer storageContainer;
        private PlayerIndex playerIndex = PlayerIndex.One;
        private string filenameTemplate = "Tilemaps/Level-xxx.xml";
        private int filenameIndex = 0;
        private DataTypes.TileData brownTemplate;
        private DataTypes.TileData redTemplate;
        private DataTypes.TileData greenTemplate;
        private DataTypes.TileData blueTemplate;
        private DataTypes.TileData tileTemplate;

        enum SavingState
        {
            NotSaving,
            ReadyToSelectStorageDevice,
            SelectingStorageDevice,

            ReadyToOpenStorageContainer,    // once we have a storage device start here
            OpeningStorageContainer,
            ReadyToSave
        }
        SavingState savingState = SavingState.NotSaving;

        /// <summary>
        /// Constructor for the level editor
        /// </summary>
        public EditScreen(Game game, SpriteBatch spriteBatch) : base(game, spriteBatch)
        {
            screenManager = (ScreenManager)game.Services.GetService(typeof(ScreenManager));
            this.spriteBatch = spriteBatch;
            brownTemplate = new DataTypes.TileData(0, 0, "Textures/brownBlock", DataTypes.CollisionType.Impassable);
            redTemplate = new DataTypes.TileData(0, 0, "Textures/redBlock", DataTypes.CollisionType.Impassable);
            greenTemplate = new DataTypes.TileData(0, 0, "Textures/greenBlock", DataTypes.CollisionType.Impassable);
            blueTemplate = new DataTypes.TileData(0, 0, "Textures/blueBlock", DataTypes.CollisionType.Impassable);
            tileTemplate = brownTemplate;
            tilemap = new List<DataTypes.TileData>();
        }

        /// <summary>
        /// Handles mouse inputs and checks for save requests
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            handleKey();
            handleClick();
            UpdateSaving();
        }

        /// <summary>
        /// Maps number keys to selection between the tile templates, and the 'S' key to requesting a save\
        /// Escapes to start screen if espace key is pressed
        /// </summary>
        private void handleKey()
        {
            keyboardState = Keyboard.GetState();

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
            if (CheckKey(Keys.Back) || CheckKey(Keys.Delete))
            {
                tilemap.Clear();
            }
            if (CheckKey(Keys.S) && savingState == SavingState.NotSaving)
            {
                savingState = SavingState.ReadyToOpenStorageContainer;
            }
            if (CheckKey(Keys.Escape))
            {
                this.Hide();
                screenManager.activeScreen = screenManager.startScreen;
                screenManager.activeScreen.Show();
            }
            
            oldKeyboardState = keyboardState;
        }

        /// <summary>
        /// Checks if a key is being released
        /// </summary>
        /// <param name="theKey">The key value to be evaluated</param>
        private bool CheckKey(Keys theKey)
        {
            return keyboardState.IsKeyUp(theKey) && oldKeyboardState.IsKeyDown(theKey);
        }

        /// <summary>
        /// Handles mouse events. Left clicking adds or overwrites blocks to the tilemap at the intersecting tile position. 
        /// Right clicking removed blocks at the intersecting tile position
        /// </summary>
        private void handleClick()
        {
            mouseState = Mouse.GetState();
            Vector2 clickTilePosition = GameConversions.toTilePosition(new Vector2(mouseState.X, mouseState.Y));

            if (CheckClick("left"))
            {
                foreach (DataTypes.TileData t in tilemap)
                {
                    if (GameConversions.toTilePosition(new Vector2(t.posX, t.posY)) == clickTilePosition)
                    {
                        tilemap.Remove(t);
                        break;
                    }
                }

                tilemap.Add(new DataTypes.TileData((int)clickTilePosition.X, (int)clickTilePosition.Y, tileTemplate.texturePath, tileTemplate.collision));
            }
            if (CheckClick("right"))
            {
                foreach (DataTypes.TileData t in tilemap)
                {
                    if (GameConversions.toTilePosition(new Vector2(t.posX, t.posY)) == clickTilePosition)
                    {
                        tilemap.Remove(t);
                        break;
                    }
                }
            }
            
            oldMouseState = mouseState;
        }

        /// <summary>
        /// Checks if the corresponding mouse button is currently pressed down
        /// </summary>
        /// <param name="button">The mouse button to check</param>
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

        /// <summary>
        /// Transitions the SavingState through the saving process. 
        /// Handles selecting the storage device, opening the storage container, and executing the save function
        /// </summary>
        private void UpdateSaving()
        {
            switch (savingState)
            {
                case SavingState.ReadyToSelectStorageDevice:
#if XBOX
                    if (!Guide.IsVisible)
#endif
                    {
                        asyncResult = StorageDevice.BeginShowSelector(playerIndex, null, null);
                        savingState = SavingState.SelectingStorageDevice;
                    }
                    break;

                case SavingState.SelectingStorageDevice:
                    if (asyncResult.IsCompleted)
                    {
                        storageDevice = StorageDevice.EndShowSelector(asyncResult);
                        savingState = SavingState.ReadyToOpenStorageContainer;
                    }
                    break;

                case SavingState.ReadyToOpenStorageContainer:
                    if (storageDevice == null || !storageDevice.IsConnected)
                    {
                        savingState = SavingState.ReadyToSelectStorageDevice;
                    }
                    else
                    {
                        asyncResult = storageDevice.BeginOpenContainer("EditorStorageContainer", null, null);
                        savingState = SavingState.OpeningStorageContainer;
                    }
                    break;

                case SavingState.OpeningStorageContainer:
                    if (asyncResult.IsCompleted)
                    {
                        storageContainer = storageDevice.EndOpenContainer(asyncResult);
                        savingState = SavingState.ReadyToSave;
                    }
                    break;

                case SavingState.ReadyToSave:
                    if (storageContainer == null)
                    {
                        savingState = SavingState.ReadyToOpenStorageContainer;
                    }
                    else
                    {
                        try
                        {
                            string filename = GetUniqueFileName();
                            Save(filename);
                        }
                        catch (IOException e)
                        {
                            // Replace with in game dialog notifying user of error
                            Console.Out.WriteLine(e.Message);
                        }
                        finally
                        {
                            storageContainer.Dispose();
                            storageContainer = null;
                            savingState = SavingState.NotSaving;
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Checks for an unused file sequentially according to the template numbering
        /// </summary>
        /// <returns>The filename of the next available file</returns>
        private string GetUniqueFileName()
        {
            string filename = filenameTemplate;
            do
            {
                filenameIndex++;
                filename = filenameTemplate.Replace("xxx", filenameIndex.ToString());
            } while (storageContainer.FileExists(filename));

            return filename;
        }

        /// <summary>
        /// Saves an XML serialization of the tilemap to the storage container
        /// </summary>
        /// <param name="filename">The file to save to</param>
        private void Save(string filename)
        {
            using (Stream stream = storageContainer.CreateFile(filename))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<DataTypes.TileData>));
                serializer.Serialize(stream, tilemap);
            }
        }
        
        /// <summary>
        /// Draws the current tilemap, descriptive text, and cursor to the screen
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            // Draw populated tiles
            foreach (DataTypes.TileData t in tilemap)
            {
                spriteBatch.Draw(game.Content.Load<Texture2D>(t.texturePath), new Rectangle(t.posX, t.posY, (int)Tile.Size.X, (int)Tile.Size.Y), Color.White);
            }

            // Draw saving text
            if (savingState == SavingState.ReadyToSave)
            {
                spriteBatch.DrawString(game.Content.Load<SpriteFont>("menufont"), "saving to '" + filenameTemplate.Replace("xxx", filenameIndex.ToString()) + "'", new Vector2(0, 0), Color.Black);
            }

            // Draw cursor
            spriteBatch.Draw(game.Content.Load<Texture2D>(tileTemplate.texturePath), new Rectangle(mouseState.X, mouseState.Y, (int)Tile.Size.X, (int)Tile.Size.Y), Color.White);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

