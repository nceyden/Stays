using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Stays.source
{
    public class Animation
    {
        Texture2D spriteSheet;
        int frames; // fps
        int rows = 0; // строка с отрисовкой кадров
        int count = 0; // счетчик
        float timeSinceLastFrame = 0;
        public Animation(Texture2D spriteSheet, float width = 32, float height = 32)
        {
            this.spriteSheet = spriteSheet;
            frames = (int)(spriteSheet.Width / width);
            Console.WriteLine(frames);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime, float milisecondsPerFrames = 500)
        {
            if (count < frames)
            {
                var rectangle = new Rectangle(32 * count, rows, 32, 32);
                spriteBatch.Draw(spriteSheet, position, rectangle, Color.White);
                timeSinceLastFrame += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (timeSinceLastFrame > milisecondsPerFrames)
                {
                    timeSinceLastFrame -= milisecondsPerFrames;
                    count++;
                    if (count ==  frames)
                    {
                        count = 0;
                    }
                }
            }
        }
    }
}