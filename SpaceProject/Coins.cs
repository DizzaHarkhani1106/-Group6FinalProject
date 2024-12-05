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
        public int radius;
        public bool IsCollected = false;
        public bool isActive = true; // Flag to check if coin is collected
        private Random random = new Random();

        public Coins()
        {
            
        }

        public void SpawnCoin(int screenWidth, int screenHeight, Random random)
        {
            int x = random.Next(radius, screenHeight - radius);
            int y = random.Next(radius, screenWidth - radius);
            position = new Vector2(x, y);
        }
    }
}