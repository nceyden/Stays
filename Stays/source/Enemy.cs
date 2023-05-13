using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Stays.source
{
    public class Enemy: Entity
    {
        Animation enemyAnimation; //    объект класса Animation, содержит анимацию врага
        float speed = 2; //    скорость
        Rectangle pathWay; //   место движения врага
        private bool _isFacingRight = true; // смотрит враг вправо или влево

        public Enemy(Texture2D enemySpriteSheet, Rectangle pathWay, float speed = 2)
        {
            enemyAnimation = new Animation(enemySpriteSheet);
            this.pathWay = pathWay;
            position = new Vector2(pathWay.X, pathWay.Y);
            hitbox = new Rectangle(pathWay.X, pathWay.Y, 16, 16);
            this.speed = speed;
        }

        public override void Update()
        {
            if (!pathWay.Contains(hitbox))
            {
                speed = -speed;
                _isFacingRight = !_isFacingRight;    // поворот в противополную сторону
            }
            position.X += speed;

            hitbox.X = (int)position.X;
            hitbox.Y = (int)position.Y;   // обновляем координаты хитбокса в соответсвии с позицией енеми

        }
        public bool hasHit(Rectangle playerRect)
        {
            return hitbox.Intersects(playerRect);

        }
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (_isFacingRight)
                enemyAnimation.Draw(spriteBatch, position, gameTime, 150);   // рисуем енеми в зависимости от того, в какую сторону смотрит
            else
                enemyAnimation.Draw(spriteBatch, position, gameTime, 150, SpriteEffects.FlipHorizontally);
        }
    }
}
