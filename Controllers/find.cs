using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GetOut.Controllers;
using Microsoft.Xna.Framework;

namespace Dungeon;

public class BfsTask
{
    public static IEnumerable<SinglyLinkedList<Point>> FindPaths(MapCell[,] map, Point start, Point[] targets)
    {
        var visited = new HashSet<Point>();
        var chestsHash = targets.ToHashSet();
        var queue = new Queue<SinglyLinkedList<Point>>();
        queue.Enqueue(new SinglyLinkedList<Point>(start, null));
        visited.Add(start);

        while (queue.Count != 0)
        {
            var list = queue.Dequeue();
            var point = list.Value;

            if (!(point.X >= 0 && point.Y >= 0 && point.X < 81 && point.Y < 81) ||
                map[point.X, point.Y] == MapCell.Wall) continue;
            if (chestsHash.Contains(point)) yield return list;

            foreach (var offset in PossibleDirections)
            {
                var nextPoint = new Point(point.X + offset.X, point.Y + offset.Y);
                if (visited.Contains(nextPoint)) continue;
                queue.Enqueue(new SinglyLinkedList<Point>(nextPoint, list));
                visited.Add(nextPoint);
            }
        }
    }

    public static List<Point> PossibleDirections = new List<Point>
    {
        new Point(0, -1),
        new Point(0, 1),
        new Point(-1, 0),
        new Point(1, 0)
    };
}

public class SinglyLinkedList<T> : IEnumerable<T>
{
    public readonly T Value;
    public readonly SinglyLinkedList<T> Previous;
    public readonly int Length;

    public SinglyLinkedList(T value, SinglyLinkedList<T> previous = null)
    {
        Value = value;
        Previous = previous;
        Length = previous?.Length + 1 ?? 1;
    }

    public IEnumerator<T> GetEnumerator()
    {
        yield return Value;
        var pathItem = Previous;
        while (pathItem != null)
        {
            yield return pathItem.Value;
            pathItem = pathItem.Previous;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}