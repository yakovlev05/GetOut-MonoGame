using System.Collections.Generic;
using GetOut.Models;
using Microsoft.Xna.Framework;

namespace GetOut.Controllers;

public class AnimationController
{
    private readonly Dictionary<object, Animation> _anims = new (); // Словарь анимация
    private object _lastKey; // Поледний использованный ключ для словаря

    public void AddAnimation(object key, Animation animation)
    {
        _anims.Add(key, animation);
        _lastKey ??= key;
    }

    public void Update(object key) // Обновление ключа (переключение анимации)
    {
        if (_anims.TryGetValue(key, out Animation value))
        {
            value.Start();
            _anims[key].Update();
            _lastKey = key;
        }
        else
        {
            _anims[_lastKey].Stop();
            _anims[_lastKey].Reset();
        }
    }

    public void Draw(Vector2 position)
    {
        _anims[_lastKey].Draw(position);
    }
}