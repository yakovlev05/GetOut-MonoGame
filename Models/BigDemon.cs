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

public class BigDemon : IEntityInterface
{
    public MapController MapController { get; set; }
    public Hero Hero { get; set; }
    public Hearts Hearts { get; set; }

    private AnimationController Anims { get; init; } = new();

    private Vector2 PositionInWorld { get; set; }
    private float Speed { get; set; }
    public int Width => 32;
    public int Height => 36;

    private List<Tuple<Point, int>> PathInWorldPoints { get; set; }
    private int CurrentPointPathIndex { get; set; } = 0;
    private float WaitTime { get; set; }

    public BigDemon(Vector2 positionInWorld, List<Tuple<Point, int>> pathInWorldPoints, int heartsCount = 3,
        float speed = 1)
    {
        PositionInWorld = positionInWorld;
        Hearts = new Hearts(PositionInWorld, heartsCount, 0.35f, new Vector2(8, 0), 3);
        Speed = speed;
        PathInWorldPoints = pathInWorldPoints;
        WaitTime = PathInWorldPoints.First().Item2;

        var texture = Globals.Content.Load<Texture2D>("./Levels/assets/BigDemon");

        Anims.AddAnimation("idle", new Animation(texture, 4, 2, 0.1f, 2, 4));
        Anims.AddAnimation("run_right", new Animation(texture, 4, 2, 0.1f, 1, 4));
        Anims.AddAnimation("run_left", new Animation(texture, 4, 2, 0.1f, 1, 4, true));
    }

    public void Update()
    {
        if (Hearts.IsDied) return;

        if (IsHeroIntersect(Hero.GetDefaultRectangleInScreenCord())) Hero.Hearts.Decrease();
        if (Hero.IsAttack && IsHeroIntersect(Hero.GetRectangleAttackInScreenCord()))
            Hearts.Decrease(); // Герой нас дамажит


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