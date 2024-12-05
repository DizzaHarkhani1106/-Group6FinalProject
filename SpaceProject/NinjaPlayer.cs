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
    public class NinjaPlayer
    {
        private bool isJumping = false; 
        private int jumpHeight = 300; 
        private int jumpSpeed = 9; 
        private int gravity = 9; 
        private int groundLevel = 500; 
        private int currentJumpHeight = 0;
        private bool isOnPlatform = false;

        public Vector2 position = new Vector2(100, 500);
        
        int speed = 20;
        int radius = 0;
        public int score = 0;

        public void setRadius(int radius)
        {
            this.radius = radius;
        }
        public int getRadius()
        {
            return this.radius;
        }

        public void collectCoins(List<Coins> coinList)
        {
            foreach (Coins coin in coinList.ToList()) 
            {
                // Check if the coin is active and close enough to the player
                if (coin.isActive && Vector2.Distance(position, coin.position) < coin.radius)
                {
                    Console.WriteLine($"Coin collected at {coin.position}"); 
                    coin.isActive = false; // Disable the coin
                    score += 10; // Increment the player's score
                }
            }
        }


        public void updatePlayer(List<WoodenPlank> planks, List<Coins> coinList)
        {
            // Check for coin collection
            collectCoins(coinList);

            // Get the current keyboard state
            KeyboardState state = Keyboard.GetState();

            // Move the ninja along the X-axis
            if (state.IsKeyDown(Keys.Left))
            {
                position.X -= speed;
            }
            if (state.IsKeyDown(Keys.Right))
            {
                position.X += speed;
            }

            // Jump logic
            if (state.IsKeyDown(Keys.Space) && !isJumping && isOnPlatform)
            {
                isJumping = true;
                currentJumpHeight = 0; // Reset jump height tracker
            }

            if (isJumping)
            {
                // Perform jump
                if (currentJumpHeight < jumpHeight)
                {
                    position.Y -= jumpSpeed;
                    currentJumpHeight += jumpSpeed;
                }
                else
                {
                    isJumping = false; // End jump if max height is reached
                }
            }
            else
            {
                // Apply gravity when not jumping
                position.Y += gravity;
                isOnPlatform = false;

                // Check collision with each plank
                foreach (var plank in planks)
                {
                    // Define the bounding rectangle for the plank (200 pixels long)
                    Rectangle plankBounds = new Rectangle((int)plank.position.X,
                (int)plank.position.Y, 250, 60);

                    // Check if the player is standing on the plank
                    if (position.Y + 50 >= plankBounds.Top && position.Y + 50 <= plankBounds.Bottom &&
                        position.X + radius > plankBounds.Left && position.X - radius < plankBounds.Right)
                    {
                        position.Y = plankBounds.Top - 50; // Snap player to the top of the plank
                        isOnPlatform = true;
                        break;
                    }
                }

                if (!isOnPlatform && position.Y > groundLevel)
                {
                    position.Y = groundLevel;
                    isOnPlatform = true;
                }
            }
        }
    }
}