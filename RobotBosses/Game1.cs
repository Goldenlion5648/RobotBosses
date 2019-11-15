using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RobotBosses
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Player player;
        Enemy shadowBoss;

        Texture2D blankSquare;
        SpriteFont debugFont;

        KeyboardState kb, oldkb;


        int screenWidth = 1080;
        int screenHeight = 720;

        int playerHeight = 100;
        int playerWidth = 60;

        int playerSpeed = 5;


        enum gameState
        {
            levelSelect, shadowBoss
        }

        gameState state = gameState.shadowBoss;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            this.graphics.PreferredBackBufferWidth = screenWidth;
            this.graphics.PreferredBackBufferHeight = screenHeight;

            this.IsMouseVisible = true;

            

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            debugFont = Content.Load<SpriteFont>("debugFont");

            blankSquare = Content.Load<Texture2D>("blankSquare");

            player = new Player(blankSquare,
                new Rectangle(200, 200, playerWidth, playerHeight));

            shadowBoss = new Enemy(blankSquare,
                new Rectangle(600, 200, playerWidth, playerHeight * 5 / 2));

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            kb = Keyboard.GetState();
            if (kb.IsKeyDown(Keys.LeftShift) && kb.IsKeyDown(Keys.F5))
                Exit();
            switch (state)
            {
                case gameState.levelSelect:

                    break;
                case gameState.shadowBoss:
                    shadowBossLevel();
                    break;


            }



            // TODO: Add your update logic here

            oldkb = kb;
            player.Update(gameTime);
            base.Update(gameTime);
        }

        public void shadowBossLevel()
        {
            userControls();
        }

        public void userControls()
        {
            if(kb.IsKeyDown(Keys.A) || kb.IsKeyDown(Keys.Left))
            {
                for (int i = 0; i < playerSpeed; i++)
                {
                    player.incrementRecX(-1);
                }
            }

            if (kb.IsKeyDown(Keys.D) || kb.IsKeyDown(Keys.Right))
            {
                for (int i = 0; i < playerSpeed; i++)
                {
                    player.incrementRecX(1);
                }
            }

            if (kb.IsKeyDown(Keys.W) || kb.IsKeyDown(Keys.Up))
            {
                for (int i = 0; i < playerSpeed; i++)
                {
                    player.incrementRecY(-1);
                }
            }

            if (kb.IsKeyDown(Keys.S) || kb.IsKeyDown(Keys.Down))
            {
                for (int i = 0; i < playerSpeed; i++)
                {
                    player.incrementRecY(1);
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();



            shadowBoss.drawCharacter(spriteBatch, Color.Black);
            player.drawCharacter(spriteBatch, Color.Red);





            spriteBatch.DrawString(debugFont, "Hitcooldown: " + player.hitCooldown, new Vector2(100, screenHeight - 100), Color.Green);
            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
