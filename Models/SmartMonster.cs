using System;
using System.Linq;
using GetOut.Algorithms.BFS;
using GetOut.Controllers;
using GetOut.Program;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace GetOut.Models;

public abstract class SmartMonster : IEntityInterface
{
    public Hero Hero { get; set; }
    public MapController MapController { get; set; }
    protected abstract Hearts Hearts { get; set; }
    public abstract AnimationController Anims { get; init; }
    public ScoreStatistic ScoreStatistic { get; set; }

    protected Vector2 PositionInWorld { get; set; }
    private Vector2 ActualDirection { get; set; }
    private float Speed { get; set; }
    private bool IsActive { get; set; } // Ищет ли игрока
    private int StepsForActivate { get; set; } // Длина пути от монстра до героя, при котором монстр ничинает охоту
    protected abstract int Width { get; }
    protected abstract int Height { get; }
    public abstract int Score { get; }

    protected SmartMonster(Vector2 positionInWorld, float speed, int stepsForActivate)
    {
        PositionInWorld = positionInWorld;
        Speed = speed;
        StepsForActivate = stepsForActivate;
    }

    public void Update()
    {
        if (Hearts.IsDied) return; // Монстр уже погиб

        if (!IsActive)
        {
            ShouldFollowHero(); // Проверяем активность монстра (должен ли он идти за героем)
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

        if (IsHeroIntersect(Hero.GetDefaultRectangleInScreenCord()))
        {
            Hero.Hearts.Decrease();
            UpdateScoreStatistic();
        } // Дамажим героя

        if (Hero.IsAttack && IsHeroIntersect(Hero.GetRectangleAttackInScreenCord()))
        {
            Hearts.Decrease();
            UpdateScoreStatistic();
        }

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
        // Получаем коодинаты героя и переводим в координаты по тайлам с учётом погрешностей
        var hero = Globals.Camera.ScreenToWorld(Hero.StartPosition);
        var heroPosition = new Point((int)Math.Round(hero.X / 16.0 + 0.5),
            (int)Math.Ceiling((hero.Y + 16 * 2) / 16.0) - 1);

        var monsterPosition =
            new Point((int)Math.Round(PositionInWorld.X / 16), (int)Math.Floor((PositionInWorld.Y / 16)));

        // Поиск в ширину, старт - позиция монстра, финиш - позиция героя
        var path = Bfs.FindPath(MapController.Map, monsterPosition, heroPosition);

        if (path.Count < 2)
        {
            ActualDirection = Vector2.Zero;
            return;
        } // Поиск возващае путь вместе с начальной точкой

        var nextStepPoint = path.Skip(1).First();


        var target = new Vector2(nextStepPoint.X * 16, nextStepPoint.Y * 16);
        var direction = Vector2.Normalize(target - PositionInWorld) * Speed;
        PositionInWorld += direction * Speed;

        ActualDirection = direction; // Обновляем позици для обновления анимации
    }

    private bool IsHeroIntersect(RectangleF rectangleHero)
    {
        var dudePosition = Globals.Camera.WorldToScreen(PositionInWorld);
        var rectangleDude = new RectangleF(dudePosition.X, dudePosition.Y, Width * Globals.Camera.Zoom,
            Height * Globals.Camera.Zoom);
        return rectangleDude.Intersects(rectangleHero);
    }

    private void ShouldFollowHero()
    {
        var hero = Globals.Camera.ScreenToWorld(Hero.StartPosition);
        var heroPosition = new Point((int)Math.Round(hero.X / 16.0 + 0.5),
            (int)Math.Ceiling((hero.Y + 16 * 2) / 16.0) - 1);
        var monsterPosition =
            new Point((int)Math.Round(PositionInWorld.X / 16), (int)Math.Floor((PositionInWorld.Y / 16)));

        var path = Bfs.FindPath(MapController.Map, monsterPosition, heroPosition);
        IsActive = path.Count <= StepsForActivate;
    }

    private void UpdateScoreStatistic()
    {
        ScoreStatistic.AddScore(Score);
    }
}