using System;
using System.Collections.Generic;
using GetOut.Models;
using GetOut.Models.MapEntity;
using GetOut.Models.Monsters.Smart;
using GetOut.Models.Monsters.Stupid;
using Microsoft.Xna.Framework;

namespace GetOut.Controllers;

public static class LevelsEntitiesRegistry
{
    public static List<IEntityInterface> GetLevel1()
    {
        return new List<IEntityInterface>()
        {
            new MapPeaks(),
            new MapHealth(2),
            new PumpkinDude(new Vector2(37 * 16, 25 * 16), 1f, 30),
            new BigZombie(new Vector2(12 * 16, 51 * 16), new List<Tuple<Point, int>>()
            {
                new(new Point(10 * 16, 50 * 16), 5),
                new(new Point(10 * 16, 46 * 16), 0),
                new(new Point(22 * 16, 46 * 16), 0),
                new(new Point(22 * 16, 50 * 16), 5), //
                new(new Point(22 * 16, 46 * 16), 0),
                new(new Point(10 * 16, 46 * 16), 0),
            }),
            new BigOgre(new Vector2(23 * 16, 66 * 16), new List<Tuple<Point, int>>()
            {
                new(new Point(37 * 16, 66 * 16), 0),
                new(new Point(26 * 16, 66 * 16), 0)
            }),
            new BigDemon(new Vector2(57 * 16, 29 * 16), new List<Tuple<Point, int>>()
            {
                new(new Point(57 * 16, 29 * 16), 3),
                new(new Point(57 * 16, 33 * 16), 0),
                new(new Point(61 * 16, 33 * 16), 0),
                new(new Point(61 * 16, 25 * 16), 3), //
                new(new Point(61 * 16, 33 * 16), 0),
                new(new Point(57 * 16, 33 * 16), 0),
            }),
            new BigZombie(new Vector2(55 * 16, 79 * 16), new List<Tuple<Point, int>>()
            {
                new(new Point(66 * 16, 73 * 16), 7),
                new(new Point(74 * 16, 73 * 16), 3)
            })
        };
    }

    public static List<IEntityInterface> GetLevel2()
    {
        return new List<IEntityInterface>()
        {
            new MapPeaks(),
            new MapHealth(1)
        };
    }
}