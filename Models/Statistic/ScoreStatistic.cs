using GetOut.Program;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace GetOut.Models.Statistic;

public class ScoreStatistic
{
    private readonly BitmapFont _bitmapFont;
    public int TotalScore { get; private set; }

    public ScoreStatistic()
    {
        _bitmapFont = Globals.Content.Load<BitmapFont>("./fonts/OffBit/OffBit");
    }

    public void Draw()
    {
        Globals.SpriteBatch.DrawString(
            _bitmapFont,
            $"Очки: {TotalScore:D5}",
            new Vector2(950, 360 + 10),
            Color.White,
            0,
            Vector2.Zero,
            0.35f,
            SpriteEffects.None,
            0
        );
    }

    public void AddScore(int score)
    {
        TotalScore += score;
    }
}