using System;
using System.Linq;
using GetOut.Controllers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

namespace GetOut.Program.Screens;

public class VictoryScreen : GameScreen
{
    private new Game1 Game => (Game1)base.Game;

    private TiledMap _tiledMap;
    private TiledMapRenderer _tiledMapRenderer;
    private SpriteBatch _spriteBatch;
    private BitmapFont _bitmapFont;

    private TiledMapTileLayer _okButtonActive;

    private RectangleF _okButtonRectangle;

    private Matrix _matrix;

    private string _spentTime;

    public VictoryScreen(Game game, string spentTime) : base(game)
    {
        _spentTime = spentTime;
    }

    public override void LoadContent()
    {
        _tiledMap = Content.Load<TiledMap>("./VictoryScreen/VictoryScreen");
        _bitmapFont = Globals.Content.Load<BitmapFont>("./fonts/OffBit/OffBit");
        _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _okButtonActive = _tiledMap.GetLayer<TiledMapTileLayer>("ok_button_active");

        // Делаем расчёт для преобразования размера карты из 1280*720 в 1920*1080
        var viewport = GraphicsDevice.Viewport;
        var scaleX = (float)viewport.Width / _tiledMap.Width;
        var scaleY = (float)viewport.Height / _tiledMap.Height;
        scaleX = Math.Min(scaleX, 1.5f);
        scaleY = Math.Min(scaleY, 1.5f);
        _matrix = Matrix.CreateScale(scaleX, scaleY, 1);

        var tileButton = _okButtonActive.Tiles.First(x => x.GlobalIdentifier != 0);
        var buttonVector = Vector2.Transform(new Vector2(tileButton.X * 16, tileButton.Y * 16), _matrix);
        _okButtonRectangle = new RectangleF(buttonVector.X, buttonVector.Y,
            226 * _matrix.M11, // Нестандарнтый размер, поэтому так
            128 * _matrix.M22);

        base.LoadContent();
    }

    public override void Update(GameTime gameTime)
    {
        _tiledMapRenderer.Update(gameTime);

        if (InputController.IsPressedKey(Keys.Escape)) Game.LoadLevelMenuScreen();

        if (InputController.IsMouseInRectangle(_okButtonRectangle))
        {
            if (InputController.IsLeftButtonPressed()) Game.LoadLevelMenuScreen();
            _okButtonActive.IsVisible = true;
        }
        else _okButtonActive.IsVisible = false;
    }

    public override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _tiledMapRenderer.Draw(_matrix);
        // _spriteBatch.DrawRectangle(_okButtonRectangle, Color.Red);
        _spriteBatch.DrawString(_bitmapFont,
            $"Затраченное время: {_spentTime}",
            new Vector2(700, 400),
            Color.White, 0,
            Vector2.Zero,
            1f,
            SpriteEffects.None,
            0);
        _spriteBatch.End();
    }
}