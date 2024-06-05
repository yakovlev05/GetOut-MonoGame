using System;
using System.Linq;
using GetOut.Controllers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

namespace GetOut.Program.Screens;

public class MainMenuScreen : GameScreen
{
    private new Game1 Game => (Game1)base.Game;
    private TiledMap _tiledMap;
    private TiledMapRenderer _tiledMapRenderer;
    private SpriteBatch _spriteBatch;
    private BitmapFont _bitmapFont;

    private TiledMapTileLayer _playButtonActive;
    private TiledMapTileLayer _exitButtonActive;

    private RectangleF _playButtonRectangle;
    private RectangleF _exitButtonRectangle;

    private Matrix _matrix;

    public MainMenuScreen(Game game) : base(game)
    {
    }

    public override void LoadContent()
    {
        _tiledMap = Content.Load<TiledMap>("./MainMenuScreen/mainmenu");
        _bitmapFont = Content.Load<BitmapFont>("./fonts/OffBit/OffBit");
        _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _playButtonActive = _tiledMap.GetLayer<TiledMapTileLayer>("play_button_active");
        _exitButtonActive = _tiledMap.GetLayer<TiledMapTileLayer>("exit_button_active");

        // Делаем расчёт для преобразования размера карты из 1280*720 в 1920*1080
        var viewport = GraphicsDevice.Viewport;
        var scaleX = (float)viewport.Width / _tiledMap.Width;
        var scaleY = (float)viewport.Height / _tiledMap.Height;
        scaleX = Math.Min(scaleX, 1.5f);
        scaleY = Math.Min(scaleY, 1.5f);
        _matrix = Matrix.CreateScale(scaleX, scaleY, 1);

        var tileButton = _playButtonActive.Tiles.First(x => x.GlobalIdentifier != 0);
        var buttonVector = Vector2.Transform(new Vector2(tileButton.X * 16, tileButton.Y * 16), _matrix);
        _playButtonRectangle = new RectangleF(buttonVector.X, buttonVector.Y,
            320 * _matrix.M11, // Нестандарнтый размер, поэтому так
            122 * _matrix.M22);

        tileButton = _exitButtonActive.Tiles.First(x => x.GlobalIdentifier != 0);
        buttonVector = Vector2.Transform(new Vector2(tileButton.X * 16, tileButton.Y * 16), _matrix);
        _exitButtonRectangle = new RectangleF(buttonVector.X, buttonVector.Y,
            320 * _matrix.M11, // Нестандарнтый размер, поэтому так
            122 * _matrix.M22);

        base.LoadContent();
    }

    public override void Update(GameTime gameTime)
    {
        _tiledMapRenderer.Update(gameTime);

        if (InputController.IsPressedKey(Keys.Escape)) Game.Exit();

        if (InputController.IsMouseInRectangle(_playButtonRectangle))
        {
            if (InputController.IsLeftButtonPressed()) Game.LoadLevelMenuScreen();
            _playButtonActive.IsVisible = true;
        }
        else _playButtonActive.IsVisible = false;


        if (InputController.IsMouseInRectangle(_exitButtonRectangle))
        {
            if (InputController.IsLeftButtonPressed()) Game.Exit();
            _exitButtonActive.IsVisible = true;
        }
        else _exitButtonActive.IsVisible = false;
    }

    public override void Initialize()
    {
        Game.IsMouseVisible = true;
        base.Initialize();
    }

    public override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);


        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _tiledMapRenderer.Draw(_matrix);
        _spriteBatch.DrawString(_bitmapFont, "github.com/yakovlev05/GetOut-MonoGame",
            new Vector2(1450, 1000),
            Color.White,
            0,
            Vector2.Zero,
            0.8f,
            SpriteEffects.None,
            0);
        _spriteBatch.End();
    }
}