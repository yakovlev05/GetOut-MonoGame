using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;

namespace GetOut.Controllers;

public class MapController
{
    private TiledMap Map { get; set; }
    private List<Rectangle> Walls { get; set; }

    public MapController(TiledMap map)
    {
        Map = map;
        Walls = GetRectanglesFromLayer("walls");
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
            if (x!=0) Console.WriteLine();
            rectangles.Add(new Rectangle(x, y, width, height));
        }

        return rectangles;
    }

    public bool IsMovePossible(Rectangle hero)
    {
        // // Уменьшаем размер прямоугольника героя на overlapAllowed пикселей
        // var adjustedHero = new Rectangle(hero.X + overlapAllowed, hero.Y + overlapAllowed, 
        //     hero.Width - 2 * overlapAllowed, hero.Height - 2 * overlapAllowed);
        foreach (var wall in Walls)
        {
            if (hero.Intersects(wall)) return false;
        }

        return true;
    }
}