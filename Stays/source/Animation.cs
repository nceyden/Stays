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
        public Animation(Texture2D spriteSheet, float width = 32, float height = 32)
        {
            this.spriteSheet = spriteSheet;
            frames = (int)(spriteSheet.Width / width);
            Console.WriteLine(frames);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            if (count <= frames)
            {
                var rectangle = new Rectangle(32 * count, 0, 32, 32);
                spriteBatch.Draw(spriteSheet, rectangle, Color.White);
                count++;
            }
            else count = 0;
        }
    }
}