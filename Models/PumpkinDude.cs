using System;
using System.Linq;
using GetOut.Algorithms.BFS;
using GetOut.Controllers;
using GetOut.Program;
using GetOut.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GetOut.Models;

public class PumpkinDude : IEntityInterface
{
    public Hearts Hearts { get; set; }
    public Hero Hero { get; set; }
    public MapController MapController { get; set; }
    public bool StaticPosition { get; init; } = false;
    public bool RequireMapController { get; init; } = true;

    private AnimationController Anims { get; init; } = new();
    public Vector2 PositionInWorld { get; private set; }
    public Vector2 ActualDirection { get; private set; }
    public int Width => 16;
    public int Height => 23;

    private MapCell[,] Map { get; set; }

    private float Speed { get; set; } = 1;

    public PumpkinDude(Vector2 positionInWorld)
    {
        PositionInWorld = positionInWorld;

        var texture = Globals.Content.Load<Texture2D>("./Levels/assets/PumpkinDude");

        Anims.AddAnimation("idle", new Animation(texture, 4, 2, 0.1f, 1, 4, offsetPosition: new Vector2(0, -7f)));
        Anims.AddAnimation("run_right", new Animation(texture, 4, 2, 0.1f, 2, 4));
        Anims.AddAnimation("run_left", new Animation(texture, 4, 2, 0.1f, 2, 4, true));
    }

    public void Update()
    {
        Anims.Update("idle");
        GoToHero();
    }

    public void Draw()
    {
        // var t = PositionInWorld - new Vector2(0, 5f);
        Anims.Draw(PositionInWorld);
    }

    public void Init()
    {
        Map = MapController.GetMapCells();
    }

    private void GoToHero()
    {
        // Получаем коодинаты героя и переводим в координаты по тайлам с учётом погрешностей
        var hero = Globals.Camera.ScreenToWorld(Hero.StartPosition);
        var heroPosition = new Point((int)Math.Round(hero.X / 16.0 + 0.5), (int)Math.Ceiling((hero.Y + 16 * 2) / 16.0));

        var dudePosition =
            new Point((int)Math.Round(PositionInWorld.X / 16), (int)Math.Floor((PositionInWorld.Y / 16)));

        // Поиск в ширину, старт - позиция монстра, финиш - позиция героя
        var path = Bfs.FindPath(Map, dudePosition, heroPosition);

        if (path.Count < 2) return; // Поиск возващае путь вместе с начальной точкой
        var nextStepPoint = path.Skip(1).First();


        var target = new Vector2(nextStepPoint.X * 16, nextStepPoint.Y * 16);
        var direction = Vector2.Normalize(target - PositionInWorld) * Speed;
        PositionInWorld += direction * Speed;

        ActualDirection = direction; // Обновляем позици для обновления анимации
    }
}