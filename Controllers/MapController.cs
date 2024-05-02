using System;
using System.Collections.Generic;
using GetOut.Program;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;

namespace GetOut.Controllers;

public class MapController
{
    private TiledMap Map { get; set; }
    private List<Rectangle> Walls { get; set; }
    public List<Point> Walls1 { get; set; }

    public MapController(TiledMap map)
    {
        Map = map;
        Walls = GetRectanglesFromLayer("walls");
        Walls1 = GetPointsInTileGrid("walls");
    }

    private List<Rectangle> GetRectanglesFromLayer(string layerName)
    {
        var rectangles = new List<Rectangle>();

        var layer = Map.GetLayer<TiledMapTileLayer>(layerName);
        var width = layer.TileWidth;
        var height = layer.TileHeight;
        foreach (var tile in layer.Tiles)
        {
            // if (tile.GlobalIdentifier == 0) continue; // Skip tiles with GID 0, they are empty tiles

            var x = tile.X;
            var y = tile.Y;
            if (x != 0) Console.WriteLine();
            rectangles.Add(new Rectangle(x, y, width, height));
        }

        return rectangles;
    }

    private List<Point> GetPointsInTileGrid(string layerName)
    {
        var result = new List<Point>();
        var layer = Map.GetLayer<TiledMapTileLayer>(layerName);
        foreach (var tile in layer.Tiles)
        {
            if (tile.GlobalIdentifier == 0) continue; // Skip tiles with GID 0, they are empty tiles
            var x = tile.X;
            var y = tile.Y;
            result.Add(new Point(x * 16, y * 16));
        }

        return result;
    }

    public bool IsMovePossible(RectangleF hero)
    {
        // var overlapAllowed = 5;
        // var adjustedHero = new RectangleF(hero.X, hero.Y,
        //     hero.Width - 2 * overlapAllowed, hero.Height - 2 * overlapAllowed);

        foreach (var point in Walls1)
        {
            // if (point.X == 30 || point.X == 29 * 16) Console.WriteLine();

            var cord = Globals.Camera1.WorldToScreen(point.X, point.Y);
            var wallRect = new RectangleF(cord.X, cord.Y, 16, 16);
            if (hero.Intersects(wallRect)) return false;
        }

        return true;
    }
}