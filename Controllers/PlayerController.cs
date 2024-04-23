using System.Linq;
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
    public MazeModel Maze { get; init; }

    public PlayerController(Texture2D texture, float speed, PlayerModel playerModel, int movePeriod,
        MazeModel mazeModel)
    {
        Texture = texture;
        Speed = speed;
        Player = playerModel;
        MovePeriod = movePeriod;
        Maze = mazeModel;
    }

    public void Move(KeyboardState keyboardState)
    {
        var lastPosition = Player.Position;
        if (keyboardState.IsKeyDown(Keys.W))
            Player.Position = new Vector2(Player.Position.X, Player.Position.Y - Speed);
        if (keyboardState.IsKeyDown(Keys.S))
            Player.Position = new Vector2(Player.Position.X, Player.Position.Y + Speed);
        if (keyboardState.IsKeyDown(Keys.A))
            Player.Position = new Vector2(Player.Position.X - Speed, Player.Position.Y);
        if (keyboardState.IsKeyDown(Keys.D))
            Player.Position = new Vector2(Player.Position.X + Speed, Player.Position.Y);
        var rect = new Rectangle((int)Player.Position.X, (int)Player.Position.Y, Player.Rectangle.Width,
            Player.Rectangle.Height);
        if (!Maze.ListWalls.Any(x => x.Intersects(rect))) Player.Position = lastPosition;
    }
}