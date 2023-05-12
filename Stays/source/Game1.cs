using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using platformerYT.src;
using System.Collections.Generic;
using TiledSharp;

namespace Stays.source
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        #region Tilemaps
        private TmxMap _map;
        private TilemapManager _tilemapManager;
        private Texture2D tileset;
        private List<Rectangle> collisionsRects;
        private Rectangle startPoint;
        private Rectangle endPoint;
        #endregion Tilemaps

        #region Player
        private Player _player;
        #endregion Player
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            #region TileMap
            _map = new TmxMap("Content\\Level1.tmx");    // загрузка уровня
            tileset = Content.Load<Texture2D>("Cave Tileset\\" + _map.Tilesets[0].Name.ToString());    // загрузка плиток

            int tileWidth = _map.Tilesets[0].TileWidth;
            int tileHeight = _map.Tilesets[0].TileHeight;
            int tilesetTileWidth = tileset.Width / tileWidth;

            _tilemapManager = new TilemapManager(_map, tileset, tilesetTileWidth, tileWidth, tileHeight, GraphicsDevice, _spriteBatch);
            #endregion TileMap

            collisionsRects = new List<Rectangle>();
            foreach (var obj in _map.ObjectGroups["collisions"].Objects) 
            {
                if (obj.Name == "")
                {
                    collisionsRects.Add(new Rectangle((int)obj.X, (int)obj.Y, (int)obj.Width, (int)obj.Height));
                }

            }


            #region Player
            _player = new Player(
                Content.Load<Texture2D>("Sprite Pack 4\\1 - Agent_Mike_Idle (32 x 32)"),
                Content.Load<Texture2D>("Sprite Pack 4\\1 - Agent_Mike_Running (32 x 32)")     // два спрайта-состояния модельки гг
                ); 
        }
            #endregion Player

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            var initPos = _player.position;
            _player.Update();
            foreach (var rectangle in collisionsRects)
            {
                if (rectangle.Intersects(_player.hitbox))
                {
                    _player.position = initPos;
                }
            }
            _player.Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            _tilemapManager.Draw(_spriteBatch);
            _player.Draw(_spriteBatch, gameTime);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}