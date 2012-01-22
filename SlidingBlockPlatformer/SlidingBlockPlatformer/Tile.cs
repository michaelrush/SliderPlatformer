using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using DataTypes;

namespace SlidingBlockPlatformer
{

    public struct Tile
    {
        public Texture2D texture;
        public TileCollision collision;

        public int width;
        public int height;
        public Vector2 position;
        public Vector2 blockIndex;
        
        public Tile(Texture2D texture, TileCollision collision, Vector2 position)
        {
            this.texture = texture;
            this.collision = collision;
            this.position = position;
            this.blockIndex = GameConversions.positionToIndex(position);
           
            width = GameConstants.TileSize;
            height = GameConstants.TileSize;
        }

        public Tile(Game game, TileData td)
        {
            this.texture = game.Content.Load<Texture2D>(td.texturePath);
            this.texture.Name = td.texturePath;
            this.collision = td.collision;
            this.position = GameConversions.indexToPosition(new Vector2(td.blockX, td.blockY));
            this.blockIndex = new Vector2(td.blockX, td.blockY);
            this.width = td.width;
            this.height = td.height;
        }
    }
}
