using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

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
        ShadowBoss shadowBoss;
        Random rand = new Random();

        Texture2D blankSquare;
        SpriteFont debugFont;

        KeyboardState kb, oldkb;
        bool shouldMakeShadowPaths = false;
        bool shouldAddShadow = true;




        int gameClock = 1;

        List<ShadowPath> shadowPathList = new List<ShadowPath>();

        HealthBar bossHealthBar;
        HealthBar playerHealthBar;

        Enemy pathMaker;


        int screenWidth = 1080;
        int screenHeight = 720;

        int playerHeight = 100;
        int playerWidth = 60;

        int currentShadowPathNum = -1;


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

            player = new Player(ref blankSquare,
                new Rectangle(200, 200, playerWidth, playerHeight));

            playerHealthBar = new HealthBar(ref blankSquare,
                new Rectangle(30, 20, 200, 45), 5);

            pathMaker = new Enemy(ref blankSquare,
                new Rectangle(screenWidth - 10, screenHeight - playerWidth - 20, playerWidth, playerWidth));

            pathMaker.speed = 2;

            shadowBoss = new ShadowBoss(ref blankSquare,
                new Rectangle(600, 200, playerWidth * 4, playerHeight / 3));

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
            // player.Update(gameTime);
            base.Update(gameTime);
        }

        public void shadowBossLevel()
        {
            userControls();
            collideWithPlayer(30, shadowBoss.getRec());

            if (shadowBoss.shouldDoAcrossAttack)
            {
                for (int i = 0; i < 4; i++)
                {
                    shadowBoss.acrossScreenAttack();
                    collideWithPlayer(30, shadowBoss.getRec());


                }
            }
            //shadowBoss.rotateToUpper(gameClock);
            //shadowBoss.animate();
            makeShadowPaths();

            endOfTickCode();
        }

        public void collideWithPlayer(int damage, Rectangle objectCollided)
        {
            if (player.getRec().Intersects(objectCollided))
            {
                if (player.hitCooldown == 0)
                {
                    player.hitCooldown = 120;
                    player.health -= damage;
                }

            }


        }

        public void endOfTickCode()
        {

            playerHealthBar.setRecWidth(player.health * 2);
            if (player.hitCooldown > 0)
            {
                player.hitCooldown--;
            }
            gameClock++;
        }

        public void makeShadowPaths()
        {
            if (shouldMakeShadowPaths == false)
                return;

            for (int i = 0; i < pathMaker.speed; i++)
            {
                if (currentShadowPathNum != -1 &&
                        ((shadowPathList[currentShadowPathNum].getRecX() > 0 && currentShadowPathNum < 3) ||
                        shadowPathList[currentShadowPathNum].getRec().Bottom < screenHeight && currentShadowPathNum >= 3))
                {
                    shouldAddShadow = false;
                }
                else
                {
                    shouldAddShadow = true;
                    if (currentShadowPathNum != -1)
                    {
                        //pathMaker.incrementRecY(-1 * rand.Next(pathMaker.getRec().Height * 5 / 2, pathMaker.getRecY() - 30));
                        //pathMaker.setRecY(rand.Next((3 - currentShadowPathNum) * pathMaker.getRec().Height,
                        //    (4 - currentShadowPathNum) * pathMaker.getRec().Height));
                        if (currentShadowPathNum < 2)
                        {
                            pathMaker.incrementRecY(-rand.Next(3 * pathMaker.getRec().Height, 5 * pathMaker.getRec().Height));
                            pathMaker.setRecX(screenWidth - 20);
                        }
                        else
                        {
                            pathMaker.setRecY(-20);
                            pathMaker.setRecX(player.getRecX());

                        }
                        pathMaker.speed = 3;
                    }
                }

                if (shouldAddShadow)
                {
                    if (currentShadowPathNum < 5)
                    {
                        shadowPathList.Add(new ShadowPath(ref blankSquare,
                            new Rectangle(pathMaker.getRecX(), pathMaker.getRecY(), pathMaker.getRec().Width, pathMaker.getRec().Height)));

                        currentShadowPathNum += 1;
                    }
                    else
                    {
                        //clear shadow paths
                        shouldMakeShadowPaths = false;
                        pathMaker.setRecY(-playerWidth);
                        shadowPathList.Clear();
                    }
                }

                if (shouldMakeShadowPaths)
                {
                    if (currentShadowPathNum < 3)
                    {
                        shadowPathList[currentShadowPathNum].incrementRecX(-1);
                        pathMaker.incrementRecX(-1);
                        shadowPathList[currentShadowPathNum].incrementRecWidth(1);
                    }
                    else
                    {
                        //shadowPathList[currentShadowPathNum].incrementRecY(1);
                        shadowPathList[currentShadowPathNum].incrementRecHeight(1);
                        pathMaker.incrementRecY(1);


                    }

                }

            }

            //collideWithPlayer(30, pathMaker.getRec());
            if (gameClock % 15 == 0)
            {

                for (int i = 0; i < shadowPathList.Count; i++)
                {
                    if (i < 3)
                    {
                        shadowPathList[i].incrementRecHeight(2);
                        shadowPathList[i].incrementRecY(-1);
                    }
                    else
                    {
                        shadowPathList[i].incrementRecWidth(2);
                        shadowPathList[i].incrementRecX(-1);
                    }

                    collideWithPlayer(30, shadowPathList[i].getRec());


                }
            }

            if (gameClock % 5 == 0)
            {
                //pathMaker.speed = (int)System.Math.Pow(pathMaker.speed, 1.7);
                pathMaker.speed += 1;
            }


        }

        public void userControls()
        {
            //debug keybinds
            if (kb.IsKeyDown(Keys.L) && oldkb.IsKeyUp(Keys.L))
            {
                shadowBoss.shouldDoAcrossAttack = true;
            }

            if (kb.IsKeyDown(Keys.R))
            {
                shadowBoss.rotateToUpper(gameClock);
            }

            if (kb.IsKeyDown(Keys.P) && oldkb.IsKeyUp(Keys.P))
            {
                shouldMakeShadowPaths = !shouldMakeShadowPaths;
            }



            if (kb.IsKeyDown(Keys.A) || kb.IsKeyDown(Keys.Left))
            {
                for (int i = 0; i < player.speed; i++)
                {
                    if (player.getRecX() - 1 >= 0)
                        player.incrementRecX(-1);
                }
            }

            if (kb.IsKeyDown(Keys.D) || kb.IsKeyDown(Keys.Right))
            {
                for (int i = 0; i < player.speed; i++)
                {
                    if (player.getRec().Right + 1 <= screenWidth)
                        player.incrementRecX(1);

                }
            }

            if (kb.IsKeyDown(Keys.W) || kb.IsKeyDown(Keys.Up))
            {
                for (int i = 0; i < player.speed; i++)
                {
                    if (player.getRecY() - 1 >= 0)
                        player.incrementRecY(-1);

                }
            }

            if (kb.IsKeyDown(Keys.S) || kb.IsKeyDown(Keys.Down))
            {
                for (int i = 0; i < player.speed; i++)
                {
                    if (player.getRec().Bottom + 1 <= screenHeight)
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



            //shadowBoss.drawCharacter(spriteBatch, Color.Black);
            shadowBoss.drawCharacter(spriteBatch, Color.Black, true);

            if (player.hitCooldown > 0)
            {
                if (gameClock % 8 != 0)
                {
                    player.drawCharacter(spriteBatch, Color.Red);

                }
            }
            else
            {
                player.drawCharacter(spriteBatch, Color.Red);

            }


            pathMaker.drawCharacter(spriteBatch, Color.Purple);

            for (int i = 0; i < shadowPathList.Count; i++)
            {
                shadowPathList[i].drawCharacter(spriteBatch, Color.Black);
            }




            playerHealthBar.drawCharacter(spriteBatch, new Color(220, 60, 30), Color.Black);
            spriteBatch.DrawString(debugFont, "Hitcooldown: " + player.hitCooldown, new Vector2(100, screenHeight - 100), Color.Green);
            spriteBatch.DrawString(debugFont, "Health: " + player.health, new Vector2(100, screenHeight - 80), Color.Green);
            spriteBatch.DrawString(debugFont, "currentShadowPath: " + currentShadowPathNum, new Vector2(100, screenHeight - 60), Color.Green);



            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
