using GetOut.Controllers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;

namespace GetOut.Program;

public class Game1 : Game
{
    private GameController _gameController;
    private ScreenManager _screenManager;
    private GraphicsDeviceManager _graphics;


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
        Globals.Content = Content;
        _gameController = new();
        _gameController.Init();


        _screenManager = new ScreenManager();
        Components.Add(_screenManager);

        base.Initialize();
    }

    protected override void LoadContent()
    {
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        if (Keyboard.GetState().IsKeyDown(Keys.X)) LoadScreen1();
        if (Keyboard.GetState().IsKeyDown(Keys.C)) LoadScreen2();

        Globals.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.SandyBrown);
        base.Draw(gameTime);
    }

    private void LoadScreen1()
    {
        _screenManager.LoadScreen(new MainMenuScreen(this), new FadeTransition(GraphicsDevice, Color.Black));
    }

    private void LoadScreen2()
    {
        _screenManager.LoadScreen(new LevelScreen(this, _gameController),
            new FadeTransition(GraphicsDevice, Color.Black));
    }
}