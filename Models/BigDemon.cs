using System.Collections.Generic;
using GetOut.Controllers;
using GetOut.Program;
using GetOut.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace GetOut.Models;

public class BigDemon : IEntityInterface
{
    public MapController MapController { get; set; }
    public Hero Hero { get; set; }
    public Hearts Hearts { get; set; }

    private AnimationController Anims { get; init; } = new();

    private Vector2 PositionInWorld { get; set; }
    public int Width => 32;
    public int Height => 36;

    private readonly Queue<string>
        _animationQueue = new(new[] { "idle", "run_right", "run_left" }); // Круг повторения анимации

    private float _timeSinceLastAnimationSwitch = 0.0f; // Время с последнего переключения анимации

    private readonly Dictionary<string, float> _animationDurations = new()
    {
        { "idle", 5.0f }, // 5 секунд
        { "run_right", 5.0f }, // 5 секунд
        { "run_left", 5.0f } // 5 секунд
    };

    public BigDemon(Vector2 positionInWorld, int heartsCount = 3)
    {
        PositionInWorld = positionInWorld;
        Hearts = new Hearts(PositionInWorld, heartsCount, 0.35f, new Vector2(8, 0), 3);

        var texture = Globals.Content.Load<Texture2D>("./Levels/assets/BigDemon");

        Anims.AddAnimation("idle", new Animation(texture, 4, 2, 0.1f, 2, 4));
        Anims.AddAnimation("run_right", new Animation(texture, 4, 2, 0.1f, 1, 4));
        Anims.AddAnimation("run_left", new Animation(texture, 4, 2, 0.1f, 1, 4, true));
    }

    public void Update()
    {
        if (Hearts.IsDied) return;
        
        if (IsHeroIntersect(Hero.GetDefaultRectangleInScreenCord())) Hero.Hearts.Decrease();
        if (Hero.IsAttack && IsHeroIntersect(Hero.GetRectangleAttackInScreenCord())) Hearts.Decrease(); // Герой нас дамажит

        var currentAnimation = _animationQueue.Peek();
        _timeSinceLastAnimationSwitch += Globals.TotalSeconds;

        if (_timeSinceLastAnimationSwitch >= _animationDurations[currentAnimation])
        {
            _animationQueue.Dequeue();
            _animationQueue.Enqueue(currentAnimation);
            _timeSinceLastAnimationSwitch = 0.0f;
        }

        Move(currentAnimation);
        AttackHero();
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

    private void Move(string currentAnimation)
    {
        if (currentAnimation == "idle") PositionInWorld += new Vector2(0, 0);
        else if (currentAnimation == "run_right") PositionInWorld += new Vector2(1, 0);
        else if (currentAnimation == "run_left") PositionInWorld += new Vector2(-1, 0);
    }

    private void AttackHero()
    {
        if (IsHeroIntersect(Hero.GetDefaultRectangleInScreenCord())) Hero.Hearts.Decrease();
    }

    private bool IsHeroIntersect(RectangleF rectangleHero)
    {
        var demonPosition = Globals.Camera.WorldToScreen(PositionInWorld);
        var rectangleDemon = new RectangleF(demonPosition.X, demonPosition.Y, 32 * Globals.Camera.Zoom,
            36 * Globals.Camera.Zoom);

        return rectangleDemon.Intersects(rectangleHero);
    }

    public void Init()
    {
    }
}