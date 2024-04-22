using GetOut.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GetOut.Controllers;

public class PlayerController
{
    public Texture2D Texture { get; init; }
    public float Speed { get; init; }
    public int MovePeriod { get; init; }
    public PlayerModel Player { get; private set; }

    public PlayerController(Texture2D texture, float speed, PlayerModel playerModel, int movePeriod)
    {
        Texture = texture;
        Speed = speed;
        Player = playerModel;
        MovePeriod = movePeriod;
    }

    public void Move(KeyboardState keyboardState)
    {
        if (keyboardState.IsKeyDown(Keys.W))
            Player.Position = new Vector2(Player.Position.X, Player.Position.Y - Speed);
        if (keyboardState.IsKeyDown(Keys.S))
            Player.Position = new Vector2(Player.Position.X, Player.Position.Y + Speed);
        if (keyboardState.IsKeyDown(Keys.A))
            Player.Position = new Vector2(Player.Position.X - Speed, Player.Position.Y);
        if (keyboardState.IsKeyDown(Keys.D))
            Player.Position = new Vector2(Player.Position.X + Speed, Player.Position.Y);
    }
}