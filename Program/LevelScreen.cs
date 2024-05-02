using System;
using GetOut.Controllers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended.ViewportAdapters;

namespace GetOut.Program;

public class LevelScreen : GameScreen
{
    private new Game1 Game => (Game1)base.Game;
    private TiledMap _tiledMap;
    private TiledMapRenderer _tiledMapRenderer;
    private SpriteBatch _spriteBatch;
    private GameManager _gameManager;
    private OrthographicCamera _camera;
    private Matrix _matrix;
    private MapController _mapController;

    public LevelScreen(Game game, GameManager gameManager) : base(game)
    {
        _gameManager = gameManager;
    }

    public override void Initialize()
    {
        _camera = new OrthographicCamera(new BoxingViewportAdapter(Game.Window, GraphicsDevice, 1920, 1080));

        _camera.Zoom = 1f;
        _matrix = _camera.GetViewMatrix();
        _matrix = _camera.GetViewMatrix();
        //-960 -540 край карты на середине
        _camera.Position = new Vector2(-912, -460);
        Globals.Camera1 = _camera;
        base.Initialize();
    }

    public override void LoadContent()
    {
        _tiledMap = Content.Load<TiledMap>("./level1/level1");
        _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _mapController = new MapController(_tiledMap);
        Globals.MapController = _mapController;
        base.LoadContent();
        Globals.Camera1 = _camera;
    }

    public override void Update(GameTime gameTime)
    {
        _tiledMapRenderer.Update(gameTime);

        Globals.Camera1 = _camera;
        _camera.Position += _gameManager.Hero.GetDirection(_mapController);
        Globals.Camera1 = _camera;

        Console.WriteLine($"ИГРОК {_gameManager.Hero.Position}");
        Console.WriteLine($"ИГРОК1 {_camera.ScreenToWorld(_gameManager.Hero.Position)}");
    }

    public override void Draw(GameTime gameTime)
    {
        Globals.SpriteBatch = _spriteBatch;
        GraphicsDevice.Clear(Color.Black);
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _tiledMapRenderer.Draw(_camera.GetViewMatrix());
        _spriteBatch.End();

        _spriteBatch.Begin(transformMatrix: _matrix, samplerState: SamplerState.PointClamp);
        _gameManager.Draw();
        _spriteBatch.End();


        _spriteBatch.Begin();
        foreach (var wall in _mapController.Walls1)
        {
            var cord = _camera.WorldToScreen(wall.X, wall.Y);
            // if (wall.X == 16 * 29)
            // {
            //     Console.WriteLine();
            // }

            _spriteBatch.DrawRectangle(cord.X, cord.Y, 16, 16, Color.Red);
        }

        _spriteBatch.DrawRectangle(_gameManager.Hero.StartPosition.X, _gameManager.Hero.StartPosition.Y, 120, 80,
            Color.SandyBrown);
        // foreach (var wall in _mapController.Walls1)
        // {                        
        //     var cord = wall;
        //     _spriteBatch.DrawRectangle(cord.X, cord.Y, 16, 16, Color.Red);
        // }
        _spriteBatch.End();
        Globals.Camera1 = _camera;
    }
}