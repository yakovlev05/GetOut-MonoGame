using GetOut.Controllers;
using GetOut.Program;
using GetOut.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GetOut.Models;

public class Hearts
{
    public Hero Hero { get; set; }
    private AnimationController Anims { get; init; } = new();
    public Vector2 Position { get; private set; }
    private int Count { get; set; }
    private int StartCount { get; set; }
    private float WidthHeart => 13.0f * Scale;
    private float HeightHeart => 4.0f * Scale;
    private float ShieldTimeInSeconds => 5; // Время невосприимчивосвти к урону после получения урона
    private float LastDamageElapsedTimeInSeconds { get; set; } = 6; // Текущее время с последнего урона
    public bool IsDied => Count == 0;
    public float Scale { get; init; }
    public Vector2 OffsetVector { get; init; }

    public Hearts(Vector2 position, int count, float scale = 1, Vector2 offsetVector = default)
    {
        Position = position;
        Count = count;
        StartCount = count;
        Scale = scale;
        OffsetVector = offsetVector;

        var texture = Globals.Content.Load<Texture2D>("./Levels/assets/hearts");

        var offsetVectorHorizon = new Vector2(WidthHeart + 3 * Scale, 0);
        for (int i = 0; i < Count; i++)
        {
            Anims.AddAnimation(i,
                new Animation(texture, 1, 3, 0.1f, 2, 1, offsetPosition: offsetVectorHorizon * i + OffsetVector, scale: Scale));
        }
    }

    public void Update()
    {
        LastDamageElapsedTimeInSeconds += Globals.TotalSeconds;
    }

    public void Draw()
    {
        for (int i = 0; i < Count; i++)
        {
            Anims.DrawFromKey(i, Position); // С 3x зумом  верхний левый угол - 1920/3 : 1080/3
        }
    }

    public void Decrease()
    {
        if (LastDamageElapsedTimeInSeconds > ShieldTimeInSeconds)
        {
            LastDamageElapsedTimeInSeconds = 0;
            Count--;
        }
    }

    public void Increase()
    {
        if (Count < StartCount) Count++;
    }

    public void UpdatePosition(Vector2 position)
    {
        Position = position;
    }
}