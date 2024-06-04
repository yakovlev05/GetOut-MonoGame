using System;
using System.Collections.Generic;
using GetOut.Controllers;
using GetOut.Program;
using GetOut.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GetOut.Models;

public class BigZombie : StupidMonster
{
    public sealed override Hearts Hearts { get; set; }
    public sealed override AnimationController Anims { get; init; } = new();
    public override int Width => 32;
    public override int Height => 36;
    public override int Score => 37;

    public BigZombie(Vector2 positionInWorld, List<Tuple<Point, int>> pathInWorldPoints, int heartsCount = 3,
        float speed = 1) : base(positionInWorld, pathInWorldPoints, speed)
    {
        Hearts = new Hearts(PositionInWorld, heartsCount, 0.35f, new Vector2(8, 5), 3);

        var texture = Globals.Content.Load<Texture2D>("./Levels/assets/BigZombie");

        Anims.AddAnimation("idle", new Animation(texture, 4, 2, 0.1f, 2, 4));
        Anims.AddAnimation("run_right", new Animation(texture, 4, 2, 0.1f, 1, 4));
        Anims.AddAnimation("run_left", new Animation(texture, 4, 2, 0.1f, 1, 4, true));
    }
}