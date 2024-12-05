using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

namespace SpaceProject
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Texture for the background image
        private Texture2D underwaterSprite;
        private Texture2D playerSprite;
        private Texture2D woodenPlatformSprite;
        private Texture2D coinTexture; // Texture for the coin
        private Texture2D cactusTexture;
        private Texture2D ninjaWeaponTexture;


        bool start = false;

        // Coins logic
        private List<Coins> coinList; // List to hold multiple coins

        //wooden plank logic
        private List<WoodenPlank> wood;
        private Vector2 ninjaWeaponPosition = new Vector2(550, 100); // Adjusted initial position
                                                                    
        private List<Vector2> cactusPositions = new List<Vector2>()
        {
            new Vector2(800, 600),   // First cactus
            new Vector2(1200, 650),  // Second cactus
            new Vector2(1500, 700),  // Third cactus
        };


        // character defining
        NinjaPlayer player = new NinjaPlayer();
        // Game variables
        private int score = 0;
        private bool isGameOver = false;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1700, // Set screen width
                PreferredBackBufferHeight = 900 // Set screen height
            };
            Content.RootDirectory = "Content";
            IsMouseVisible = true; 
        }

        protected override void Initialize()
        {
            //coin Logic
            coinList = new List<Coins>();
            RandomCoins();

            //wood Logic
            wood = new List<WoodenPlank>();
            RandomWoods();

            _graphics.ApplyChanges(); // Apply the graphics settings
            base.Initialize();
        }

        // Add coins with specific positions
        private void RandomCoins()
        {
            coinList.Add(new Coins { position = new Vector2(850, 150) });  // Coin above the first platform
            coinList.Add(new Coins { position = new Vector2(450, 300) });  // Coin above the second platform
            coinList.Add(new Coins { position = new Vector2(1250, 300) }); // Coin above the third platform
            coinList.Add(new Coins { position = new Vector2(600, 450) });  // Random position
            coinList.Add(new Coins { position = new Vector2(1400, 400) }); // Random position
            coinList.Add(new Coins { position = new Vector2(700, 250) });  // Random position
            coinList.Add(new Coins { position = new Vector2(1100, 500) }); // Random position
        }

        //Add wood logs in the screen
        private void RandomWoods()
        {

            wood.Add(new WoodenPlank(new Vector2(800, 200)));
            wood.Add(new WoodenPlank(new Vector2(400, 350)));
            wood.Add(new WoodenPlank(new Vector2(1200, 350)));
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            try
            {
                // Load the background texture
                underwaterSprite = Content.Load<Texture2D>("forest");
                playerSprite = Content.Load<Texture2D>("ninja");
                woodenPlatformSprite = Content.Load<Texture2D>("wood");
                coinTexture = Content.Load<Texture2D>("reward");
                cactusTexture = Content.Load<Texture2D>("Cactus");
                ninjaWeaponTexture = Content.Load<Texture2D>("ninjaStar");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading texture: {ex.Message}");  
            }
        }
        protected override void Update(GameTime gameTime)
        {
            if (isGameOver) return;
            player.updatePlayer(wood);
            // Check for collision with cactus
            foreach (var cactusPosition in cactusPositions)
            {
                Rectangle playerRect = new Rectangle((int)player.position.X - playerSprite.Width / 2, (int)player.position.Y - playerSprite.Height / 2, playerSprite.Width, playerSprite.Height);
                Rectangle cactusRect = new Rectangle((int)cactusPosition.X - cactusTexture.Width / 2, (int)cactusPosition.Y - cactusTexture.Height / 2, cactusTexture.Width, cactusTexture.Height);

                if (playerRect.Intersects(cactusRect))
                {
                    // Game Over when player collides with cactus
                    isGameOver = true;
                    score -= 5;  // Decrease score by 5
                    Console.WriteLine("Game Over! You lose. Score: " + score); 
                }
            }
            // Check for collision with ninja weapon
            Rectangle playerWeaponRect = new Rectangle((int)player.position.X - playerSprite.Width / 2, (int)player.position.Y - playerSprite.Height / 2, playerSprite.Width, playerSprite.Height);
            Rectangle weaponRect = new Rectangle((int)ninjaWeaponPosition.X - ninjaWeaponTexture.Width / 2, (int)ninjaWeaponPosition.Y - ninjaWeaponTexture.Height / 2, ninjaWeaponTexture.Width, ninjaWeaponTexture.Height);

            if (playerWeaponRect.Intersects(weaponRect))
            {
                // Add points when player collects ninja weapon
                score += 10;
                ninjaWeaponPosition = new Vector2(-1000, -1000); // Move the weapon off screen after collection
            }
            // Exit the game when pressing Escape
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            // Add your game logic updates here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Clear the screen with a background color
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

                if (start == true)
                {

                        
                }

            // Draw the background texture
            if (underwaterSprite != null)
            {
                _spriteBatch.Draw( underwaterSprite, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight),Color.White);
            }
           
            // Draw wood plank 
            if(woodenPlatformSprite != null)
            {
                foreach(WoodenPlank wood in wood)
                {
                    _spriteBatch.Draw(woodenPlatformSprite, wood.position, Color.White);
                }
            }
            if (playerSprite != null)
            {
                _spriteBatch.Draw(playerSprite, player.position - new Vector2(playerSprite.Width / 2, playerSprite.Height / 2), Color.White);
            }

            // Draw coins
            if (coinTexture != null)
            {
                foreach (Coins coin in coinList)
                {
                    _spriteBatch.Draw(coinTexture, coin.position - new Vector2(Coins.radius, Coins.radius), Color.White);
                }
            }
            foreach (var cactusPosition in cactusPositions)
            {
                // Center the cactus by adjusting its position using the texture width/height
                Vector2 adjustedPosition = cactusPosition - new Vector2(cactusTexture.Width / 2, cactusTexture.Height / 2);

                // Draw the cactus at the adjusted position
                _spriteBatch.Draw(cactusTexture, adjustedPosition, Color.White);
            }
          
            // Draw ninja weapon
            if (ninjaWeaponTexture != null)
            {
                _spriteBatch.Draw(ninjaWeaponTexture, ninjaWeaponPosition - new Vector2(ninjaWeaponTexture.Width / 2, ninjaWeaponTexture.Height / 2), Color.White);
            }
            // Display "Game Over" message if the game is over
            if (isGameOver)
            {
                SpriteFont font = Content.Load<SpriteFont>("GameOverFont"); // Assuming you have a sprite font named "GameOverFont"
                _spriteBatch.DrawString(font, "Game Over!", new Vector2(600, 400), Color.Red);
                _spriteBatch.DrawString(font, $"Score: {score}", new Vector2(600, 450), Color.White);
            }
            // Add additional drawing logic here

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
