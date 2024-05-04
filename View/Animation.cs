using System.Collections.Generic;
using GetOut.Program;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GetOut.View;

public class Animation
{
    private readonly Texture2D _texture; // Лист спрайтов
    private readonly List<Rectangle> _sourceRectangles = new(); // Лист прямоугольников для каждого спрайта
    private readonly int _frames; // Количество фреймов анимации
    private int _frame; // Текущий фрейм
    private readonly float _frameTime; // Время для смены кадров
    private float _frameTimeLeft; // Сколько времени прошло с поледней смены
    private bool _active = true; // Включена анимация или нет
    private bool IsFlipHorizontally { get; set; }

    public Animation(Texture2D texture, int framesX, int framesY, float frameTime, int row = 1,
        int chooseCountFrames = 1000, bool isFlipHorizontally = false)
    {
        IsFlipHorizontally = isFlipHorizontally;
        _texture = texture;
        _frameTime = frameTime;
        _frameTimeLeft = _frameTime;
        _frames = framesX;
        var frameWidth = _texture.Width / framesX;
        var frameHeight = _texture.Height / framesY;

        _frames = _frames < chooseCountFrames ? _frames : chooseCountFrames;
        for (int i = 0; i < _frames; i++)
        {
            _sourceRectangles.Add(new(i * frameWidth, (row - 1) * frameHeight, frameWidth, frameHeight));
        }
    }

    public void Stop()
    {
        _active = false;
    }

    public void Start()
    {
        _active = true;
    }

    public void Reset()
    {
        _frame = 0;
        _frameTimeLeft = _frameTime;
    }

    public void Update()
    {
        if (!_active) return;

        _frameTimeLeft -= Globals.TotalSeconds;

        if (_frameTimeLeft <= 0)
        {
            _frameTimeLeft += _frameTime;
            _frame = (_frame + 1) % _frames;
        }
    }

    public void Draw(Vector2 position)
    {
        var effect = IsFlipHorizontally ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        Globals.SpriteBatch.Draw(_texture, position, _sourceRectangles[_frame], Color.White, 0, Vector2.Zero,
            Vector2.One, effect, 1);
    }
}