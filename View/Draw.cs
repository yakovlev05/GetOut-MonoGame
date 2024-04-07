using System.Linq;
using GetOut.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GetOut.View;

public class Draw
{
    public static void DrawMaze(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, string[,] maze,
        Content content)
    {
        // spriteBatch.Begin(SpriteSortMode.Deferred,BlendState.AlphaBlend,SamplerState.PointClamp);
        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
        for (var x = 0; x < maze.GetLength(0); x++)
        {
            for (var y = 0; y < maze.GetLength(1); y++)
            {
                var splitMaze = maze[x, y].Split("-");

                var texture = content.Textures[splitMaze.First()];
                var position = new Vector2(x * 16, y * 16);

                var frameX = int.Parse(splitMaze.Last());
                var frameY = int.Parse(splitMaze[1]);
                var rectangle =
                    new Rectangle(new Point(frameX - 16, frameY - 16),
                        new Point(16, 16)); // TODO: Исправить location в тайлсете

                spriteBatch.Draw(texture, position, rectangle, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None,
                    0f);
            }
        }

        spriteBatch.End();
    }
}