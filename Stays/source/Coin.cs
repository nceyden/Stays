using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Stays.source;

namespace Stays.source
{
    public class Coin
    {
        private Animation coinAnimation;
        public Coin(Texture2D sprite)
        {
            coinAnimation = new Animation(sprite, 16, 16);
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime)
        {
            coinAnimation.Draw(spriteBatch, position, gameTime);
        }
    }
}