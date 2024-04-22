using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace GetOut.Models;

public class MazeModel
{
    public Point StartPoint { get; init; }
    public Point FinishPoint { get; init; }
    public string HeroSprite { get; init; }
    public int MazeWidth { get; init; }
    public int MazeHeight { get; init; }
    public string MazeAssetsFolder { get; init; }
    public MazeCellModel[,] Maze { get; init; }
    public int CellHeight { get; init; } = 16;
    public int CellWidth { get; init; } = 16; // TODO: Доделать получение размеров 

    public MazeModel(DirectoryInfo mazePath)
    {
        var mazeData = File.ReadAllLines(mazePath.FullName);
        StartPoint = new Point(int.Parse(mazeData[1].Split(',')[0]), int.Parse(mazeData[1].Split(',')[1]));
        FinishPoint = new Point(int.Parse(mazeData[4].Split(',')[0]), int.Parse(mazeData[4].Split(',')[1]));
        HeroSprite = mazeData[7];
        MazeWidth = int.Parse(mazeData[13].Split('*')[0]);
        MazeHeight = int.Parse(mazeData[13].Split('*')[1]);
        MazeAssetsFolder = mazeData[16];
        Maze = LoadMaze(mazeData.Skip(18).ToArray());
    }

    private MazeCellModel[,] LoadMaze(string[] maze)
    {
        var lengthY = maze.Length;
        var lengthX = maze[0].Split().Length;
        var mazeArray = new MazeCellModel[lengthX, lengthY];
        for (var y = 0; y < lengthY; y++)
        {
            var splitMaze = maze[y].Split();
            var x = 0;
            foreach (var cell in splitMaze)
            {
                var splitCell = cell.Split('-');
                var texture = ContentModel.Textures[MazeAssetsFolder][splitCell[0]];
                var tileX = int.Parse(splitCell[1]);
                var tileY = int.Parse(splitCell[2]);
                var width = int.Parse(splitCell[3]);
                var height = int.Parse(splitCell[4]);
                var mazeCell = new MazeCellModel(texture, tileX, tileY, width, height);
                mazeArray[x, y] = mazeCell;
                x++;
            }
        }

        return mazeArray;
    }
}