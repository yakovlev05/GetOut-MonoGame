using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;

namespace GetOut.Program.Screens;

public class LevelScreen : GameScreen
{
    private new Game1 Game => (Game1)base.Game;

    public LevelScreen(Game game) : base(game)
    {
    }

    public override void Update(GameTime gameTime)
    {
        throw new System.NotImplementedException();
    }

    public override void Draw(GameTime gameTime)
    {
        throw new System.NotImplementedException();
    }
}