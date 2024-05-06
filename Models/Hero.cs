using GetOut.Controllers;
using GetOut.Program;
using GetOut.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace GetOut.Models;

public class Hero
{
    private float Speed { get; init; }
    private AnimationController Anims { get; init; } = new();
    public Vector2 StartPosition { get; init; }

    private Vector2 Direction => Vector2.Normalize(InputController.Direction) * Speed * Globals.TotalSeconds;

    public Hero(Vector2 position, float speed)
    {
        StartPosition = position;
        Speed = speed;

        var texture = Globals.Content.Load<Texture2D>("./hero");

        Anims.AddAnimation(new Vector2(0, 0), new Animation(texture, 12, 8, 0.1f, 5, 10)); // Состояние в покое
        Anims.AddAnimation(new Vector2(1, 0), new Animation(texture, 12, 8, 0.1f, 8, 10)); // Движение вправо
        Anims.AddAnimation(new Vector2(1, 1), new Animation(texture, 12, 8, 0.1f, 8, 10)); //  Движение вправо вниз
        Anims.AddAnimation(new Vector2(0, 1), new Animation(texture, 12, 8, 0.1f, 8, 10)); // Движение вниз
        Anims.AddAnimation(new Vector2(-1, 1),
            new Animation(texture, 12, 8, 0.1f, 8, 10, true)); // Движение влево вниз
        Anims.AddAnimation(new Vector2(-1, 0), new Animation(texture, 12, 8, 0.1f, 8, 10, true)); // Движение влево
        Anims.AddAnimation(new Vector2(-1, -1),
            new Animation(texture, 12, 8, 0.1f, 8, 10, true)); // Движение влево вверх
        Anims.AddAnimation(new Vector2(0, -1), new Animation(texture, 12, 8, 0.1f, 8, 10)); // Движение вверх
        Anims.AddAnimation(new Vector2(1, -1), new Animation(texture, 12, 8, 0.1f, 8, 10)); // Движение вправо вверх
        Anims.AddAnimation(Keys.Q, new Animation(texture, 12, 8, 0.1f, 4, 10)); // Смерть
    }

    public void Update()
    {
        Anims.Update(InputController.Direction);
        if (InputController.IsPressedKey(Keys.Q) != Keys.None)
            Anims.Update(Keys.Q);
    }

    public void Draw() => Anims.Draw(StartPosition);

//TODO: MapController сделать
    public Vector2 GetDirection(MapController mapController, Matrix _matrix)
    {
        if (double.IsNaN(Direction.X) || double.IsNaN(Direction.Y)) return Vector2.Zero;


        var direction2 = new Vector2(Direction.X * _matrix.M11, Direction.Y * _matrix.M22);
        Vector2 nextPosition =
            Vector2.Transform(new Vector2(StartPosition.X + 50, StartPosition.Y + 37), _matrix) + direction2;


        var nextHeroRectangle = new RectangleF(nextPosition.X, nextPosition.Y, 15 * 3, 43 * 3);
        if (!Globals.MapController.IsMovePossible(nextHeroRectangle)) return Vector2.Zero;


        return Direction;
    }
}