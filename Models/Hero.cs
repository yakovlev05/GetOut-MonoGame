using GetOut.Controllers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GetOut.Models;

public class Hero
{
    private Vector2 _position;
    private float _speed;
    private readonly AnimationManager _anims = new();

    public Hero(Vector2 position, float speed)
    {
        var texture = Globals.Content.Load<Texture2D>("./hero");
        _position = position;
        _speed = speed;

        _anims.AddAnimation(new Vector2(0, 0), new Animation(texture, 12, 8, 0.1f, 5, 10)); // Состояние в покое
        _anims.AddAnimation(new Vector2(1, 0), new Animation(texture, 12, 8, 0.1f, 8, 10)); // Движение вправо
        _anims.AddAnimation(new Vector2(1, 1), new Animation(texture, 12, 8, 0.1f, 8, 10)); //  Движение вправо вниз
        _anims.AddAnimation(new Vector2(0, 1), new Animation(texture, 12, 8, 0.1f, 8, 10)); // Движение вниз
        _anims.AddAnimation(new Vector2(-1, 1), new Animation(texture, 12, 8, 0.1f, 8, 10, true)); // Движение влево вниз
        _anims.AddAnimation(new Vector2(-1,0), new Animation(texture, 12, 8, 0.1f, 8, 10, true)); // Движение влево
        _anims.AddAnimation(new Vector2(-1,-1), new Animation(texture, 12, 8, 0.1f, 8, 10, true)); // Движение влево вверх
        _anims.AddAnimation(new Vector2(0,-1), new Animation(texture, 12, 8, 0.1f, 8, 10)); // Движение вверх
        _anims.AddAnimation(new Vector2(1,-1), new Animation(texture, 12, 8, 0.1f, 8, 10)); // Движение вправо вверх
        
    }

    public void Update()
    {
        if (InputManager.Moving)
        {
            _position += Vector2.Normalize(InputManager.Direction) * _speed * Globals.TotalSeconds;
        }

        _anims.Update(InputManager.Direction);
    }

    public void Draw()
    {
        _anims.Draw(_position);
    }
}