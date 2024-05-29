using System;
using System.Linq;
using GetOut.Algorithms.BFS;
using GetOut.Controllers;
using GetOut.Program;
using GetOut.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace GetOut.Models;

public class Chort : IEntityInterface
{
    public Hero Hero { get; set; }
    public MapController MapController { get; set; }
    private Hearts Hearts { get; set; }
    private AnimationController Anims { get; set; } = new();

    private Vector2 PositionInWorld { get; set; }
    private Vector2 ActualDirection { get; set; }
    private int Width => 16;
    private int Height => 23;
    private bool IsActive { get; set; }
    private float Speed { get; set; }
    private int StepsForActivate { get; set; }

    public Chort(Vector2 positionInWorld, float speed = 1, int stepsForActivate = 50, int heartsCount = 3)
    {
        PositionInWorld = positionInWorld;
        Speed = speed;
        StepsForActivate = stepsForActivate;

        Hearts = new Hearts(PositionInWorld, heartsCount, 0.35f, new Vector2(0, -6.5f), 3);

        var texture = Globals.Content.Load<Texture2D>("./Levels/assets/Chort");

        Anims.AddAnimation("idle", new Animation(texture, 4, 2, 0.1f, 2, 4));
        Anims.AddAnimation("run_right", new Animation(texture, 4, 2, 0.1f, 1, 4));
        Anims.AddAnimation("run_left", new Animation(texture, 4, 2, 0.1f, 1, 4, true));
    }

    public void Update()
    {
        if (Hearts.IsDied) return;

        if (!IsActive)
        {
            ShouldFollowHero();
            Anims.Update("idle");
            return;
        }

        GoToHero();
        if (Math.Abs(ActualDirection.X) > Math.Abs(ActualDirection.Y))
        {
            Anims.Update(ActualDirection.X > 0 ? "run_right" : "run_left");
        }
        else
        {
            Anims.Update(ActualDirection.Y != 0 ? "run_right" : "idle");
        }

        if (IsHeroIntersect(Hero.GetDefaultRectangleInScreenCord())) Hero.Hearts.Decrease(); // Дамажим героя
        if (Hero.IsAttack && IsHeroIntersect(Hero.GetRectangleAttackInScreenCord())) Hearts.Decrease();

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

    private void GoToHero()
    {
        var hero = Globals.Camera.ScreenToWorld(Hero.StartPosition);
        var heroPosition = new Point(
            (int)Math.Round(hero.X / 16.0 + 0.5),
            (int)Math.Ceiling((hero.Y + 16 * 2) / 16.0) - 1
        );

        var chortPosition = new Point((int)Math.Round(PositionInWorld.X / 16),
            (int)Math.Floor((PositionInWorld.Y / 16))
        );

        var path = Bfs.FindPath(MapController.Map, chortPosition, heroPosition);
        if (path.Count < 2)
        {
            ActualDirection = Vector2.Zero;
            return;
        }

        var nextStepPoint = path.Skip(1).First();

        var target = new Vector2(nextStepPoint.X * 16, nextStepPoint.Y * 16);
        var direction = Vector2.Normalize(target - PositionInWorld) * Speed;
        PositionInWorld += direction * Speed;

        ActualDirection = direction; // Обновляем позици для обновления анимации
    }

    private bool IsHeroIntersect(RectangleF rectangleHero)
    {
        var chortPosition = Globals.Camera.WorldToScreen(PositionInWorld);
        var rectangleDude = new RectangleF(chortPosition.X, chortPosition.Y, Width * Globals.Camera.Zoom,
            Height * Globals.Camera.Zoom);
        return rectangleDude.Intersects(rectangleHero);
    }

    private void ShouldFollowHero()
    {
        var hero = Globals.Camera.ScreenToWorld(Hero.StartPosition);
        var heroPosition = new Point((int)Math.Round(hero.X / 16.0 + 0.5),
            (int)Math.Ceiling((hero.Y + 16 * 2) / 16.0) - 1);
        var chortPosition =
            new Point((int)Math.Round(PositionInWorld.X / 16), (int)Math.Floor((PositionInWorld.Y / 16)));

        var path = Bfs.FindPath(MapController.Map, chortPosition, heroPosition);
        IsActive = path.Count <= StepsForActivate;
    }
}