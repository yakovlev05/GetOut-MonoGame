﻿using System.IO;
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
    private GameManager _gameManager;

    private ScreenManager _screenManager;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private OrthographicCamera _camera;

    //
    private MazeModel _mazeModel;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferHeight = 1080;
        _graphics.PreferredBackBufferWidth = 1920;
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
        Globals.Content = Content;
        _gameManager = new();
        _gameManager.Init();

        GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;

        _screenManager = new ScreenManager();
        Components.Add(_screenManager);

        _camera = new OrthographicCamera(new BoxingViewportAdapter(Window, GraphicsDevice, 1920, 1080));

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        new ContentModel(Content);
        _mazeModel = new MazeModel(new DirectoryInfo("./Mazes/initial.txt"));


        Globals.SpriteBatch = _spriteBatch;
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

        Globals.Update(gameTime);
        _gameManager.Update();
        // _camera.Move(Camera.GetMovementDirection() * 20);
        // _playerController.Move(Keyboard.GetState());
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.SandyBrown);
        DrawMaze.Draw(_spriteBatch, _mazeModel, _camera);
        // DrawPlayer.Draw(_spriteBatch, _player);

        _spriteBatch.Begin();
        _gameManager.Draw();
        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void LoadScreen1()
    {
        _screenManager.LoadScreen(new MainMenuScreen(this), new FadeTransition(GraphicsDevice, Color.Black));
    }
}