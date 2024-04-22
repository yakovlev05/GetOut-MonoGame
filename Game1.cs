using System.IO;
using GetOut.Controllers;
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

    //
    private PlayerModel _player;
    private PlayerController _playerController;

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

        _player = new PlayerModel(ContentModel.Textures["mazeVariant1"]["player"], new Vector2(0, 0),
            new Rectangle(0, 0, 16, 28));
        _playerController = new PlayerController(ContentModel.Textures["mazeVariant1"]["player"], 5, _player, 100);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        _playerController.Move(Keyboard.GetState());
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.SandyBrown);
        DrawMaze.Draw(_spriteBatch, new MazeModel(new DirectoryInfo("./Mazes/initial.txt")));
        DrawPlayer.Draw(_spriteBatch, _player);
        base.Draw(gameTime);
    }
}