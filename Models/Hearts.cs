using GetOut.Controllers;
using GetOut.Program;
using GetOut.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace GetOut.Models;

public class Hearts
{
    public Hero Hero { get; set; }

    private AnimationController Anims { get; init; } = new();
    private readonly BitmapFont _bitmapFont;

    private Vector2 Position { get; set; }
    private int Count { get; set; }
    private int StartCount { get; set; }
    private float WidthHeart => 13.0f * Scale;
    private float HeightHeart => 4.0f * Scale;
    private float ShieldTimeInSeconds { get; init; } // Время невосприимчивосвти к урону после получения урона
    private float LastDamageElapsedTimeInSeconds { get; set; } // Текущее время с последнего урона
    private Vector2 OffsetVector { get; init; }
    private float Scale { get; init; }
    public bool IsDied => Count == 0;
    public bool IsActiveShield => LastDamageElapsedTimeInSeconds <= ShieldTimeInSeconds;
    private bool IsGodMode { get; set; }

    public Hearts(Vector2 position, int count, float scale = 1, Vector2 offsetVector = default,
        int shieldTimeInSeconds = 5)
    {
        Position = position;
        Count = count;
        StartCount = count;
        Scale = scale;
        OffsetVector = offsetVector;
        ShieldTimeInSeconds = shieldTimeInSeconds;
        LastDamageElapsedTimeInSeconds = ShieldTimeInSeconds + 1;

        var texture = Globals.Content.Load<Texture2D>("./Levels/assets/hearts");

        var offsetVectorHorizon = new Vector2(WidthHeart + 3 * Scale, 0);
        for (int i = 0; i < Count; i++)
        {
            Anims.AddAnimation(i,
                new Animation(texture, 1, 3, 0.1f, 2, 1, offsetPosition: offsetVectorHorizon * i + OffsetVector,
                    scale: Scale));
        }

        _bitmapFont = Globals.Content.Load<BitmapFont>("./fonts/OffBit/OffBit");
    }

    public void Update()
    {
        LastDamageElapsedTimeInSeconds += Globals.TotalSeconds;
    }

    public void Draw()
    {
        if (IsGodMode)
        {
            Globals.SpriteBatch.DrawString(
                _bitmapFont,
                $"Здоровье: infinity",
                new Vector2(640 + 10, 360 + 10),
                Color.White,
                0,
                Vector2.Zero,
                0.35f,
                SpriteEffects.None,
                0
            );
            return;
        }

        for (int i = 0; i < Count; i++)
        {
            Anims.DrawFromKey(i, Position); // С 3x зумом  верхний левый угол - 1920/3 : 1080/3
        }
    }

    public void Decrease()
    {
        if (IsGodMode) return;
        if (LastDamageElapsedTimeInSeconds > ShieldTimeInSeconds)
        {
            LastDamageElapsedTimeInSeconds = 0;
            Count--;
        }
    }

    public void Increase()
    {
        if (IsGodMode) return;
        if (Count < StartCount) Count++;
    }

    public void UpdatePosition(Vector2 position)
    {
        Position = position;
    }

    public void SetGodMode(bool isGodMode)
    {
        IsGodMode = isGodMode;
    }
}