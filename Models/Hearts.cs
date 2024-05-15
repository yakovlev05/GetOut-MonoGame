using GetOut.Controllers;
using GetOut.Program;
using GetOut.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GetOut.Models;

public class Hearts
{
    public Hero Hero { get; set; }
    public bool StaticPosition { get; init; } = true;
    private AnimationController Anims { get; init; } = new();
    public Vector2 Position { get; init; }
    private int Count { get; set; }
    private int WidthHeart => 13;
    private int HeightHeart => 4;

    public Hearts(Vector2 position, int count)
    {
        Position = position;
        Count = count;

        var texture = Globals.Content.Load<Texture2D>("./Levels/assets/hearts");

        var offsetVector = new Vector2(WidthHeart + 3, 0);
        for (int i = 0; i < Count; i++)
        {
            Anims.AddAnimation(i, new Animation(texture, 1, 3, 0.1f, 2, 1, offsetPosition: offsetVector * i));
        }
    }

    public void Update()
    {
    }

    public void Draw()
    {
        for (int i = 0; i < Count; i++)
        {
            Anims.DrawFromKey(i, new Vector2(640 + 10, 360 + 10)); // С 3x зумом  верхний левый угол - 1920/3 : 1080/3
        }
    }
}