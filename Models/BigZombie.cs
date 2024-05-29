using System.Collections.Generic;
using GetOut.Controllers;
using GetOut.Program;
using GetOut.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace GetOut.Models;

public class BigZombie : IEntityInterface
{
    public Hero Hero { get; set; }
    public MapController MapController { get; set; }
    public Hearts Hearts { get; set; }

    private AnimationController Anims { get; init; } = new();

    private int Width => 32;
    private int Height => 36;
    private float Speed { get; set; }
    private Vector2 PositionInWorld { get; set; }

    private float _timeSinceLastAnimationSwitch = 0.0f; // Время с последнего переключения анимации

    private readonly Queue<string> _animationQueue = new(new[] // Круг анимации
    {
        "idle",
        "run_right",
        "run_left"
    });

    private readonly Dictionary<string, float> _animationDurations = new() // По сути - маршрут монстра
    {
        { "idle", 5.0f }, // 5 секунд
        { "run_right", 5.0f }, // 5 секунд
        { "run_left", 5.0f } // 5 секунд
    };


    public BigZombie(Vector2 positionInWorld, float speed = 1, int heartsCount = 3)
    {
        PositionInWorld = positionInWorld;
        Speed = speed;
        Hearts = new Hearts(PositionInWorld, heartsCount, 0.35f, new Vector2(8, 5), 3);

        var texture = Globals.Content.Load<Texture2D>("./Levels/assets/BigZombie");

        Anims.AddAnimation("idle", new Animation(texture, 4, 2, 0.1f, 2, 4));
        Anims.AddAnimation("run_right", new Animation(texture, 4, 2, 0.1f, 1, 4));
        Anims.AddAnimation("run_left", new Animation(texture, 4, 2, 0.1f, 1, 4, true));
    }

    public void Update()
    {
        if (Hearts.IsDied) return;

        if (IsHeroIntersect(Hero.GetDefaultRectangleInScreenCord())) Hero.Hearts.Decrease();
        if (Hero.IsAttack && IsHeroIntersect(Hero.GetRectangleAttackInScreenCord())) Hearts.Decrease();

        var currentAnimation = _animationQueue.Peek();
        _timeSinceLastAnimationSwitch += Globals.TotalSeconds;

        if (_timeSinceLastAnimationSwitch >= _animationDurations[currentAnimation])
        {
            _animationQueue.Dequeue();
            _animationQueue.Enqueue(currentAnimation);
            _timeSinceLastAnimationSwitch = 0.0f;
        }

        Move(currentAnimation);
        Anims.Update(currentAnimation);

        Hearts.Update();
        Hearts.UpdatePosition(PositionInWorld);
    }

    public void Draw()
    {
        if (Hearts.IsDied) return;
        Anims.Draw(PositionInWorld, Hearts.IsActiveShield);
        Hearts.Draw();
    }

    public void Init()
    {
    }

    private bool IsHeroIntersect(RectangleF rectangleHero)
    {
        var zombiePosition = Globals.Camera.WorldToScreen(PositionInWorld);
        var rectangleDemon = new RectangleF(zombiePosition.X, zombiePosition.Y, Width * Globals.Camera.Zoom,
            Height * Globals.Camera.Zoom);

        return rectangleDemon.Intersects(rectangleHero);
    }

    private void Move(string currentAnimation)
    {
        if (currentAnimation == "idle") PositionInWorld += new Vector2(0, 0) * Speed;
        else if (currentAnimation == "run_right") PositionInWorld += new Vector2(1, 0) * Speed;
        else if (currentAnimation == "run_left") PositionInWorld += new Vector2(-1, 0) * Speed;
    }
}