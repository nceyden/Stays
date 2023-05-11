using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Stays.source
{
    internal class Player : Entity
    {
        public Vector2 velocity;
        public float playerSpeed = 2;
        public Animation playerAnimation;

        public Player(Texture2D sprite)
        {
            spriteSheet = sprite;
            velocity = new Vector2();
            playerAnimation = new Animation(spriteSheet);
        }
        public override void Update()
        {
            KeyboardState keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(Keys.A))
            {
                velocity.X -= playerSpeed;
            }
            if (keyboard.IsKeyDown(Keys.D))
            {
                velocity.X += playerSpeed;
            }

            position = velocity;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            playerAnimation.Draw(spriteBatch, position);
            //spriteBatch.Draw(spriteSheet, position, Color.White);
        }

    }
}