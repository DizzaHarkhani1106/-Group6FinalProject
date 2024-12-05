using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

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
            _graphics.ApplyChanges(); // Apply the graphics settings

            base.Initialize();
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

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading texture: {ex.Message}");  
            }
        }

        protected override void Update(GameTime gameTime)
        {
            player.updatePlayer();
            // Exit the game when pressing the Back button or Escape
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
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
           
            if (woodenPlatformSprite != null)
            {
                _spriteBatch.Draw(woodenPlatformSprite, new Vector2(800,200), Color.White);
            }

            if (woodenPlatformSprite != null)
            {
                _spriteBatch.Draw(woodenPlatformSprite, new Vector2(400, 350), Color.White);
            }

            if (woodenPlatformSprite != null)
            {
                _spriteBatch.Draw(woodenPlatformSprite, new Vector2(1200, 350), Color.White);
            }

            if (playerSprite != null)
            {
                _spriteBatch.Draw(playerSprite, player.position - new Vector2(playerSprite.Width / 2, playerSprite.Height / 2), Color.White);
            }


            // Add additional drawing logic here

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
