using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GetOut;

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

        _anims.AddAnimation(new Vector2(0,0), new Animation(texture, 10,3, 0.1f, 2));
        _anims.AddAnimation(new Vector2(1, 0), new Animation(texture, 10, 3, 0.1f, 3));
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