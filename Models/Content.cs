using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GetOut.Models;

public class Content
{
    public readonly Dictionary<string, Texture2D> Textures = new();

    public Content(ContentManager content)
    {
        Textures.Add("wall", content.Load<Texture2D>("walls"));
    }
}