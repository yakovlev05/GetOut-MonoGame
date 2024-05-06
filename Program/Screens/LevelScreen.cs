using GetOut.Controllers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended.ViewportAdapters;

namespace GetOut.Program.Screens;

public class LevelScreen : GameScreen
{
    private new Game1 Game => (Game1)base.Game;
    private TiledMap _tiledMap;
    private TiledMapRenderer _tiledMapRenderer;
    private SpriteBatch _spriteBatch;
    private GameController _gameController;
    private OrthographicCamera _camera;
    private Matrix _matrix;
    private MapController _mapController;

    public LevelScreen(Game game) : base(game)
    {
    }

    public override void Initialize()
    {
        _camera = new OrthographicCamera(new BoxingViewportAdapter(Game.Window, GraphicsDevice, 1920, 1080));
        _camera.Zoom = 3f;
        _matrix = _camera.GetViewMatrix();

        _gameController = new GameController();
        _gameController.Init();

        //-960 -540 край карты на середине
        _camera.Position = new Vector2(-912, -400);
        Globals.Camera1 = _camera;
        base.Initialize();
    }

    public override void LoadContent()
    {
        _tiledMap = Content.Load<TiledMap>("./level1/level1");
        _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _mapController = new MapController(_tiledMap, _camera);
        Globals.MapController = _mapController;
        base.LoadContent();
    }

    public override void Update(GameTime gameTime)
    {
        _tiledMapRenderer.Update(gameTime);
        _gameController.Update();
        _camera.Position += _gameController.Hero.GetDirection(_mapController, _matrix);
    }

    public override void Draw(GameTime gameTime)
    {
        Globals.SpriteBatch = _spriteBatch;
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin(transformMatrix: _matrix, samplerState: SamplerState.PointClamp);
        _tiledMapRenderer.Draw(_camera.GetViewMatrix());
        _gameController.Draw();
        _spriteBatch.End();


        _spriteBatch.Begin();
        foreach (var wall in _mapController.Walls)
        {
            var cord = _camera.WorldToScreen(wall.X, wall.Y);
            _spriteBatch.DrawRectangle(cord.X, cord.Y, 16 * 3, 16 * 3, Color.Red);
        }

        _spriteBatch.End();


        _spriteBatch.Begin(); // Новые рамзеры
        Vector2 cord1 =
            Vector2.Transform(
                new Vector2(_gameController.Hero.StartPosition.X + 50, _gameController.Hero.StartPosition.Y + 37),
                _matrix);


        _spriteBatch.DrawRectangle(cord1.X, cord1.Y, 15 * 3, 43 * 3,
            Color.SandyBrown);
        _spriteBatch.End();
    }
}