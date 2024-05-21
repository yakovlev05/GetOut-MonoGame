using System;
using System.Collections.Generic;
using System.Linq;
using GetOut.Controllers;
using GetOut.Program;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;

namespace GetOut.Models;

public class MapPeaks : IEntityInterface
{
    public Hearts Hearts { get; set; }
    public Hero Hero { get; set; }
    public bool StaticPosition { get; init; } = false; // Не имеет значения, это карта
    public bool RequireMapController { get; init; } = true;

    public MapController MapController { get; set; }
    public TiledMapTilesetAnimatedTile AnimatedTile { get; set; } // Они двигаются синхронно, одного хватит
    public TiledMapTilesetTileAnimationFrame DamageFrame { get; set; }
    public List<Point> ListPointsInScreenCordsPeaks { get; set; }

    public bool IsDamageFrameKnow =>
        AnimatedTile.CurrentAnimationFrame.LocalTileIdentifier == DamageFrame.LocalTileIdentifier;

    public void Update()
    {
        if (IsHeroIntersects())  Console.WriteLine("ПИКИ ПИКИ ПИКИ");
    }

    public void Draw()
    {
    }

    public void Init()
    {
        AnimatedTile = MapController.GetAnimatedTile("Levels\\assets\\peaks_0");
        DamageFrame = AnimatedTile.AnimationFrames.Last();

        ListPointsInScreenCordsPeaks = MapController
            .GetPointsInTileGrid("peaks")
            .ToList();
    }

    public bool IsHeroIntersects()
    {
        return ListPointsInScreenCordsPeaks
            .Select(x => Globals.Camera.WorldToScreen(x.X, x.Y))
            .Select(x => new RectangleF(x.X, x.Y, 16 * Globals.Camera.Zoom, 16 * Globals.Camera.Zoom))
            .Any(x => x.Intersects(Hero.GetRectangleInScreenCord()));
    }
}