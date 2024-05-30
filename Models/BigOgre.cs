using System;
using System.Collections.Generic;
using System.Linq;
using GetOut.Controllers;
using GetOut.Program;
using GetOut.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace GetOut.Models;

public class BigOgre : IEntityInterface
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

    private List<Tuple<Point, int>> PathInWorldPoints { get; set; }
    private int CurrentPointPathIndex { get; set; } = 0;
    private float WaitTime { get; set; }

    public BigOgre(Vector2 positionInWorld, List<Tuple<Point, int>> pathInWorldPoints, float speed = 1,
        int heartsCount = 3)
    {
        PositionInWorld = positionInWorld;
        Speed = speed;
        Hearts = new Hearts(PositionInWorld, heartsCount, 0.35f, new Vector2(8, 0), 3);
        PathInWorldPoints = pathInWorldPoints;
        WaitTime = PathInWorldPoints.First().Item2;

        var texture = Globals.Content.Load<Texture2D>("./Levels/assets/BigOgre");

        Anims.AddAnimation("idle", new Animation(texture, 4, 2, 0.1f, 2, 4));
        Anims.AddAnimation("run_right", new Animation(texture, 4, 2, 0.1f, 1, 4));
        Anims.AddAnimation("run_left", new Animation(texture, 4, 2, 0.1f, 1, 4, true));
    }

    public void Update()
    {
        if (Hearts.IsDied) return;

        if (IsHeroIntersect(Hero.GetDefaultRectangleInScreenCord())) Hero.Hearts.Decrease();
        if (Hero.IsAttack && IsHeroIntersect(Hero.GetRectangleAttackInScreenCord())) Hearts.Decrease();

        Move();

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
        var ogrePosition = Globals.Camera.WorldToScreen(PositionInWorld);
        var rectangleDemon = new RectangleF(ogrePosition.X, ogrePosition.Y, Width * Globals.Camera.Zoom,
            Height * Globals.Camera.Zoom);

        return rectangleDemon.Intersects(rectangleHero);
    }

    private void Move()
    {
        var currentPoint = PathInWorldPoints[CurrentPointPathIndex].Item1;
        var vector = currentPoint.ToVector2() - PositionInWorld;
        var direction = vector == Vector2.Zero ? vector : Vector2.Normalize(vector);

        PositionInWorld += Vector2.Distance(PositionInWorld, currentPoint.ToVector2()) < Speed
            ? Vector2.Zero
            : direction * Speed;
        
        if (WaitTime <= 0)
        {
            if (Vector2.Distance(PositionInWorld, currentPoint.ToVector2()) < Speed)
            {
                CurrentPointPathIndex++;
                if (CurrentPointPathIndex >= PathInWorldPoints.Count) CurrentPointPathIndex = 0;
                WaitTime = PathInWorldPoints[CurrentPointPathIndex].Item2;
            }
        }
        else WaitTime -= Globals.TotalSeconds;

        if (direction.X > 0 || (direction.X == 0 && direction.Y != 0)) Anims.Update("run_right");
        else if (direction.X < 0) Anims.Update("run_left");
        else Anims.Update("idle");
    }
}