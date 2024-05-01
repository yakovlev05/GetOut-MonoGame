using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GetOut.Models;

public class ContentModel
{
    public static Dictionary<string, Dictionary<string, Texture2D>>
        Textures { get; private set; } // Папка:Название текстуры - текстура

    public ContentModel(ContentManager contentManager)
    {
        Textures = new Dictionary<string, Dictionary<string, Texture2D>>();
        var folders = new DirectoryInfo(contentManager.RootDirectory).GetDirectories();
        foreach (var folder in folders)
        {
            if (folder.Name == "bin" || folder.Name == "obj" || folder.Name=="MainMenu" || folder.Name=="level1") continue;

            var textures = new Dictionary<string, Texture2D>();
            var files = folder.GetFiles();
            foreach (var file in files)
            {
                var name = Path.GetFileNameWithoutExtension(file.Name);
                textures.Add(name,
                    contentManager.Load<Texture2D>($"./{folder.Name}/{Path.GetFileNameWithoutExtension(file.Name)}"));
            }

            Textures.Add(folder.Name, textures);
        }
    }
}