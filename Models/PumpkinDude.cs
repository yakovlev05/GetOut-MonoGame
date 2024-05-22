using System;
using System.Collections.Generic;
using System.Linq;
using Dungeon;
using GetOut.Controllers;
using GetOut.Program;
using GetOut.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

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
    public int Width => 16;
    public int Height => 23;

    private MapCell[,] Map { get; set; }

    private readonly float Speed = 0.8f;

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

    public void GoToHero()
    { //Говнокод, но мы  это исправим
        var person = Hero.StartPosition;
        var p = Globals.Camera.ScreenToWorld(person);
        // var point1 = new Point((int)Math.Round((p.X / 16.0) + 0.5), (int)Math.Ceiling(p.Y / 16.0) + 1);
        var point1 = new Point((int)Math.Round((p.X / 16.0) + 0.5), (int)Math.Ceiling((p.Y +16*2)/ 16.0));

        var start = new Point((int)Math.Round(PositionInWorld.X / 16), (int)Math.Floor((PositionInWorld.Y / 16)));

        List<Point> reversePath = null;
        try
        {
            reversePath = BfsTask.FindPaths(Map,
                start,
                new Point[] { point1 }).First().ToList();
        }
        catch
        {
            return;
        }


        if (reversePath.Count <= 1) return;
        reversePath.Reverse();
        var path = reversePath;

        var point = path.Skip(1).First();


        var target = new Vector2(point.X * 16, point.Y * 16);
        var direction = Vector2.Normalize(target - PositionInWorld) * Speed;
        PositionInWorld += direction * Speed;


        // PositionInWorld += TryMove(direction);


        // PositionInWorld = target;
        Console.WriteLine(PositionInWorld);
    }

    public Vector2 TryMove(Vector2 nextPos)
    {
        var nextRect = new RectangleF(nextPos.X, nextPos.Y, Width * 3, Height * 3);

        if (MapController.IsMovePossible(nextRect)) return nextPos;

        return Vector2.Zero;
    }
}