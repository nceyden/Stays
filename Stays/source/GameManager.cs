using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stays.source
{
    public class GameManager
    {
        private Rectangle endPoint;
        public GameManager(Rectangle endPoint)
        {
            this.endPoint = endPoint;
        }

        public bool isGameEnded(Rectangle playerHitbox)
        {
            return endPoint.Intersects(playerHitbox);
        }
    }
}
