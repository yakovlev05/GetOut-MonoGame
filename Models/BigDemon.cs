using System;
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
    public Hearts Hearts { get; set; }
    public Hero Hero { get; set; }
    public bool StaticPosition { get; init; } = false;
    private AnimationController Anims { get; init; } = new();
    public Vector2 PositionInWorld { get; private set; }
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

    public BigDemon(Vector2 positionInWorld)
    {
        PositionInWorld = positionInWorld;

        var texture = Globals.Content.Load<Texture2D>("./Levels/assets/BigDemon");

        Anims.AddAnimation("idle", new Animation(texture, 4, 2, 0.1f, 2, 4));
        Anims.AddAnimation("run_right", new Animation(texture, 4, 2, 0.1f, 1, 4));
        Anims.AddAnimation("run_left", new Animation(texture, 4, 2, 0.1f, 1, 4, true));
    }

    public void Update()
    {
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
    }

    public void Draw()
    {
        Anims.Draw(PositionInWorld);
    }

    public void Move(string currentAnimation)
    {
        if (currentAnimation == "idle") PositionInWorld += new Vector2(0, 0);
        else if (currentAnimation == "run_right") PositionInWorld += new Vector2(1, 0);
        else if (currentAnimation == "run_left") PositionInWorld += new Vector2(-1, 0);
    }

    public void AttackHero()
    {
        if (IsHeroIntersect()) Hearts.Decrease();

    }

    private bool IsHeroIntersect()
    {
        /////////Герой
        var personPosition =
            Vector2.Transform(new Vector2(900 +50, 500 + 42),
                Globals.HeroMatrix);
        
        var nextHeroRectangle =
            new RectangleF(personPosition.X, personPosition.Y, 15 * Globals.HeroMatrix.M11, 38 * Globals.HeroMatrix.M22);
        
        
        ///////Демон
        var demonPosition = Globals.Camera.WorldToScreen(PositionInWorld);
        var rectangleDemon = new RectangleF(demonPosition.X, demonPosition.Y, 32 * Globals.Camera.Zoom, 36 * Globals.Camera.Zoom);

        return rectangleDemon.Intersects(nextHeroRectangle);
    }
}