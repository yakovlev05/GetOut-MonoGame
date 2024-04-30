using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GetOut.View;

public static class Camera
{
    public static Vector2 GetMovementDirection()
    {
        var movementDirection = Vector2.Zero;
        var state = Keyboard.GetState();
        if (state.IsKeyDown(Keys.Down) || state.IsKeyDown(Keys.S))
            movementDirection += Vector2.UnitY;

        if (state.IsKeyDown(Keys.Up) || state.IsKeyDown(Keys.W))
            movementDirection -= Vector2.UnitY;

        if (state.IsKeyDown(Keys.Left) || state.IsKeyDown(Keys.A))
            movementDirection -= Vector2.UnitX;

        if (state.IsKeyDown(Keys.Right) || state.IsKeyDown(Keys.D))
            movementDirection += Vector2.UnitX;

        return movementDirection;
    }
}