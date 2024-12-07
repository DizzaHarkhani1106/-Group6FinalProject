using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class Coins
    {
        public Vector2 position;
        public static int radius = 60;
        private int coinSize = 50;
        public bool isActive = true;
        private Random random = new Random();

        public Coins()
        {
            // SpawnCoin(1700, 900);
        }

        public void SpawnCoin(int screenWidth, int screenHeight, Random random)
        {
            int x = random.Next(radius, screenHeight - radius);
            int y = random.Next(radius, screenWidth - radius);
            position = new Vector2(x, y);
        }
    }
}