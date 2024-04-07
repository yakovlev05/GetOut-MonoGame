using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GetOut.Services;

public class MazeLoader
{
    public static IEnumerable<string> LoadNamesFromFolder(DirectoryInfo folder)
    {
        return folder
            .GetFiles()
            .Where(file => file.Name.EndsWith(".txt"))
            .Select(file => Path.GetFileNameWithoutExtension(file.Name));
    }

    public static string[][] LoadMazeFromFolder(DirectoryInfo folder, string name)
    {
        var file = folder.GetFiles($"{name}.txt").Single();
        var lineInFile = File.ReadAllText(file.FullName);
        return GetMazeFromLines(lineInFile);
    }

    public static string[][] GetMazeFromLines(string lines)
    {
        return lines
            .Split("\r\n")
            .Select(line => line.Split(' '))
            .ToArray();
    }
}