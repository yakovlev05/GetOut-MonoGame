using System;
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
    public bool StaticPosition { get; init; } = true;
    private float Speed { get; init; }
    private AnimationController Anims { get; init; } = new();
    public Vector2 StartPosition { get; init; }

    public Vector2 ActualPositionInScreenCord =>
        Vector2.Transform(new Vector2(StartPosition.X + OffsetPositionX, StartPosition.Y + OffsetPositionY),
            Globals.HeroMatrix);

    private Vector2 MoveDirection => Vector2.Normalize(InputController.Direction) * Speed * Globals.TotalSeconds;
    private Vector2 Direction => InputController.Direction;

    public float WidthHero => 15;
    public float HeightHero => 38;

    public float OffsetPositionX => 50;
    public float OffsetPositionY => 42;

    public bool IsDied { get; set; } = false;
    public bool IsShowGameOverScreen { get; private set; } = false;

    public bool IsAttack => InputController.IsPressedKey(Keys.Space);

    public Vector2 LastHorizontalDirection { get; set; } = new Vector2(1, 0);

    public Hearts Hearts { get; set; }

    public Hero(Vector2 position, float speed)
    {
        StartPosition = position;
        Speed = speed;

        var texture = Globals.Content.Load<Texture2D>("./hero");

        Anims.AddAnimation(new Vector2(0, 0), new Animation(texture, 12, 8, 0.1f, 5, 10)); // Состояние в покое
        Anims.AddAnimation("left_idle", new Animation(texture, 12, 8, 0.1f, 5, 10, true)); // Состояние в покое - лево
        Anims.AddAnimation(new Vector2(1, 0), new Animation(texture, 12, 8, 0.1f, 8, 10)); // Движение вправо
        Anims.AddAnimation("left_right_run",
            new Animation(texture, 12, 8, 0.1f, 8, 10, true)); // Движение вправо - лицо влево
        Anims.AddAnimation(new Vector2(1, 1), new Animation(texture, 12, 8, 0.1f, 8, 10)); //  Движение вправо вниз
        Anims.AddAnimation(new Vector2(0, 1), new Animation(texture, 12, 8, 0.1f, 8, 10)); // Движение вниз
        Anims.AddAnimation(new Vector2(-1, 1),
            new Animation(texture, 12, 8, 0.1f, 8, 10, true)); // Движение влево вниз
        Anims.AddAnimation(new Vector2(-1, 0), new Animation(texture, 12, 8, 0.1f, 8, 10, true)); // Движение влево
        Anims.AddAnimation(new Vector2(-1, -1),
            new Animation(texture, 12, 8, 0.1f, 8, 10, true)); // Движение влево вверх
        Anims.AddAnimation(new Vector2(0, -1), new Animation(texture, 12, 8, 0.1f, 8, 10)); // Движение вверх
        Anims.AddAnimation(new Vector2(1, -1), new Animation(texture, 12, 8, 0.1f, 8, 10)); // Движение вправо вверх
        Anims.AddAnimation("die", new Animation(texture, 12, 8, 0.1f, 4, 10)); // Смерть
        Anims.AddAnimation("attack_right", new Animation(texture, 12, 8, 0.1f, 3, 10));
        Anims.AddAnimation("left_attack_right", new Animation(texture, 12, 8, 0.1f, 3, 10, true));
    }

    public void Update()
    {
        if (Hearts.IsDied && !Anims.IsAnimationFinished("die"))
        {
            IsDied = true;
            Anims.Update("die");
        }
        else if (InputController.IsPressedKey(Keys.Space))
        {
            Anims.Update(LastHorizontalDirection == new Vector2(1, 0) ? "attack_right" : "left_attack_right");
        }
        else if (Direction == Vector2.Zero && LastHorizontalDirection == new Vector2(-1, 0))
        {
            Anims.Update("left_idle");
        }
        else if ((Direction == new Vector2(0, 1) || (Direction == new Vector2(0, -1))) &&
                 LastHorizontalDirection == new Vector2(-1, 0))
        {
            Anims.Update("left_right_run");
        }
        else Anims.Update(InputController.Direction);

        LastHorizontalDirection = new Vector2(Direction.X == 0 ? LastHorizontalDirection.X : Direction.X, 0);
    }

    public void Draw()
    {
        if (IsDied && Anims.IsAnimationFinished("die"))
        {
            Anims.DrawFrame("die", StartPosition, 10);
            IsShowGameOverScreen = true;
        }
        else Anims.Draw(StartPosition);
    }

    public Vector2 GetMoveDirection(MapController mapController, Matrix matrix)
    {
        if (IsDied || IsAttack) return Vector2.Zero;

        if (double.IsNaN(MoveDirection.X) || double.IsNaN(MoveDirection.Y)) return Vector2.Zero;

        // Основное движение
        var vectorDirection = TryMove(MoveDirection, mapController, matrix);
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

    public RectangleF GetRectangleAttackInScreenCord()
    {
        if (IsAttack && LastHorizontalDirection == new Vector2(1, 0))
            return new RectangleF(ActualPositionInScreenCord.X, ActualPositionInScreenCord.Y,
                WidthHero * Globals.HeroMatrix.M11 + 150, HeightHero * Globals.HeroMatrix.M22);
        if (IsAttack && LastHorizontalDirection == new Vector2(-1, 0))
            return new RectangleF(ActualPositionInScreenCord.X - 150, ActualPositionInScreenCord.Y,
                WidthHero * Globals.HeroMatrix.M11 + 150, HeightHero * Globals.HeroMatrix.M22);


        return RectangleF.Empty;
    }

    public RectangleF GetDefaultRectangleInScreenCord()
    {
        return new RectangleF(ActualPositionInScreenCord.X, ActualPositionInScreenCord.Y,
            WidthHero * Globals.HeroMatrix.M11, HeightHero * Globals.HeroMatrix.M22);
    }
}