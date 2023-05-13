using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Stays.source
{
    public class Player : Entity
    {
        public Vector2 velocity;
        public Rectangle playerFallRect;
        public SpriteEffects effects;

        public float playerSpeed = 1;
        public float fallSpeed = 8;
        public float jumpingSpeed = -10;

        public bool isFalling = true;
        public bool isJumping;
        public bool isShooting;

        public float startY;

        public bool isIntersecting = false;

        public Animation[] playerAnimation;
        public currentAnimation playerAnimationController;

        public Player(Vector2 position, Texture2D idleSprite, Texture2D runSprite, Texture2D jumpSprite, Texture2D fallSprite)
        {
            playerAnimation = new Animation[4];

            this.position = position;
            velocity = new Vector2();
            effects = SpriteEffects.None;

            playerAnimation[0] = new Animation(idleSprite);
            playerAnimation[1] = new Animation(runSprite);
            playerAnimation[2] = new Animation(jumpSprite);
            playerAnimation[3] = new Animation(fallSprite);
            hitbox = new Rectangle((int)position.X, (int)position.Y, 32, 32);
            playerFallRect = new Rectangle((int)position.X + 3, (int)position.Y + 32, 32, (int)fallSpeed);
        }
        public override void Update()
        {
            KeyboardState keyboard = Keyboard.GetState();
            playerAnimationController = currentAnimation.Idle;

            isShooting = keyboard.IsKeyDown(Keys.Enter);
            startY = position.Y;
            Move(keyboard);
            Jump(keyboard);

            if (isFalling)
            {
                velocity.Y += fallSpeed;
                playerAnimationController = currentAnimation.Falling;
            }

            position = velocity;
            hitbox.X = (int)position.X;
            hitbox.Y = (int)position.Y;
            playerFallRect.X = (int)position.X;
            playerFallRect.Y = (int)position.Y;
        }

        private void Move(KeyboardState keyboard)
        {
            if (keyboard.IsKeyDown(Keys.A))
            {
                velocity.X -= playerSpeed;
                playerAnimationController = currentAnimation.Run;
                effects = SpriteEffects.FlipHorizontally;
            }
            if (keyboard.IsKeyDown(Keys.D))
            {
                velocity.X += playerSpeed;
                playerAnimationController = currentAnimation.Run;
                effects = SpriteEffects.FlipHorizontally;
            }
        }

        private void Jump(KeyboardState keyboard)
        {
            if (isJumping)
            {
                velocity.Y += jumpingSpeed;
                jumpingSpeed += 1;
                Move(keyboard);
                playerAnimationController = currentAnimation.Jumping; 

                if (velocity.Y >= startY) //   если положение по Y больше, чем у земли
                {
                    velocity.Y = startY;    //остановка прыжка
                    isJumping = false;

                }
            }
            else
            {
                if (keyboard.IsKeyDown(Keys.Space) && !isFalling)
                {
                    isJumping = true;
                    isFalling = false;
                    jumpingSpeed = -14;
                }
            }
        }
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            switch (playerAnimationController)
            {
                case currentAnimation.Idle:
                    playerAnimation[0].Draw(spriteBatch, position, gameTime, 500, effects);
                    break;
                case currentAnimation.Run:
                    playerAnimation[1].Draw(spriteBatch, position, gameTime, 100, effects);
                    break;
                case currentAnimation.Jumping:
                    playerAnimation[3].Draw(spriteBatch, position, gameTime, 100, effects);
                    Console.WriteLine("Jumping");
                    break;
                case currentAnimation.Falling:
                    playerAnimation[4].Draw(spriteBatch, position, gameTime, 600, effects);
                    Console.WriteLine("Falling");
                    break;
            }       
        }
    }
}