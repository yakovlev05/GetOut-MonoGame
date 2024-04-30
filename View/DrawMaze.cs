using GetOut.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace GetOut.View;

public class DrawMaze
{
    public static void Draw(SpriteBatch spriteBatch, MazeModel maze, OrthographicCamera camera)
    {
        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, transformMatrix: camera.GetViewMatrix());
        var xx = (1920 - 16 * 8) / 2;
        var yy = (1080 - 16 * 5) / 2;
        var currentPosition = new Vector2(xx, yy);
        for (var x = 0; x < maze.Maze.GetLength(0); x++)
        {
            for (var y = 0; y < maze.Maze.GetLength(1); y++)
            {
                var cell = maze.Maze[x, y];
                // currentPosition = new Vector2(x * 16, y * 16);
                spriteBatch.Draw(
                    cell.Texture,
                    currentPosition,
                    cell.Rectangle,
                    Color.White,
                    0f,
                    Vector2.Zero,
                    1f,
                    SpriteEffects.None,
                    0f
                );
                currentPosition += new Vector2(0, maze.CellHeight);
            }

            currentPosition += new Vector2(maze.CellWidth, 0);
            currentPosition.Y = yy;
        }

        spriteBatch.End();
    }
}