using System;
using System.Collections.Generic;
using GetOut.Controllers;
using GetOut.Models;
using GetOut.Program.Screens;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;

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

    public void LoadMainMenuScreen() =>
        _screenManager.LoadScreen(new MainMenuScreen(this), new FadeTransition(GraphicsDevice, Color.Black));

    public void LoadLevelMenuScreen() =>
        _screenManager.LoadScreen(new LevelMenuScreen(this), new FadeTransition(GraphicsDevice, Color.Black));


    public void LoadVictoryScreen(string spentTime) =>
        _screenManager.LoadScreen(new VictoryScreen(this, spentTime), new FadeTransition(GraphicsDevice, Color.Black));

    public void LoadGameOverScreen() =>
        _screenManager.LoadScreen(new GameOverScreen(this), new FadeTransition(GraphicsDevice, Color.Black));

    public void LoadLevelScreen(string mapPath,  List<IEntityInterface> entities)
    {
        _screenManager.LoadScreen(new LevelScreen(this, mapPath, new GameController(entities)),
            new FadeTransition(GraphicsDevice, Color.Black));
    }
}