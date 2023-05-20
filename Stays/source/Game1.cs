using Apos.Gui;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Stays.src;
using System;
using System.Collections.Generic;
using TiledSharp;

namespace Stays.source
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private RenderTarget2D _renderTarget;

        public static float screenWidth;
        public static float screenHeight;

        #region UI
        IMGUI _ui;
        Texture2D coinIcon;
        #endregion UI

        #region Managers
        private GameManager _gameManager;
        //private bool isGameOver = false;
        #endregion Managers

        #region Tilemaps
        private TmxMap _map;
        private TilemapManager _tilemapManager;
        private Texture2D _tileset;
        private List<Rectangle> _collisionRects;
        private Rectangle _startPoint;
        private Rectangle _endPoint;
        #endregion Tilemaps

        #region Enemy
        private Enemy _martian;
        private List<Enemy> _enemies;
        private List<Rectangle> _enemyPathWays;
        #endregion Enemy

        #region Camera
        private Camera _camera;
        private Matrix transformMatrix;
        #endregion Camera

        #region Player
        private Player _player;
        private List<Bullet> _bullets;
        private Texture2D _bulletTexture;
        private int _time_between_bullets;
        private int _points = 0;
        private int _playerHealth = 10;
        private int _timeBetweenHurt = 20;
        private int _hitCounter = 0;

        #endregion Player
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferHeight = 850;
            _graphics.PreferredBackBufferWidth = 1024;
            _graphics.ApplyChanges();
            screenHeight = _graphics.PreferredBackBufferHeight;
            screenWidth = _graphics.PreferredBackBufferWidth;
            base.Initialize();
        }
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //#region UI
            //FontSystem fontSystem = FontSystemFactory.Create(GraphicsDevice, 2048, 2048);
            //fontSystem.AddFont(TitleContainer.OpenStream($"{Content.RootDirectory}/dogicapixel.ttf"));
            //GuiHelper.Setup(this, fontSystem);
            //_ui = new IMGUI();
            //#endregion UI

            #region TileMap
            _map = new TmxMap("Content\\Level1.tmx");    // загрузка уровня
            _tileset = Content.Load<Texture2D>("Cave Tileset\\" + _map.Tilesets[0].Name.ToString());    // загрузка плиток

            int tileWidth = _map.Tilesets[0].TileWidth;
            int tileHeight = _map.Tilesets[0].TileHeight;
            int tilesetTileWidth = _tileset.Width / tileWidth;

            _tilemapManager = new TilemapManager(_map, _tileset, tilesetTileWidth, tileWidth, tileHeight, GraphicsDevice, _spriteBatch);
            #endregion TileMap

            #region collision
            _collisionRects = new List<Rectangle>();
            foreach (var obj in _map.ObjectGroups["collisions"].Objects) // добавление коллизий
            {
                if (obj.Name == "")
                {
                    _collisionRects.Add(new Rectangle((int)obj.X, (int)obj.Y, (int)obj.Width, (int)obj.Height));
                }
                if (obj.Name == "Start")
                {
                    _startPoint = new Rectangle((int)obj.X, (int)obj.Y, (int)obj.Width, (int)obj.Height);
                }
                if (obj.Name == "End")
                {
                    _endPoint = new Rectangle((int)obj.X, (int)obj.Y, (int)obj.Width, (int)obj.Height);
                }
            }
            #endregion collision

            _gameManager = new GameManager(_endPoint);

            #region Player
            _player = new Player(
                new Vector2(_startPoint.X, _startPoint.Y),
                Content.Load<Texture2D>("Sprite Pack 4\\1 - Agent_Mike_Idle (32 x 32)"),
                Content.Load<Texture2D>("Sprite Pack 4\\1 - Agent_Mike_Running (32 x 32)"),     // спрайты-состояния модельки гг
                Content.Load<Texture2D>("Sprite Pack 4\\Agent_Mike_Jump"),
                Content.Load<Texture2D>("Sprite Pack 4\\Agent_Mike_Falling")
                );
            #region Bullet
            _bullets = new List<Bullet>();
            _bulletTexture = Content.Load<Texture2D>("Sprite Pack 4\\1 - Agent_Mike_Bullet (16 x 16)"); //  спрайт пульки
            #endregion

            #endregion Player

            #region Camera
            _camera = new Camera();
            #endregion

            #region Enemy
            _enemyPathWays = new List<Rectangle>();
            foreach (var o in _map.ObjectGroups["EnemyPathWays"].Objects)  // добавление путей енеми
            {
                _enemyPathWays.Add(new Rectangle((int)o.X, (int)o.Y, (int)o.Width, (int)o.Height));
            }
            _enemies = new List<Enemy>();
            _martian = new Enemy(
               Content.Load<Texture2D>("Sprite Pack 4\\2 - Martian_Red_Running (32 x 32)"),
               _enemyPathWays[0]
                );

            _enemies.Add(_martian);
            _martian = new Enemy(
               Content.Load<Texture2D>("Sprite Pack 4\\2 - Martian_Red_Running (32 x 32)"),
               _enemyPathWays[1]
                );
            _enemies.Add(_martian);
            #endregion

            _renderTarget = new RenderTarget2D(GraphicsDevice, 1024, 800);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            #region UI
            #endregion UI

            #region Enemy
            foreach (var enemy in _enemies)
            {
                enemy.Update();
                if (enemy.hasHit(_player.hitbox))
                {
                    _hitCounter++;
                    if (_hitCounter > _timeBetweenHurt)
                    {
                        _playerHealth--;
                        _hitCounter = 0;
                    }
                }
            }
            #endregion Enemy

            #region Camera update
            Rectangle target = new Rectangle((int)_player.position.X, (int)_player.position.Y, 32, 32);
            transformMatrix = _camera.Follow(target);
            #endregion

            #region Managers 
            if (_gameManager.isGameEnded(_player.hitbox))
            {
                Console.WriteLine("GAME OVER");
            }
            if (_playerHealth <= 0)
            {
                Console.WriteLine("YPU DIED!");
            }
            Console.WriteLine($"Health: {_playerHealth}");
            #endregion Managers

            #region Bullet

            if (_player.isShooting)
            {
                if (_time_between_bullets > 5 && _bullets.ToArray().Length < 20)
                {
                    var temp_hitbox = new
                                Rectangle((int)_player.position.X + 7,
                                          (int)_player.position.Y + 15,
                                          _bulletTexture.Width,
                                          _bulletTexture.Height);
                    if (_player.effects == SpriteEffects.None)
                    {

                        _bullets.Add(new Bullet(_bulletTexture, 4, temp_hitbox));
                    }
                    if (_player.effects == SpriteEffects.FlipHorizontally)
                    {

                        _bullets.Add(new Bullet(_bulletTexture, -4, temp_hitbox));
                    }
                    _time_between_bullets = 0;
                }
                else
                {
                    _time_between_bullets++;
                }

            }

            foreach (var bullet in _bullets.ToArray())
            {
                bullet.Update();

                foreach (var el in _collisionRects)
                {
                    if (el.Intersects(bullet.hitbox))
                    {
                        _bullets.Remove(bullet);
                        break;
                    }
                }
                foreach (var enemy in _enemies.ToArray())
                {
                    if (bullet.hitbox.Intersects(enemy.hitbox))
                    {
                        _bullets.Remove(bullet);
                       _enemies.Remove(enemy);
                        _points++;
                        break;
                    }
                }
            }

            #endregion

            Console.WriteLine("Points: " + _points);
            #region Player Collisions
            var initPos = _player.position;
            _player.Update();
            //   Oy

            foreach (var el in _collisionRects)
            {
                if (!_player.isJumping)
                    _player.isFalling = true;
                if (el.Intersects(_player.playerFallRect))
                {
                    _player.isFalling = false;
                    break;
                }
            }


            //   Ox
            foreach (var el in _collisionRects)
            {
                if (el.Intersects(_player.hitbox))
                {
                    _player.position = initPos;
                    _player.velocity = initPos;
                    break;
                }
            }
            #endregion

            //#region UI
            //GuiHelper.UpdateSetup(gameTime);
            //_ui.UpdateAll(gameTime);

            //Panel.Push().XY = new Vector2(10, 10);

            //Label.Put($"Points: {_points}");
            //Panel.Pop();
            //Panel.Push().XY = new Vector2(10, 50);

            //Label.Put($"Health: {_playerHealth}");
            //Panel.Pop();
            //GuiHelper.UpdateCleanup();
            //#endregion

            DrawLevel(gameTime);
            base.Update(gameTime);
        }

        public void DrawLevel(GameTime gameTime)
        {

            GraphicsDevice.SetRenderTarget(_renderTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(transformMatrix: transformMatrix);
            _tilemapManager.Draw(_spriteBatch);

            #region Enemy
            foreach (var enemy in _enemies)
            {
                enemy.Draw(_spriteBatch, gameTime);
            }
            #endregion

            #region Player


            #region Bullets

            foreach (var bullet in _bullets.ToArray())
            {
                bullet.Draw(_spriteBatch);
            }
            #endregion

            _player.Draw(_spriteBatch, gameTime);

            #endregion

            _spriteBatch.End();
            GraphicsDevice.SetRenderTarget(null);

        }
        protected override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _spriteBatch.Draw(_renderTarget, new Vector2(0, 0), null, Color.White, 0f, new Vector2(), 2f, SpriteEffects.None, 0);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}