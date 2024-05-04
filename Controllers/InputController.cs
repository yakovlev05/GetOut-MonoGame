using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GetOut.Controllers;

public static class InputController
{
    private static Vector2 _direction;
    public static Vector2 Direction => _direction;
    public static bool Moving => _direction != Vector2.Zero;

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

    public static Keys IsPressedKey(Keys key) // Возвращает значение клаивиши, если она нажата, иначе null
    {
        return Keyboard.GetState().IsKeyDown(key) ? key : Keys.None;
    }
}