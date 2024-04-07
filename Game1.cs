using System;
using System.IO;
using System.Linq;
using GetOut.Models;
using GetOut.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GetOut;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Content _content;
    private string[] _namesMazes;
    private string[,] _currentMaze;

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
        // TODO: Add your initialization logic here
        // GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _content = new Content(Content); //
        _namesMazes = MazeLoader.LoadNamesFromFolder(new DirectoryInfo("../../../Mazes")).ToArray(); //
        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        _currentMaze =
            MazeToGraphic.ConvertMazeToGraphic(MazeLoader.LoadMazeFromFolder(new DirectoryInfo("../../../Mazes"),
                _namesMazes.First())); //
        View.Draw.DrawMaze(_graphics, _spriteBatch, _currentMaze, _content); // ТЕСТ
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        View.Draw.DrawMaze(_graphics,
            _spriteBatch,
            _currentMaze,
            _content);

        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }
}