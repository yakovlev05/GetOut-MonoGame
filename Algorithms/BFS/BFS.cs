using System.Collections.Generic;
using System.Linq;
using GetOut.Controllers;
using Microsoft.Xna.Framework;

namespace GetOut.Algorithms.BFS;

public static class Bfs
{
    public static List<Point> FindPath(MapCell[,] map, Point start, Point target)
    {
        var visited = new HashSet<Point>();
        var queue = new Queue<SinglyLinkedList<Point>>();
        queue.Enqueue(new SinglyLinkedList<Point>(start, null));
        visited.Add(start);

        while (queue.Count != 0)
        {
            var list = queue.Dequeue();
            var point = list.Value;

            if (!(point.X >= 0 && point.Y >= 0 && point.X < 81 && point.Y < 81) ||
                map[point.X, point.Y] == MapCell.Wall) continue;
            if (target == point) return list.Reverse().ToList();

            foreach (var offset in PossibleDirections)
            {
                var nextPoint = new Point(point.X + offset.X, point.Y + offset.Y);
                if (visited.Contains(nextPoint)) continue;
                queue.Enqueue(new SinglyLinkedList<Point>(nextPoint, list));
                visited.Add(nextPoint);
            }
        }

        return new List<Point>();
    }

    private static readonly List<Point> PossibleDirections = new List<Point>
    {
        new Point(0, -1),
        new Point(0, 1),
        new Point(-1, 0),
        new Point(1, 0)
    };
}