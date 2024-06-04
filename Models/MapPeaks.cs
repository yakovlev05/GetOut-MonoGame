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
    public Hero Hero { get; set; }
    public MapController MapController { get; set; }
    public ScoreStatistic ScoreStatistic { get; set; }

    private TiledMapTilesetAnimatedTile AnimatedTile { get; set; } // Они двигаются синхронно, одного хватит
    private TiledMapTilesetTileAnimationFrame DamageFrame { get; set; }
    private List<Point> ListPointsInScreenCordsPeaks { get; set; }

    private bool IsDamageFrameKnow =>
        AnimatedTile.CurrentAnimationFrame.LocalTileIdentifier == DamageFrame.LocalTileIdentifier;

    public int Score => 3;

    public void Update()
    {
        if (IsDamageFrameKnow && IsHeroIntersects())
        {
            Hero.Hearts.Decrease();
            UpdateScoreStatistic();
        }
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

    private bool IsHeroIntersects()
    {
        var heroOriginalRect = Hero.GetDefaultRectangleInScreenCord();
        heroOriginalRect.Height /= 3;
        heroOriginalRect.Y += heroOriginalRect.Height * Globals.Camera.Zoom * 2 / 3;

        return ListPointsInScreenCordsPeaks
            .Select(x => Globals.Camera.WorldToScreen(x.X, x.Y))
            .Select(x => new RectangleF(x.X, x.Y, 16 * Globals.Camera.Zoom, 16 * Globals.Camera.Zoom))
            .Any(x => x.Intersects(heroOriginalRect));
    }

    private void UpdateScoreStatistic()
    {
        ScoreStatistic.AddScore(Score);
    }
}