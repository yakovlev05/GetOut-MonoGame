using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

namespace GetOut.Program;

public class MainMenuScreen : GameScreen
{
    private new Game1 Game => (Game1) base.Game;
    private TiledMap _tiledMap;
    private TiledMapRenderer _tiledMapRenderer;
    private SpriteBatch _spriteBatch;

    public MainMenuScreen(Game game) : base(game)
    {
    }

    public override void LoadContent()
    {
        _tiledMap = Content.Load<TiledMap>("./MainMenuScreen/mainmenu");
        _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);

        _spriteBatch = new SpriteBatch(GraphicsDevice);
        base.LoadContent();
    }

    public override void Update(GameTime gameTime)
    {
        _tiledMapRenderer.Update(gameTime);
        var button = _tiledMap.GetLayer<TiledMapLayer>("play_button_active");
        button.IsVisible = true;
    }

    public override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        var viewport = GraphicsDevice.Viewport;
        var scaleX = (float)viewport.Width / _tiledMap.Width;
        var scaleY = (float)viewport.Height / _tiledMap.Height;
        
        scaleX = Math.Min(scaleX, 1.5f);
        scaleY = Math.Min(scaleY, 1.5f);
        
        var scaleMatrix = Matrix.CreateScale(scaleX, scaleY, 1);
        
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _tiledMapRenderer.Draw(scaleMatrix);
        _spriteBatch.End();
    }
}