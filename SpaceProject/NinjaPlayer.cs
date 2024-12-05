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
        private int jumpHeight = 200; 
        private int jumpSpeed = 10; 
        private int gravity = 8; 
        private int groundLevel = 500; 
        private int currentJumpHeight = 0; 

        public Vector2 position = new Vector2(100, 500); // ( x-axis, y-axis)

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
        

        public void updatePlayer()
        {
            // Get the current keyboard state
            KeyboardState state = Keyboard.GetState();

            // Move the ninja along the X-axis
            if (state.IsKeyDown(Keys.Left))
            {
                this.position.X -= speed;
            }
            if (state.IsKeyDown(Keys.Right))
            {
                this.position.X += speed;
            }
            

            // Jump logic
            if (state.IsKeyDown(Keys.Space) && !isJumping)
            {
                // Start the jump
                isJumping = true;
                currentJumpHeight = 0; // Reset jump height tracker
            }

            if (isJumping)
            {
                // Jump logic
                if (currentJumpHeight < jumpHeight)
                {
                    this.position.Y -= jumpSpeed; 
                    currentJumpHeight += jumpSpeed; 
                }
                else
                {
                    // gravity effects
                    this.position.Y += gravity; 

                    // Stop jumping when reach the ground
                    if (this.position.Y >= groundLevel)
                    {
                        this.position.Y = groundLevel; 
                        isJumping = false; 
                    }
                }

                if (state.IsKeyDown(Keys.Space) && state.IsKeyDown(Keys.Enter))
                {
                    this.position.Y -= jumpSpeed*2;
                    currentJumpHeight += jumpSpeed; // Increases jump height on pressing Enter
                }
            }
        }
       


       

        
        }
    }

