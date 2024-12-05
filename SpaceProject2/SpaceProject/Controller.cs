using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceProject
{
    public class Controller
    {
        private TimeSpan elapsedTime;
        private int secondsElapsed;
        public int score;

        public Controller()
        {
            elapsedTime = TimeSpan.Zero;
            secondsElapsed = 0;
            score = 0;
        }

        // Update the timer and return the seconds elapsed
        public int updateTime(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime.TotalSeconds >= 1)
            {
                secondsElapsed++;
                elapsedTime = TimeSpan.Zero;
            }

            return secondsElapsed; // Return the current seconds count
        }

    }
}
