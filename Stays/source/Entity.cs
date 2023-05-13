using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Stays.source
{
    public abstract class Entity
    {
        public enum currentAnimation
        {
            Idle,
            Run,
            Jumping,
            Falling
        }
        
        public Vector2 position;   // позиция на экране
        public Rectangle hitbox;   //  хитбокс в виде прямоугольника

        public abstract void Update();
        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);
    }
}