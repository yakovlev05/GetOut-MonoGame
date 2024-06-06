using System.Collections.Generic;
using GetOut.Controllers;
using GetOut.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Screens;

namespace GetOut.Program.Screens;

public class InfoLevelScreen : GameScreen
{
    private new Game1 Game => (Game1)base.Game;
    private BitmapFont _bitmapFont;
    private SpriteBatch _spriteBatch;
    private string _mapPath;
    private List<IEntityInterface> _entities;

    public InfoLevelScreen(Game game, string mapPath, List<IEntityInterface> entities) : base(game)
    {
        _mapPath = mapPath;
        _entities = entities;
        _bitmapFont = Globals.Content.Load<BitmapFont>("./fonts/OffBit/OffBit");
        _spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    public override void Update(GameTime gameTime)
    {
        if (InputController.GetPressedKeys().Length > 0) Game.LoadLevelScreen(_mapPath, _entities);
    }

    public override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        
        _spriteBatch.Begin();
        _spriteBatch.DrawString(_bitmapFont, "Помоги рыцарю выбраться из лабиринта", new Vector2(660,200), Color.White);
        _spriteBatch.DrawString(_bitmapFont, "В лабиринте вам придётся встретиться с монстрами, различными ловушками", new Vector2(480,400), Color.White);
        _spriteBatch.DrawString(_bitmapFont, "Используйте WASD для перемещения", new Vector2(700,480), Color.White);
        _spriteBatch.DrawString(_bitmapFont, "SPACE для атаки", new Vector2(830,520), Color.White);
        _spriteBatch.DrawString(_bitmapFont, "ESC - выход из игры", new Vector2(810,560), Color.White);
        _spriteBatch.DrawString(_bitmapFont, "Сражайтесь с монстрами, преодолевайте препятствия и найдите выход", new Vector2(500,640), Color.White);
        _spriteBatch.DrawString(_bitmapFont, "Если вы тестируете игру, то нажмите GOD для включения неуязвимости", new Vector2(500,960), Color.White);
        _spriteBatch.DrawString(_bitmapFont, "Для продолжения нажмите любую клавишу", new Vector2(700,1000), Color.White);
        _spriteBatch.End();
    }
}