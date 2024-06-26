﻿using System.Collections.Generic;
using GetOut.Program;
using GetOut.View;
using Microsoft.Xna.Framework;

namespace GetOut.Controllers;

public class AnimationController
{
    private readonly Dictionary<object, Animation> _anims = new(); // Словарь анимация
    private object _lastKey; // Поледний использованный ключ для словаря

    private readonly float _periodBlinkingInSeconds = 0.2f; // Период мигания
    private float _elapsedTimeFromLastBlink = 0f; // Время с проследнего "мига"
    private bool _hideOnBlink = false; // Скрытие при мигании

    public void AddAnimation(object key, Animation animation)
    {
        _anims.Add(key, animation);
        _lastKey ??= key;
    }

    public void DeleteAnimation(object key)
    {
        _anims.Remove(key);
    }

    public void Update(object key) // Обновление ключа (переключение анимации)
    {
        if (_anims.TryGetValue(key, out Animation value))
        {
            ResetAnimation(key);
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

    public void Draw(Vector2 position, bool blinking = false)
    {
        if (blinking)
        {
            if (_elapsedTimeFromLastBlink > _periodBlinkingInSeconds)
            {
                _elapsedTimeFromLastBlink = 0;
                _hideOnBlink = !_hideOnBlink;
            }
            else
            {
                _elapsedTimeFromLastBlink += Globals.TotalSeconds;
            }
        }
        else _hideOnBlink = false;

        if (!_hideOnBlink) _anims[_lastKey].Draw(position);
    }

    public void DrawFromKey(object key, Vector2 position)
    {
        _anims[key].Draw(position);
    }

    public bool IsAnimationFinished(object key)
    {
        return _anims[key].HasCompletedCycle;
    }

    public void DrawFrame(object key, Vector2 position, int frame)
    {
        _anims[key].DrawFrame(position, frame);
    }

    private void ResetAnimation(object key)
    {
        if (!_lastKey.Equals(key))
        {
            _anims[key].Reset();
        }
    }
}