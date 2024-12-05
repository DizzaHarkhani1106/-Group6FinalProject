﻿using System;
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

        public Vector2 position = new Vector2(100, 500); // ( x-axis, y-axis)
        public Vector2 velocity = Vector2.Zero;         // Velocity for gravity and jumping

        int speed = 20;
        int radius = 0;

        public void setRadius(int radius)
        {
            this.radius = radius;
        }
        public int getRadius()
        {
            return this.radius;
        }


        public void updatePlayer(List<WoodenPlank> planks)
        {
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
                    // Define the bounding rectangle for the plank
                    Rectangle plankBounds = new Rectangle(
                        (int)plank.position.X, // X-coordinate
                        (int)plank.position.Y, // Y-coordinate
                        100,                   // Width of the plank (adjust as needed)
                        20                     // Height of the plank (adjust as needed)
                    );

                    // Check collision with the plank
                    if (position.Y + 20 >= plankBounds.Top && position.Y <= plankBounds.Top && // Close to the top of the plank
                        position.X + radius > plankBounds.Left && position.X - radius < plankBounds.Right) // Within plank width
                    {
                        position.Y = plankBounds.Top - 50; // Snap player to the plank
                        isOnPlatform = true;
                        break;
                    }
                }


                // Stop at ground level if no platform below
                if (!isOnPlatform && position.Y > groundLevel)
                {
                    position.Y = groundLevel;
                    isOnPlatform = true;
                }
            }
        }

    }
}