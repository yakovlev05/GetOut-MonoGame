using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GetOut.Models;

public class MazeCellModel
{
    public int TileX { get; init; }
    public int TileY { get; init; }
    public int Width { get; init; }
    public int Height { get; init; }
    public Texture2D Texture { get; init; }
    public Rectangle Rectangle { get; init; }

    public MazeCellModel(Texture2D texture, int tileX, int tileY, int width, int height)
    {
        TileX = tileX;
        TileY = tileY;
        Width = width;
        Height = height;
        Texture = texture;
        Rectangle = new Rectangle(new Point(TileX, TileY), new Point(width, height));
    }
}