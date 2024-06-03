using System;
using GetOut.Program;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace GetOut.Models;

public class TimeStatistic
{
    private readonly DateTime _startTime;
    private TimeSpan _actualTime;
    private readonly BitmapFont _bitmapFont;
    public string TimeInString => $"{_actualTime.Hours:D2}:{_actualTime.Minutes:D2}:{_actualTime.Seconds:D2}";

    public TimeStatistic()
    {
        _startTime = DateTime.Now;
        _bitmapFont = Globals.Content.Load<BitmapFont>("./fonts/OffBit/OffBit");
    }

    public void Update()
    {
        _actualTime = DateTime.Now - _startTime;
    }

    public void Draw()
    {
        Globals.SpriteBatch.DrawString(_bitmapFont, TimeInString,
            new Vector2(1230, 360 + 10),
            Color.White,
            0,
            Vector2.Zero,
            0.35f,
            SpriteEffects.None,
            0);
    }
}