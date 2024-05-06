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
    private float _speed;
    private readonly AnimationController _anims = new();
    public Vector2 StartPosition { get; private init; }

    private Vector2 Direction => Vector2.Normalize(InputController.Direction) * _speed * Globals.TotalSeconds;

    public Hero(Vector2 position, float speed)
    {
        StartPosition = position;
        var texture = Globals.Content.Load<Texture2D>("./hero");
        _speed = speed;

        _anims.AddAnimation(new Vector2(0, 0), new Animation(texture, 12, 8, 0.1f, 5, 10)); // Состояние в покое
        _anims.AddAnimation(new Vector2(1, 0), new Animation(texture, 12, 8, 0.1f, 8, 10)); // Движение вправо
        _anims.AddAnimation(new Vector2(1, 1), new Animation(texture, 12, 8, 0.1f, 8, 10)); //  Движение вправо вниз
        _anims.AddAnimation(new Vector2(0, 1), new Animation(texture, 12, 8, 0.1f, 8, 10)); // Движение вниз
        _anims.AddAnimation(new Vector2(-1, 1),
            new Animation(texture, 12, 8, 0.1f, 8, 10, true)); // Движение влево вниз
        _anims.AddAnimation(new Vector2(-1, 0), new Animation(texture, 12, 8, 0.1f, 8, 10, true)); // Движение влево
        _anims.AddAnimation(new Vector2(-1, -1),
            new Animation(texture, 12, 8, 0.1f, 8, 10, true)); // Движение влево вверх
        _anims.AddAnimation(new Vector2(0, -1), new Animation(texture, 12, 8, 0.1f, 8, 10)); // Движение вверх
        _anims.AddAnimation(new Vector2(1, -1), new Animation(texture, 12, 8, 0.1f, 8, 10)); // Движение вправо вверх
        _anims.AddAnimation(Keys.Q, new Animation(texture, 12, 8, 0.1f, 4, 10)); // Смерть
    }

    public void Update()
    {
        _anims.Update(InputController.Direction);
        if (InputController.IsPressedKey(Keys.Q) != Keys.None)
            _anims.Update(Keys.Q);
    }

    public void Draw()
    {
        _anims.Draw(StartPosition);
    }

    public Vector2 GetDirection(MapController mapController, Matrix _matrix)
    {
        if (Globals.MapController != null)
        {
            if (double.IsNaN(Direction.X) || double.IsNaN(Direction.Y) || double.IsInfinity(Direction.X) ||
                double.IsInfinity(Direction.Y))
            {
                return Vector2.Zero;
            }
            else
            {
                var direction2 = new Vector2(Direction.X * _matrix.M11, Direction.Y * _matrix.M22);
                Vector2 nextPosition =
                    Vector2.Transform(new Vector2(StartPosition.X + 50, StartPosition.Y + 37), _matrix) + direction2;


                var nextHeroRectangle = new RectangleF(nextPosition.X, nextPosition.Y, 15 * 3, 43 * 3);
                if (!Globals.MapController.IsMovePossible(nextHeroRectangle)) return Vector2.Zero;
            }
        }

        return Direction;
    }
}