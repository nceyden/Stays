using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Stays.source
{
    public class Bullet
    {
        private Texture2D _bulletTexture; // спрайт пули
        private float _speed; // скорость
        public Rectangle hitbox; // ее хитбокс

        public Bullet(Texture2D bulletTexture, float speed, Rectangle hitbox)
        {
            _bulletTexture = bulletTexture;
            _speed = speed;
            this.hitbox = hitbox;
        }

        public void Update()
        {
            hitbox.X += (int)_speed; // увеличение скорости пули с каждым кадром

        }
        public void Draw(SpriteBatch spriteBatch)  
        {
            spriteBatch.Draw(_bulletTexture, hitbox, Color.White);
        }
    }
}
