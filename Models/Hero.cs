using System;
using System.Collections.Generic;
using GetOut.Controllers;
using GetOut.Program;
using GetOut.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace GetOut.Models;

public class Hero : IEntityInterface
{
    public bool StaticPosition { get; init; } = true;
    private float Speed { get; init; }
    private AnimationController Anims { get; init; } = new();
    public Vector2 StartPosition { get; init; }

    private Vector2 Direction => Vector2.Normalize(InputController.Direction) * Speed * Globals.TotalSeconds;

    public float WidthHero => 15;
    public float HeightHero => 38;

    public float OffsetPositionX => 50;
    public float OffsetPositionY => 42;

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
        if (InputController.IsPressedKey(Keys.Q))
            Anims.Update(Keys.Q);
    }

    public void Draw() => Anims.Draw(StartPosition);

    public Vector2 GetDirection(MapController mapController, Matrix matrix)
    {
        if (double.IsNaN(Direction.X) || double.IsNaN(Direction.Y)) return Vector2.Zero;

        // Основное движение
        var vectorDirection = TryMove(Direction, mapController, matrix);
        if (vectorDirection != null) return vectorDirection.Value;

        // Пробуем по x
        vectorDirection =
            TryMove(Vector2.Normalize(new Vector2(InputController.Direction.X, 0)) * Speed * Globals.TotalSeconds,
                mapController, matrix);
        if (vectorDirection != null) return vectorDirection.Value;

        // Пробуем по y
        vectorDirection =
            TryMove(Vector2.Normalize(new Vector2(0, InputController.Direction.Y)) * Speed * Globals.TotalSeconds,
                mapController, matrix);
        if (vectorDirection != null) return vectorDirection.Value;

        return Vector2.Zero;
    }

    private Vector2? TryMove(Vector2 direction, MapController mapController, Matrix matrix)
    {
        if (double.IsNaN(direction.X) || double.IsNaN(direction.Y)) return Vector2.Zero;

        var scaleDirection = new Vector2(direction.X * matrix.M11, direction.Y * matrix.M22);
        var personPosition =
            Vector2.Transform(new Vector2(StartPosition.X + OffsetPositionX, StartPosition.Y + OffsetPositionY),
                matrix);
        var nextPosition = personPosition + scaleDirection;

        var nextHeroRectangle =
            new RectangleF(nextPosition.X, nextPosition.Y, WidthHero * matrix.M11, HeightHero * matrix.M22);

        if (mapController.IsMovePossible(nextHeroRectangle)) return direction;
        return null;
    }
}