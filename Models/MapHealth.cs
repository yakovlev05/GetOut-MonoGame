using System.Collections.Generic;
using System.Linq;
using GetOut.Controllers;
using GetOut.Program;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;

namespace GetOut.Models;

public class MapHealth : IEntityInterface
{
    public Hero Hero { get; set; }
    public MapController MapController { get; set; }

    private int StartCountHealthOnMap { get; init; }
    private Dictionary<string, TiledMapTileLayer> HealthLayers { get; set; } = new(); // Каждый объект - один слой
    private Dictionary<string, Point> HealthPoints { get; set; } = new(); // Координаты каждого зелья

    public MapHealth(int startCountHealthOnMap)
    {
        StartCountHealthOnMap =
            startCountHealthOnMap; // Один слой - одно зелье, поэтому количество слоёв = количеству зелий
        // Название слоя: health_0, health_1 ...
    }

    public void Update()
    {
        var layerName = IsHeroIntersects();
        if (IsHeroIntersects() != null)
        {
            if (HealthLayers[layerName].IsVisible) Hero.Hearts.Increase();
            HealthLayers[layerName].IsVisible = false;
        }
    }

    public void Draw()
    {
    }

    public void Init()
    {
        for (int i = 0; i < StartCountHealthOnMap; i++)
        {
            HealthLayers.Add($"health_{i}", MapController.GetTileLayer($"health_{i}"));
            HealthPoints.Add($"health_{i}", MapController.GetPointsInTileGrid($"health_{i}").First());
        }
    }

    private string IsHeroIntersects() // Возвращает слой, если да, иначе null
    {
        foreach (var point in HealthPoints)
        {
            var pointInCord = Globals.Camera.WorldToScreen(point.Value.X, point.Value.Y);
            var healthRect = new RectangleF(pointInCord.X, pointInCord.Y, 16 * Globals.Camera.Zoom,
                16 * Globals.Camera.Zoom);
            if (Hero.GetDefaultRectangleInScreenCord().Intersects(healthRect)) return point.Key;
        }

        return null;
    }
}