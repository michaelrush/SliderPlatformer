using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Media;

namespace SlidingBlockPlatformer
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Tilemap
    {
        public List<Tile> tiles = new List<Tile>();
        private ContentManager content;
        private List<DataTypes.TileData> tilemap;
        private IAsyncResult asyncResult;
        private StorageDevice storageDevice;
        private StorageContainer storageContainer;
        private PlayerIndex playerIndex = PlayerIndex.One;
        private string filename = "";

        enum LoadingState
        {
            NotLoading,
            ReadyToSelectStorageDevice,
            SelectingStorageDevice,

            ReadyToOpenStorageContainer,    // once we have a storage device start here
            OpeningStorageContainer,
            ReadyToLoad
        }
        LoadingState loadingState = LoadingState.NotLoading;


        public Tilemap(IServiceProvider serviceProvider, String levelIndex)
        {
            content = new ContentManager(serviceProvider, "Content");
            filename = "Tilemaps/Level-" + levelIndex + ".xml";

            loadingState = LoadingState.ReadyToSelectStorageDevice;
            /*
            List<DataTypes.TileData> tiledata = null;// content.Load<List<DataTypes.TileData>>(filename);
             */
        }

        public void Update(GameTime gameTime)
        {
            UpdateLoading();
        }

        /// <summary>
        /// Transitions the SavingState through the saving process. 
        /// Handles selecting the storage device, opening the storage container, and executing the save function
        /// </summary>
        private void UpdateLoading()
        {
            switch (loadingState)
            {
                case LoadingState.ReadyToSelectStorageDevice:
#if XBOX
                    if (!Guide.IsVisible)
#endif
                    {
                        asyncResult = StorageDevice.BeginShowSelector(playerIndex, null, null);
                        loadingState = LoadingState.SelectingStorageDevice;
                    }
                    break;

                case LoadingState.SelectingStorageDevice:
                    if (asyncResult.IsCompleted)
                    {
                        storageDevice = StorageDevice.EndShowSelector(asyncResult);
                        loadingState = LoadingState.ReadyToOpenStorageContainer;
                    }
                    break;

                case LoadingState.ReadyToOpenStorageContainer:
                    if (storageDevice == null || !storageDevice.IsConnected)
                    {
                        loadingState = LoadingState.ReadyToSelectStorageDevice;
                    }
                    else
                    {
                        asyncResult = storageDevice.BeginOpenContainer("EditorStorageContainer", null, null);
                        loadingState = LoadingState.OpeningStorageContainer;
                    }
                    break;

                case LoadingState.OpeningStorageContainer:
                    if (asyncResult.IsCompleted)
                    {
                        storageContainer = storageDevice.EndOpenContainer(asyncResult);
                        loadingState = LoadingState.ReadyToLoad;
                    }
                    break;

                case LoadingState.ReadyToLoad:
                    if (storageContainer == null)
                    {
                        loadingState = LoadingState.ReadyToOpenStorageContainer;
                    }
                    else
                    {
                        try
                        {
                            Load(filename);
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
                            loadingState = LoadingState.NotLoading;
                        }
                    }
                    break;
            }
        }

        private void Load(string filename)
        {            
            // Open the file
            using (Stream stream = storageContainer.OpenFile(filename, FileMode.Open))
            {
                // Read the data from the file
                XmlSerializer serializer = new XmlSerializer(typeof(List<DataTypes.TileData>));
                List<DataTypes.TileData> tiledata = (List<DataTypes.TileData>)serializer.Deserialize(stream);
                
                for (int i = 0; i < tiledata.Count; i++)
                {
                    tiles.Add(new Tile(new Vector2(tiledata[i].posX, tiledata[i].posY), content.Load<Texture2D>(tiledata[i].texturePath), tiledata[i].collision));
                }
            }
        }
    }
}
