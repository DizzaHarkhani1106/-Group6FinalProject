using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using static System.Formats.Asn1.AsnWriter;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Audio;
using System.Xml.Schema;

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
        private Texture2D ninjaWeapon;
        private Texture2D cactus;
        private SpriteFont scoreFont;
        private SpriteFont timeFont;
        private SpriteFont gameFont;
        
        Coins coins=new Coins();

        //1 to 10 , skipping even numbers

     
        List<Coins> goldcoins;
        List<Coins> silvercoins;
        List<Coins> bronzecoins;


        private SoundEffect winSound;
        private SoundEffect loseSound;
       
        private SoundEffect backgroundSound;


        // Coins logic
        private List<Coins> coinList; // List to hold multiple coins// 3 types of coins: Gold, Silver, Bronze

        //wooden plank logic
        private List<WoodenPlank> wood;
        private Vector2 ninjaWeaponPosition = new Vector2(1600, 100); // Adjusted initial position

        // character defining
        NinjaPlayer player = new NinjaPlayer();
        int score = 0;
        bool isGameOver = false;

        private string currentState = "MainMenu";

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

        private TimeSpan elapsedTime;
        private int secondsElapsed;

        public void Time()
        {
            elapsedTime = TimeSpan.Zero;
            secondsElapsed = 0;
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

        // Add coins with specific positions
        private void RandomCoins()
        {
            coinList.Add(new Coins { position = new Vector2(450, 300) });  // Coin above the second platform
            coinList.Add(new Coins { position = new Vector2(200, 200) });  // Random position
            coinList.Add(new Coins { position = new Vector2(700, 150) });  // Random position
            coinList.Add(new Coins { position = new Vector2(850, 200) });  // Coin above the first platform
            coinList.Add(new Coins { position = new Vector2(1100, 500) }); // Random position
            coinList.Add(new Coins { position = new Vector2(1400, 150) }); // Random position
            coinList.Add(new Coins { position = new Vector2(1250, 200) }); // Coin above the third platform

        }

        //Add wood logs in the screen
        private void RandomWoods()
        {

            wood.Add(new WoodenPlank(new Vector2(800, 200)));
            wood.Add(new WoodenPlank(new Vector2(400, 350)));
            wood.Add(new WoodenPlank(new Vector2(1200, 350)));
        }

          private List<Vector2> cactusPositions = new List<Vector2>()
         {
             new Vector2(800, 600),   // First cactus
             new Vector2(600, 650),  // Second cactus
             new Vector2(1500, 700),  // Third cactus
         };

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
                scoreFont = Content.Load<SpriteFont>("spaceFont");
                ninjaWeapon = Content.Load<Texture2D>("ninjaStar");
                cactus = Content.Load<Texture2D>("Cactus");

                winSound = Content.Load<SoundEffect>("win");
                loseSound = Content.Load<SoundEffect>("losingSound");

                timeFont = Content.Load<SpriteFont>("timerFont");
                gameFont = Content.Load<SpriteFont>("spaceFont");
                
                backgroundSound = Content.Load<SoundEffect>("backgroundAudio");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading texture: {ex.Message}");  
            }
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();

            if (currentState == "MainMenu")
            {
                if (state.IsKeyDown(Keys.Enter))
                {
                    StartGame();
                }
                else if (state.IsKeyDown(Keys.Escape))
                {
                    Exit();
                }
            }
            else if (currentState == "Playing")
            {   
                // Update timer
                updateTime(gameTime);

                // Update player logic
                player.updatePlayer(wood, coinList);

                // Handle coin collection
                for (int i = coinList.Count - 1; i >= 0; i--)
                {
                    Coins coin = coinList[i];
                    if (Vector2.Distance(player.position, coin.position) <= player.getRadius() / 3 + Coins.radius)
                    {
                        coinList.RemoveAt(i); // Remove the coin from the list
                       score += 1; // Increase the score
                    }
                }
               
               
                // Handle cactus collisions
                foreach (var cactusPosition in cactusPositions)
                {
                    Rectangle playerRect = new Rectangle((int)player.position.X - playerSprite.Width / 2,(int)player.position.Y - playerSprite.Height / 2,playerSprite.Width,playerSprite.Height);

                    Rectangle cactusRect = new Rectangle((int)cactusPosition.X - cactus.Width / 2,(int)cactusPosition.Y - cactus.Height / 2,cactus.Width,cactus.Height);

                    if (playerRect.Intersects(cactusRect))
                    {
                        currentState = "GameOver";
                        loseSound.Play();
                        break;
                    }
                }

                // Check for weapon collection
                Rectangle playerWeaponRect = new Rectangle((int)player.position.X - playerSprite.Width / 2,(int)player.position.Y - playerSprite.Height / 2,playerSprite.Width,playerSprite.Height);

                Rectangle weaponRect = new Rectangle( (int)ninjaWeaponPosition.X - ninjaWeapon.Width / 2,(int)ninjaWeaponPosition.Y - ninjaWeapon.Height / 2, ninjaWeapon.Width,ninjaWeapon.Height);

                if (playerWeaponRect.Intersects(weaponRect))
                {
                    score += 5;
                    ninjaWeaponPosition = new Vector2(-1000, -1000); // Move weapon off-screen
                }

                // Check if all coins are collected
                if (coinList.Count == 7)
                {
                    currentState = "GameOver";
                    winSound.Play();
                }

                // Exit game
                if (state.IsKeyDown(Keys.Escape))
                {
                    Exit();
                }
            }
            else if (currentState == "GameOver")
            {
                if (state.IsKeyDown(Keys.R))
                {
                    RestartGame();
                }
                else if (state.IsKeyDown(Keys.Escape))
                {
                    Exit();
                }
            }

            base.Update(gameTime);
        }



        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();

            if (currentState == "MainMenu")
            {
                // Draw the main menu
                _spriteBatch.DrawString(scoreFont, "Main Menu", new Vector2(700, 100), Color.White);
                _spriteBatch.DrawString(scoreFont, "Press Enter to Start", new Vector2(700, 200), Color.White);
                _spriteBatch.DrawString(scoreFont, "Press Escape to Exit", new Vector2(700, 300), Color.White);
            }
            else if (currentState == "Playing")
            {
                // Draw game objects
                _spriteBatch.DrawString(timeFont, "Time: " + secondsElapsed, new Vector2(0 , 0), Color.White);
                _spriteBatch.DrawString(timeFont, $"Score: {score}", new Vector2(10, 40), Color.White);

                _spriteBatch.Draw(underwaterSprite, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);
               

                foreach (WoodenPlank plank in wood)
                {
                    _spriteBatch.Draw(woodenPlatformSprite, plank.position, Color.White);
                }
                foreach (Coins coin in coinList)
                {
                    _spriteBatch.Draw(coinTexture, coin.position - new Vector2(Coins.radius, Coins.radius), Color.White);
                }
                foreach (var cactusPosition in cactusPositions)
                {
                    _spriteBatch.Draw(cactus, cactusPosition - new Vector2(cactus.Width / 2, cactus.Height / 2), Color.White);
                }

                _spriteBatch.Draw(ninjaWeapon, ninjaWeaponPosition - new Vector2(ninjaWeapon.Width / 2, ninjaWeapon.Height / 2), Color.White);
                _spriteBatch.Draw(playerSprite, player.position - new Vector2(playerSprite.Width / 2, playerSprite.Height / 2), Color.White);
            }
            else if (currentState == "GameOver")
            {
                // Draw the game over screen
                _spriteBatch.DrawString(scoreFont, "Game Over", new Vector2(700, 100), Color.White);
                _spriteBatch.DrawString(scoreFont, $"Final Score: {score}", new Vector2(700, 200), Color.White);
                _spriteBatch.DrawString(scoreFont, "Press R to Restart or Escape to Exit", new Vector2(700, 300), Color.White);
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }


        private void StartGame()
        {
            currentState = "Playing";
            score = 0; // Reset score
            player.position = new Vector2(100, 500); // Reset player position
            player.score = 0; // Reset player score
            RandomCoins(); // Reset coins
            RandomWoods(); // Reset platforms

            Time();
        }

        private void RestartGame()
        {
            StartGame(); // Same as starting the game
        }


    }

}
