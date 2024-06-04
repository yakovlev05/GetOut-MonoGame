using System;
using System.Collections.Generic;
using System.Linq;
using GetOut.Controllers;
using GetOut.Program;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace GetOut.Models;

public abstract class StupidMonster : IEntityInterface
{
    public MapController MapController { get; set; }
    public Hero Hero { get; set; }
    public abstract Hearts Hearts { get; set; }
    public abstract AnimationController Anims { get; init; }
    public ScoreStatistic ScoreStatistic { get; set; }

    protected Vector2 PositionInWorld { get; private set; }
    private float Speed { get; set; }
    public abstract int Width { get; }
    public abstract int Height { get; }
    public abstract int Score { get; }

    private List<Tuple<Point, int>> PathInWorldPoints { get; set; }
    private int CurrentPointPathIndex { get; set; }
    private float WaitTime { get; set; }

    protected StupidMonster(Vector2 positionInWorld, List<Tuple<Point, int>> pathInWorldPoints, float speed)
    {
        PositionInWorld = positionInWorld;
        PathInWorldPoints = pathInWorldPoints;
        Speed = speed;
        WaitTime = PathInWorldPoints.First().Item2;
    }

    public void Update()
    {
        if (Hearts.IsDied) return;

        if (IsHeroIntersect(Hero.GetDefaultRectangleInScreenCord()))
        {
            Hero.Hearts.Decrease();
            UpdateScoreStatistic();
        }

        if (Hero.IsAttack && IsHeroIntersect(Hero.GetRectangleAttackInScreenCord()))
        {
            Hearts.Decrease();
            UpdateScoreStatistic();
        } // Герой нас дамажит


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
        var monsterPosition = Globals.Camera.WorldToScreen(PositionInWorld);
        var rectangleDemon = new RectangleF(monsterPosition.X, monsterPosition.Y, 32 * Globals.Camera.Zoom,
            36 * Globals.Camera.Zoom);

        return rectangleDemon.Intersects(rectangleHero);
    }

    public void Init()
    {
    }

    private void UpdateScoreStatistic()
    {
        ScoreStatistic.AddScore(Score);
    }
}