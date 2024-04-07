using System.Text;

namespace GetOut.Models;

public class Wall
{
    public static int Height { get; } = 16;
    public static int Width { get; } = 16;
    public static string File { get; } = "wall";

    public static string ConvertToGraphic(string wall)
    {
        var splitWall = wall.Split('-');
        var row = int.Parse(splitWall[1]);
        var column = int.Parse(splitWall[2]);
        var wallGraphic = $"{File}-{row * Height}-{column * Width}";
        return wallGraphic;
    }
}