using System.IO;
using GetOut.Controllers;
using GetOut.Models;
using GetOut.Program;
using GetOut.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;
using MonoGame.Extended.ViewportAdapters;

namespace GetOut;

public class Game1 : Game
{
    private ScreenManager _screenManager;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private OrthographicCamera _camera;

    //
    private PlayerModel _player;
    private PlayerController _playerController;
    private MazeModel _mazeModel;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        // _graphics.PreferredBackBufferHeight = 1080;
        // _graphics.PreferredBackBufferWidth = 1920;
        _graphics.IsFullScreen = true;
        // _graphics.PreferMultiSampling = true; // Сглажение краёв
        _graphics.ApplyChanges();
        // Window.IsBorderless = true;
        // Window.AllowUserResizing = true;

        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _screenManager = new ScreenManager();
        Components.Add(_screenManager);

        _camera = new OrthographicCamera(new BoxingViewportAdapter(Window, GraphicsDevice, 1920, 1080));

        // LoadScreen1();
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        new ContentModel(Content);
        _mazeModel = new MazeModel(new DirectoryInfo("./Mazes/initial.txt"));

        _player = new PlayerModel(ContentModel.Textures["mazeVariant1"]["player"], new Vector2(0, 0),
            new Rectangle(0, 0, 16, 28));
        _playerController =
            new PlayerController(ContentModel.Textures["mazeVariant1"]["player"], 5, _player, 100, _mazeModel);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        if (Keyboard.GetState().IsKeyDown(Keys.X))
        {
            LoadScreen1();
        }

        _camera.Move(Camera.GetMovementDirection() * 20);
        // _playerController.Move(Keyboard.GetState());
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.SandyBrown);
        DrawMaze.Draw(_spriteBatch, _mazeModel, _camera);
        DrawPlayer.Draw(_spriteBatch, _player);
        base.Draw(gameTime);
    }

    private void LoadScreen1()
    {
        _screenManager.LoadScreen(new MainMenuScreen(this), new FadeTransition(GraphicsDevice, Color.Black));
    }
}