using GetOut.Models;

namespace GetOut.Services;

public class MazeToGraphic
{
    public static string[,] ConvertMazeToGraphic(string[][] maze)
    {
        var height = maze.Length;
        var width = maze[0].Length;
        var graphicMaze = new string[width, height];
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                graphicMaze[x, y] = Wall.ConvertToGraphic(maze[y][x]);
                // graphicMaze[x, y] = maze[y][x];
            }
        }

        return graphicMaze;
    }
}