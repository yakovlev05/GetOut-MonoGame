using GetOut.Controllers;
using GetOut.Program.Screens;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

namespace GetOut.Program;

public class Game1 : Game
{
    private ScreenManager _screenManager;
    private readonly GraphicsDeviceManager _graphics;


    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferHeight = 1080;
        _graphics.PreferredBackBufferWidth = 1920;
        _graphics.IsFullScreen = true;
        _graphics.ApplyChanges();

        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        Globals.Window = Window;
        Globals.Content = Content;
        Globals.GraphicsDevice = GraphicsDevice;

        _screenManager = new ScreenManager();
        Components.Add(_screenManager);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        LoadMainMenuScreen();
    }

    protected override void Update(GameTime gameTime)
    {
        InputController.Update();
        Globals.Update(gameTime);

        // if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
        //     Keyboard.GetState().IsKeyDown(Keys.Escape))
        //     Exit();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
    }

    public void LoadMainMenuScreen()
    {
        _screenManager.LoadScreen(new MainMenuScreen(this), new FadeTransition(GraphicsDevice, Color.Black));
    }

    public void LoadLevelMenuScreen()
    {
        _screenManager.LoadScreen(new LevelMenuScreen(this), new FadeTransition(GraphicsDevice, Color.Black));
    }

    public void LoadLevelScreenTest()
    {
        _screenManager.LoadScreen(new LevelScreenTest(this, "./level1/level1", new GameController()), new FadeTransition(GraphicsDevice, Color.Black));
    }
}