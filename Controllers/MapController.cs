using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

namespace GetOut.Controllers;

public class MapController
{
    public List<Point> Walls { get; private set; }
    private OrthographicCamera Camera { get; init; }

    private TiledMapRenderer TiledMapRenderer { get; init; }
    private TiledMap TiledMap { get; set; }

    public MapController(TiledMap tiledMap, TiledMapRenderer tiledMapRenderer, OrthographicCamera camera)
    {
        TiledMap = tiledMap;
        Camera = camera;
        TiledMapRenderer = tiledMapRenderer;
        Walls = GetPointsInTileGrid("walls");
    }

    private List<Point> GetPointsInTileGrid(string layerName)
    {
        return TiledMap
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

    public void Update(GameTime gameTime) => TiledMapRenderer.Update(gameTime);

    public void Draw() => TiledMapRenderer.Draw(Camera.GetViewMatrix());
}