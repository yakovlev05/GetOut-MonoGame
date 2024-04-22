using GetOut.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GetOut.View;

public class DrawPlayer
{
    public static void Draw(SpriteBatch spriteBatch, PlayerModel player)
    {
        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
        spriteBatch.Draw(player.Texture, player.Position, player.Rectangle, Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0f);
        spriteBatch.End();
    }
}