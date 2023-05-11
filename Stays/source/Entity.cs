using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Stays.source
{
    public abstract class Entity
    {
        public Texture2D spriteSheet;
        public Vector2 position;

        public abstract void Update();
        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);
    }
}