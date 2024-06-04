using GetOut.Controllers;
using GetOut.Program;
using GetOut.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GetOut.Models;

public class PumpkinDude : SmartMonster
{
    protected sealed override Hearts Hearts { get; set; }
    public sealed override AnimationController Anims { get; init; } = new();
    protected override int Width => 16;
    protected override int Height => 23;
    public override int Score => 321;

    public PumpkinDude(Vector2 positionInWorld, float speed = 1, int stepsForActivate = 20, int heartsCount = 3) :
        base(positionInWorld, speed, stepsForActivate)
    {
        Hearts = new Hearts(PositionInWorld, heartsCount, 0.35f, new Vector2(0, -6.5f), 3);

        var texture = Globals.Content.Load<Texture2D>("./Levels/assets/PumpkinDude");

        Anims.AddAnimation("idle", new Animation(texture, 4, 2, 0.1f, 1, 4, offsetPosition: new Vector2(0, -7f)));
        Anims.AddAnimation("run_right", new Animation(texture, 4, 2, 0.1f, 2, 4, offsetPosition: new Vector2(0, -7f)));
        Anims.AddAnimation("run_left",
            new Animation(texture, 4, 2, 0.1f, 2, 4, true, offsetPosition: new Vector2(0, -7f)));
    }
}