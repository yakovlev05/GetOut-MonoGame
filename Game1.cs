using System.IO;
using GetOut.Models;
using GetOut.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GetOut;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

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
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        new ContentModel(Content);
        var test = new MazeModel(new DirectoryInfo("./Mazes/initial.txt"));
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.SandyBrown);
        DrawMaze.Draw(_spriteBatch, new MazeModel(new DirectoryInfo("./Mazes/initial.txt")));
        base.Draw(gameTime);
    }
}