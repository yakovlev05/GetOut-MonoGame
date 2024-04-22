using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GetOut.Models;

public class PlayerModel
{
    public Texture2D Texture { get; init; }
    public Vector2 Position { get; set; }
    public Rectangle Rectangle { get; init; }

    public PlayerModel(Texture2D texture, Vector2 position, Rectangle rectangle)
    {
        Texture = texture;
        Position = position;
        Rectangle = rectangle;
    }
}