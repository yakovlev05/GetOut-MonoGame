using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;

namespace GetOut.Controllers;

public class MapController
{
    private TiledMap Map { get; set; }
    public List<Point> Walls { get; private set; }
    private OrthographicCamera Camera { get; init; }

    public MapController(TiledMap map, OrthographicCamera camera)
    {
        Map = map;
        Camera = camera;
        Walls = GetPointsInTileGrid("walls");
    }

    private List<Point> GetPointsInTileGrid(string layerName)
    {
        return Map
            .GetLayer<TiledMapTileLayer>(layerName).Tiles
            .Where(x => x.GlobalIdentifier != 0)
            .Select(x => new Point(x.X * 16, x.Y * 16))
            .ToList();
    }

    public bool IsMovePossible(RectangleF hero)
    {
        foreach (var point in Walls)
        {
            var cord = Camera.WorldToScreen(point.X, point.Y);
            var wallRect = new RectangleF(cord.X, cord.Y, 16 * 3, 16 * 3);
            if (hero.Intersects(wallRect)) return false;
        }

        return true;
    }
}