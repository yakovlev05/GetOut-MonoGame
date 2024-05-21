using GetOut.Program;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace GetOut.Controllers;

public static class InputController
{
    private static Vector2 _direction;
    public static Vector2 Direction => _direction;
    public static bool Moving => _direction != Vector2.Zero;
    public static Point MousePosition => Mouse.GetState(Globals.Window).Position;

    public static void Update()
    {
        _direction = Vector2.Zero;
        var keyboardState = Keyboard.GetState();

        if (keyboardState.GetPressedKeyCount() > 0)
        {
            if (keyboardState.IsKeyDown(Keys.A)) _direction.X--;
            if (keyboardState.IsKeyDown(Keys.D)) _direction.X++;
            if (keyboardState.IsKeyDown(Keys.W)) _direction.Y--;
            if (keyboardState.IsKeyDown(Keys.S)) _direction.Y++;
        }
    }

    public static bool IsPressedKey(Keys key) // Возвращает значение клаивиши, если она нажата, иначе null
    {
        return Keyboard.GetState().IsKeyDown(key);
    }

    public static bool IsLeftButtonPressed()
    {
        return Mouse.GetState(Globals.Window).LeftButton == ButtonState.Pressed;
    }

    public static bool IsMouseInRectangle(RectangleF rectangleF)
    {
        var mouseRectangleF = new RectangleF(MousePosition.X, MousePosition.Y, 1, 1);
        return mouseRectangleF.Intersects(rectangleF);
    }

    public static Keys[] GetPressedKeys()
    {
        return Keyboard.GetState().GetPressedKeys();
    }
}