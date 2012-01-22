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
    class EditScreen : GameScreen
    {
        public List<Tile> tilemap;
        private KeyboardState keyboardState;
        private KeyboardState oldKeyboardState;
        private MouseState mouseState;
        private MouseState oldMouseState;
        private Game game;
        private TileData brownTemplate;
        private TileData redTemplate;
        private TileData greenTemplate;
        private TileData blueTemplate;
        private TileData tileTemplate;
        private SpriteBatch spriteBatch;

        private IAsyncResult result;
        private Object stateobj;
        private bool GameSaveRequested = false;

        public EditScreen(Game game, SpriteBatch spriteBatch) : base(game, spriteBatch)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;
            brownTemplate = new TileData("Textures/brownBlock", 0, 0, 0, GameConstants.TileSize, GameConstants.TileSize);
            redTemplate = new TileData("Textures/redBlock", 0, 0, 0, GameConstants.TileSize, GameConstants.TileSize);
            greenTemplate = new TileData("Textures/greenBlock", 0, 0, 0, GameConstants.TileSize, GameConstants.TileSize);
            blueTemplate = new TileData("Textures/blueBlock", 0, 0, 0, GameConstants.TileSize, GameConstants.TileSize);
            tileTemplate = brownTemplate;
            tilemap = new List<Tile>();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            handleKey();
            handleClick();
            
            //Check for pause or exit
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
                Vector2 index = GameConversions.positionToIndex(new Vector2(mouseState.X, mouseState.Y));
                foreach (Tile t in tilemap)
                {
                    if (t.blockIndex.X == index.X && t.blockIndex.Y == index.Y)
                    {
                        tilemap.Remove(t);
                        break;
                    }
                }
                tileTemplate.blockX = (int) index.X;
                tileTemplate.blockY = (int) index.Y;
                tilemap.Add(new Tile(game, tileTemplate));
            }
            if (CheckClick("right"))
            {
                Vector2 index = GameConversions.positionToIndex(new Vector2((int)mouseState.X, (int)mouseState.Y));
                foreach (Tile t in tilemap)
                {
                    if (t.blockIndex.X == index.X && t.blockIndex.Y == index.Y)
                    {
                        Console.Out.WriteLine("removing " + t.blockIndex.X + ", " + t.blockIndex.Y);
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
                //return mouseState.LeftButton == ButtonState.Released && oldMouseState.LeftButton == ButtonState.Pressed;
                return mouseState.LeftButton == ButtonState.Pressed;
            }
            else if (button == "right")
            {
                //return mouseState.RightButton == ButtonState.Released && oldMouseState.RightButton == ButtonState.Pressed;
                return mouseState.RightButton == ButtonState.Pressed;
            }
            return false;
        }

        private void saveMap()
        {
            /*
            XElement xel = new XElement("Tilemap-test");
            xel.Add(tiles);
            xel.Save("Tilemap-test.xml");*/

            if ((!Guide.IsVisible) && (GameSaveRequested == false))
            {
                GameSaveRequested = true;
                result = StorageDevice.BeginShowSelector(PlayerIndex.One, null, null);
            }
            

            /*AsyncCallback callback = new AsyncCallback(selectedDevice);
            IAsyncResult result = StorageDevice.BeginShowSelector(callback, null);
            result.AsyncWaitHandle.WaitOne();
            StorageDevice device = StorageDevice.EndShowSelector(result);
            result.AsyncWaitHandle.Close();*/
            
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

        private void doSaveGame(StorageDevice device)
        {
            TileData[] tiles = new TileData[tilemap.Count()];
            List<Tile>.Enumerator it = tilemap.GetEnumerator();
            int cnt = 0;
            while (it.MoveNext())
            {
                Tile t = it.Current;
                tiles[cnt] = new TileData(t.texture.Name, (DataTypes.TileCollision)t.collision, (int)t.blockIndex.X, (int)t.blockIndex.Y, t.width, t.height);
                cnt++;
            }

            Console.Out.WriteLine(tiles[0].texturePath);
            Console.Out.WriteLine(tiles[1].collision);


            IAsyncResult res = device.BeginOpenContainer("StorageDemo", null, null);
            res.AsyncWaitHandle.WaitOne();
            StorageContainer container = device.EndOpenContainer(res);
            res.AsyncWaitHandle.Close();
            String filename = "testtmp2.xml";
            if (!container.FileExists(filename))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<TileData>));
                Stream stream = container.CreateFile(filename);
                Console.Out.WriteLine(container.DisplayName);
                Console.Out.WriteLine(container.ToString());
                serializer.Serialize(stream, tiles);
                stream.Close();
            }
            container.Dispose();
            
            
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            foreach (Tile t in tilemap)
            {
                spriteBatch.Draw(t.texture, t.position, Color.White);
            }
            spriteBatch.DrawString(game.Content.Load<SpriteFont>("menufont"), tileTemplate.texturePath, Vector2.Zero, Color.Black);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

