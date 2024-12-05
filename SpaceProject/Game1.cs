using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

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

        // Coins logic
        private List<Coins> coinList; // List to hold multiple coins

        //wooden plank logic
        private List<WoodenPlank> wood;

        // character defining
        NinjaPlayer player = new NinjaPlayer();

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

            _graphics.ApplyChanges(); 
            base.Initialize();
        }

        // Add coins with specific positions
        private void RandomCoins()
        {
            coinList.Add(new Coins { position = new Vector2(850, 150) });  
            coinList.Add(new Coins { position = new Vector2(450, 300) });  
            coinList.Add(new Coins { position = new Vector2(1250, 300) }); 
            coinList.Add(new Coins { position = new Vector2(600, 450) });  
            coinList.Add(new Coins { position = new Vector2(1400, 400) }); 
            coinList.Add(new Coins { position = new Vector2(700, 250) });  
            coinList.Add(new Coins { position = new Vector2(1100, 500) }); 
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

                // Load font for score display
                Content.Load<SpriteFont>("ScoreFont");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading texture: {ex.Message}");  
            }
        }

        protected override void Update(GameTime gameTime)
        {
            player.updatePlayer(wood, coinList);
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

            // Draw the background texture
            if (underwaterSprite != null)
            {
                _spriteBatch.Draw( underwaterSprite, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight),Color.White);
            }
            if (playerSprite != null)
            {
                _spriteBatch.Draw(playerSprite, player.position - new Vector2(playerSprite.Width / 2, playerSprite.Height / 2), Color.White);
            }
            // Draw wood plank 
            if(woodenPlatformSprite != null)
            {
                foreach(WoodenPlank wood in wood)
                {
                    _spriteBatch.Draw(woodenPlatformSprite, wood.position, Color.White);
                }
            }
            // Modify the Draw method as follows:
            if (coinTexture != null)
            {
                foreach (Coins coin in coinList)
                {
                    if (coin.isActive) // Only draw active coins
                    {
                        _spriteBatch.Draw(coinTexture, coin.position, Color.White);
                    }
                }
            }


            // Add additional drawing logic here

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
