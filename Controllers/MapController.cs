﻿using System.Collections.Generic;
using System.Linq;
using GetOut.Models;
using GetOut.Program;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

namespace GetOut.Controllers;

public class MapController
{
    public List<Point> Walls { get; set; }
    public List<Point> Flags { get; set; }

    private OrthographicCamera Camera { get; init; }

    private TiledMapRenderer TiledMapRenderer { get; init; }
    private TiledMap TiledMap { get; set; }
    public MapCell[,] Map { get; private set; }

    public MapController(string pathMap, OrthographicCamera camera)
    {
        TiledMap = Globals.Content.Load<TiledMap>(pathMap);
        TiledMapRenderer = new TiledMapRenderer(Globals.GraphicsDevice, TiledMap);

        Camera = camera;
        Walls = GetPointsInTileGrid("walls");
        Flags = GetPointsInTileGrid("flag");
        Map = GetMapCells();
    }

    public List<Point> GetPointsInTileGrid(string layerName)
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
            var wallRect = new RectangleF(cord.X, cord.Y, 16 * Camera.Zoom, 16 * Camera.Zoom);
            if (hero.Intersects(wallRect)) return false;
        }

        return true;
    }

    public void Update(GameTime gameTime) => TiledMapRenderer.Update(gameTime);

    public void Draw() => TiledMapRenderer.Draw(Camera.GetViewMatrix());

    public bool IsExit(Hero hero)
    {
        return Flags
            .Select(x => Camera.WorldToScreen(x.X, x.Y))
            .Select(x => new RectangleF(x.X, x.Y, 16 * Camera.Zoom, 16 * Camera.Zoom))
            .Any(x => x.Intersects(new RectangleF(hero.StartPosition.X + Hero.OffsetPositionX,
                hero.StartPosition.Y + Hero.OffsetPositionY, Hero.WidthHero * Camera.Zoom,
                Hero.HeightHero * Camera.Zoom)));
    }

    public TiledMapTilesetAnimatedTile GetAnimatedTile(string layerName)
    {
        var tileset = TiledMap.Tilesets.First(t => t.Name == layerName);
        var myTile = tileset.Tiles.First();
        var animatedTile = (TiledMapTilesetAnimatedTile)myTile;
        return animatedTile;
    }

    public TiledMapTileLayer GetTileLayer(string layerName)
    {
        return TiledMap.GetLayer<TiledMapTileLayer>(layerName);
    }

    public MapCell[,] GetMapCells()
    {
        var map = new MapCell[TiledMap.Width, TiledMap.Height];

        var walls = TiledMap.GetLayer<TiledMapTileLayer>("walls").Tiles;

        foreach (var tile in walls)
        {
            if (tile.GlobalIdentifier != 0)
            {
                map[tile.X, tile.Y] = MapCell.Wall;
            }
            // map[tile.X,tile.Y] = tile.GlobalIdentifier != 0 ? MapCell.Empty : MapCell.Wall;
        }

        return map;
    }
}

public enum MapCell
{
    Empty,
    Wall
}