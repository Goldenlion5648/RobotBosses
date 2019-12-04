using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

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
        Texture2D potionBottle;
        Texture2D ringTexture;
        Texture2D playerTexture;
        Texture2D bandana;
        
        SpriteFont debugFont, titleFont;
        
        KeyboardState kb, oldkb;
        //SoundEffect backgroundMusic;
        Song backgroundMusic;

        MouseState ms;
        Point mousePos;

        //bool shouldMakeShadowPaths = false;
        bool shouldAddShadow = true;

        bool debugMode = false;

        int gameClock = 1;

        List<ShadowPath> shadowPathList = new List<ShadowPath>();

        HealthBar bossHealthBar;
        HealthBar playerHealthBar;

        List<Collectable> collectables = new List<Collectable>();
        //Collectable collectableItem;

        Ring guardRing;

        Enemy pathMaker;

        bool colorCountingUp = true;
        int colorNum = 30;

        int fontClock = 0;
        bool shouldShowText = true;


        public static int screenWidth = 1080;
        public static int screenHeight = 720;

        public static int lightCooldown = 0;

        int playerHeight = 60;
        int playerWidth = 40;

        int currentShadowPathNum = -1;
        int shadowPathStartTime = 0;

        int maxTimeCollectableOnScreen = 600;


        enum gameState
        {
           shadowBoss, titleScreen, lose, win
        }

        gameState state = gameState.titleScreen;


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
            titleFont = Content.Load<SpriteFont>("titleFont");

            blankSquare = Content.Load<Texture2D>("blankSquare");
            potionBottle = Content.Load<Texture2D>("potionBottle");
            ringTexture = Content.Load<Texture2D>("greenLightBall");
            playerTexture = Content.Load<Texture2D>("snakePlayer2");
            bandana = Content.Load<Texture2D>("bandana2");

            backgroundMusic = Content.Load<Song>("snakeBoss3");

            player = new Player(ref playerTexture,
                new Rectangle(200, 200, playerWidth, playerHeight));

            //collectableItem = new Collectable(ref potionBottle,
            //    new Rectangle(200, 200, playerWidth, playerWidth), ref player);

            

            guardRing = new Ring(ref ringTexture,
                new Rectangle(-1200, 200, playerWidth, playerWidth), ref player, 2);

            playerHealthBar = new HealthBar(ref blankSquare, new Rectangle(30, screenHeight - 60, 200, 35), 5);
            bossHealthBar = new HealthBar(ref blankSquare, new Rectangle(screenWidth / 2 + 30, screenHeight - 60, 400, 35), 5);

            pathMaker = new Enemy(ref blankSquare,
                new Rectangle(screenWidth - 10, screenHeight - playerWidth - 20, playerWidth, playerWidth));

            pathMaker.speed = 2;

            shadowBoss = new ShadowBoss(ref blankSquare,
                new Point(400, screenHeight / 2), ref player, ref guardRing);

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
            ms = Mouse.GetState();
            mousePos = new Point(ms.X, ms.Y);
            if (kb.IsKeyDown(Keys.LeftShift) && kb.IsKeyDown(Keys.F5))
                Exit();
            switch (state)
            {
                case gameState.shadowBoss:
                    shadowBossLevel();
                    break;

                case gameState.titleScreen:
                    titleScreen();
                    break;
                case gameState.win:
                    win();
                    break;
                case gameState.lose:
                    lose();
                    break;

            }

            // TODO: Add your update logic here

            oldkb = kb;
            // player.Update(gameTime);
            base.Update(gameTime);
        }

        public void titleScreen()
        {
            if (kb.IsKeyDown(Keys.Enter) && oldkb.IsKeyUp(Keys.Enter))
            {
                state = gameState.shadowBoss;
            }
            fontClock++;
        }

        public void win()
        {
            if(kb.IsKeyDown(Keys.Enter))
            {
                reset();
                state = gameState.shadowBoss;
            }

            if (kb.IsKeyDown(Keys.Escape))
            {
                reset();
                state = gameState.titleScreen;
            }
        }

        public void lose()
        {
            if (kb.IsKeyDown(Keys.Enter))
            {
                reset();
                state = gameState.shadowBoss;
            }

            if (kb.IsKeyDown(Keys.Escape))
            {
                reset();
                state = gameState.titleScreen;
            }
        }

        public void reset()
        {
            gameClock = 1;
            currentShadowPathNum = -1;
            shadowPathStartTime = 0;
            lightCooldown = 0;

            //player.health = player.startingHealth;
            //shadowBoss.health = shadowBoss.startingHealth;
            shadowBoss = null;
            player = null;
            player = new Player(ref playerTexture,
                new Rectangle(200, 200, playerWidth, playerHeight));
            shadowBoss = new ShadowBoss(ref blankSquare,
                new Point(400, screenHeight / 2), ref player, ref guardRing);

            guardRing = new Ring(ref ringTexture,
                new Rectangle(200, 200, playerWidth, playerWidth), ref player, 2);


            shouldAddShadow = true;

            colorCountingUp = true;
            colorNum = 30;

            currentShadowPathNum = -1;
            shadowPathStartTime = 0;

            pathMaker.resetPos();
            shadowPathList.Clear();



            collectables.Clear();
        }

        public void shadowBossLevel()
        {
            userControls();

            makeShadowPaths();

            if (shadowBoss.currentPhase == ShadowBoss.phase.flail)
            {
                shadowBoss.verticalSweepAttack();
            }

            if (shadowBoss.currentPhase == ShadowBoss.phase.patrol)
            {

                shadowBoss.patrol();
                //shadowBoss.shouldMoveToPoint = true;
                //for (int i = 0; i < shadowBoss.speed; i++)
                //{
                //    shadowBoss.hasMovedInTick = false;

                //    shadowBoss.moveToPoint(new Point(0, screenHeight - shadowBoss.getPartRec(0).Height));
                //    //shadowBoss.moveToPoint(new Point(player.getRecX(), player.getRecY()));

                //    for (int j = 0; j < shadowBoss.numParts; j++)
                //    {
                //        collideWithPlayer(30, shadowBoss.getPartRec(j));
                //    }
                //    //collideWithPlayer(30, shadowBoss.getRec());
                //    //}
                //}
            }

            endOfTickCode();
        }

        public void collideWithPlayer(int damage, Rectangle objectCollided)
        {
            if (player.getRec().Intersects(objectCollided))
            {
                if (player.hitCooldown == 0)
                {
                    player.hitCooldown = 180;
                    player.health -= damage;
                }

            }


        }

        public void collectableCode()
        {
            if (gameClock != 0 && gameClock % 360 == 0)
            {
                collectables.Add(new Collectable(ref potionBottle, new Rectangle(rand.Next(0, screenWidth - playerWidth),
                    rand.Next(0, screenHeight - playerWidth), playerWidth, playerWidth), ref player));
                collectables[collectables.Count - 1].typeOfCollectable =
                    (Collectable.collectableType)rand.Next(0, (int)(Collectable.collectableType.lights + 1));
                //collectableItem.timeExisted = 0;
                //collectableItem.setRecX(rand.Next(0, screenWidth - collectableItem.getRec().Width));
                //collectableItem.setRecY(rand.Next(0, screenHeight - collectableItem.getRec().Height));
                //player.currentWeapon = Player.weapon.fist;
            }


            for (int i = 0; i < collectables.Count; i++)
            {
                if (collectables[i].onCollect())
                {
                    collectables.RemoveAt(i);
                    continue;
                }

                collectables[i].timeExisted++;
                if(collectables[i].timeExisted >= maxTimeCollectableOnScreen)
                {
                    collectables.RemoveAt(i);
                }

            }

        }

        public void endOfTickCode()
        {
            if (MediaPlayer.State == MediaState.Stopped)
                MediaPlayer.Play(backgroundMusic);

            collectableCode();

            if (player.hitCooldown > 0)
            {
                player.hitCooldown--;
            }
            if (shadowBoss.phaseCooldown > 0)
                shadowBoss.phaseCooldown--;

            if(shadowBoss.phaseCooldown == 0)
            {
                changeToNewPhase();
            }

            if(player.health < player.startingHealth && player.health > 0)
            {
                if(gameClock % 300 == 0)
                {
                    player.health++;
                }
            }
            if (shadowBoss.health < shadowBoss.startingHealth && shadowBoss.health > 0 && lightCooldown == 0)
            {
                if (gameClock % 210 == 0)
                {
                    shadowBoss.health++;
                }
            }



            if (player.currentWeapon == Player.weapon.ring)
            {
                guardRing.move(gameClock);
                shadowBoss.takeDamage(guardRing);
                if (player.weaponCooldown > 0)
                    player.weaponCooldown--;
                if (player.weaponCooldown == 0)
                    player.currentWeapon = Player.weapon.fist;
            }
            else
            {
                guardRing.setRecX(-1000);
            }

            if (lightCooldown > 0)
            {
                lightCooldown--;
                if (gameClock % 60 == 0)
                {
                    shadowBoss.health -= 4;
                    for (int i = 0; i < shadowBoss.numParts; i++)
                    {
                        shadowBoss.getPart(i).hitCooldown += 8;
                    }
                }
            }

            for (int i = 0; i < shadowBoss.numParts; i++)
            {
                if (shadowBoss.getPart(i).hitCooldown > 0)
                {
                    shadowBoss.getPart(i).hitCooldown--;
                }
            }

            if (player.speedCooldown > 0)
                player.speedCooldown--;

            if (player.speedCooldown == 0)
                player.speed = player.startingSpeed;

            shadowBoss.inflictDamageToPlayer();
            if (player.health < 7)
            {
                playerHealthBar.setRecWidth(player.health);
            }
            else
            {
                playerHealthBar.setRecWidth((int)((double)((double)player.health / (double)player.startingHealth)
                    * (playerHealthBar.getBackground().Width) - playerHealthBar.border * 2));
            }

            if (shadowBoss.health < 10)
            {
                bossHealthBar.setRecWidth(shadowBoss.health);
            }
            else
            {
                bossHealthBar.setRecWidth((int)((double)((double)shadowBoss.health / (double)shadowBoss.startingHealth)
                    * (bossHealthBar.getBackground().Width) - bossHealthBar.border * 2));
            }

            if(shadowBoss.health <= 0)
            {
                state = gameState.win;
            }

            if (player.health <= 0)
                state = gameState.lose;

            //playerHealthBar.setRecWidth(player.health * 2);
            gameClock++;
        }

        public void makeShadowPaths()
        {
            if (shadowBoss.currentPhase != ShadowBoss.phase.shadowPaths)
                return;

            if(shadowPathStartTime == 0)
            {
                shadowPathStartTime = gameClock;
                shadowBoss.phaseCooldown = 900;
                shadowBoss.straightenInPlace();
            }

            if (shadowBoss.phaseCooldown > 780)
            {

                for (int i = 0; i < shadowBoss.speed; i++)
                {
                    shadowBoss.hasMovedInTick = false;

                    //shadowBoss.moveToPoint(new Point(screenWidth + 10, shadowBoss.startingPos.Y));
                    shadowBoss.moveToPoint(new Point(screenWidth + 10, playerHeight));

                    for (int j = 0; j < shadowBoss.numParts; j++)
                    {
                        collideWithPlayer(30, shadowBoss.getPartRec(j));
                    }
                    //collideWithPlayer(30, shadowBoss.getRec());
                    //}
                }


                return;
            }

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
                            pathMaker.setRecY(player.getRec().Y);
                            //pathMaker.incrementRecY(-rand.Next(5, 8) * pathMaker.getRec().Height);
                            pathMaker.setRecX(screenWidth - 20);
                        }
                        else
                        {
                            pathMaker.setRecY(-20);
                            if (currentShadowPathNum > 5)
                            {
                                if (currentShadowPathNum % 2 == 0)
                                {
                                    pathMaker.setRecX(player.getRecX());
                                }
                                else
                                {
                                    pathMaker.setRecX(player.getRecX() + 50);

                                }
                            }
                            else
                            {
                                pathMaker.setRecX(player.getRec().Center.X + rand.Next(-20, 10));
                            }
                        }
                        pathMaker.speed = 3;
                    }
                }

                if (shouldAddShadow)
                {
                    //shadowPathStartTime = gameClock;
                    //if (gameClock - shadowPathStartTime > 730)
                    if (shadowBoss.phaseCooldown == 1)
                    {
                        //clear shadow paths

                        //shouldMakeShadowPaths = false;
                        //pathMaker.resetPos();
                        //shadowPathList.Clear();
                        //currentShadowPathNum = -1;
                        //shouldAddShadow = false;
                        //shadowPathStartTime = 0;
                        //changeToNewPhase();
                    }
                    else
                    {
                        shadowPathList.Add(new ShadowPath(ref blankSquare,
                            new Rectangle(pathMaker.getRecX(), pathMaker.getRecY(), pathMaker.getRec().Width, pathMaker.getRec().Height)));

                        currentShadowPathNum += 1;
                    }

                    if (currentShadowPathNum < 6)
                    {
                        //shadowPathList.Add(new ShadowPath(ref blankSquare,
                        //    new Rectangle(pathMaker.getRecX(), pathMaker.getRecY(), pathMaker.getRec().Width, pathMaker.getRec().Height)));

                        //currentShadowPathNum += 1;
                    }
                    else
                    {
                        

                    }
                }

                //if (shadowBoss.currentPhase == ShadowBoss.phase.shadowPaths)
                //{
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

                //}

            }

            //collideWithPlayer(30, pathMaker.getRec());
            if (gameClock % 20 == 0)
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

                    //collideWithPlayer(30, shadowPathList[i].getRec());


                }
            }

            for (int i = 0; i < shadowPathList.Count; i++)
            {
                collideWithPlayer(30, shadowPathList[i].getRec());
            }

            if (gameClock % 5 == 0)
            {
                //pathMaker.speed = (int)System.Math.Pow(pathMaker.speed, 1.7);
                pathMaker.speed += 1;
            }


        }

        public void speedUpSweep()
        {
            if(shadowBoss.getPartRec(0).Y < -2000)
            {
                shadowBoss.turnUp();
            }
        }

        public void changeToNewPhase()
        {
            //var  last = Enum.GetValues(typeof(ShadowBoss.phase)).Cast<int>().Max();

            if(shadowBoss.currentPhase == ShadowBoss.phase.shadowPaths)
            {
                pathMaker.resetPos();
                shadowPathList.Clear();
                currentShadowPathNum = -1;
                shouldAddShadow = false;
                shadowPathStartTime = 0;
            }

            int originalPhase = (int)shadowBoss.currentPhase;
            //while ((int)shadowBoss.currentPhase == originalPhase)
            //{
            //    //shadowBoss.currentPhase = (ShadowBoss.phase)rand.Next(1, (int)ShadowBoss.phase.debug);
            //    shadowBoss.currentPhase = (ShadowBoss.phase)rand.Next(0, (int)ShadowBoss.phase.debug);

            //}

            shadowBoss.currentPhase = (ShadowBoss.phase)((int)shadowBoss.currentPhase + 1);
            if ((int)shadowBoss.currentPhase >= (int)ShadowBoss.phase.debug)
                shadowBoss.currentPhase = (ShadowBoss.phase)0;


            //shadowBoss.currentPhase = (ShadowBoss.phase)5;


            shadowBoss.colorSwitchCount = 10;
        }

        public void userControls()
        {
            //debug keybinds
            //if (kb.IsKeyDown(Keys.D2) && oldkb.IsKeyUp(Keys.D2))
            //{
            //    shadowBoss.shouldDoAcrossAttack = !shadowBoss.shouldDoAcrossAttack;

            //}
            if (debugMode)
            {

                if (kb.IsKeyDown(Keys.I))
                {
                    shadowBoss.turnUp();
                }

                if (kb.IsKeyDown(Keys.J))
                {
                    shadowBoss.moveLeft(3);
                }

                if (kb.IsKeyDown(Keys.D9))
                {
                    shadowBoss.straightenInPlace();
                }

                if (kb.IsKeyDown(Keys.L))
                {
                    shadowBoss.moveRight(3);
                }

                if (kb.IsKeyDown(Keys.H))
                {
                    shadowBoss.currentPhase = ShadowBoss.phase.debug;
                    //if (gameClock % 10 == 0)
                    //{
                    for (int i = 0; i < shadowBoss.speed; i++)
                    {
                        shadowBoss.hasMovedInTick = false;

                            shadowBoss.moveToPoint(mousePos);
                            for (int j = 0; j < shadowBoss.numParts; j++)
                            {
                                collideWithPlayer(30, shadowBoss.getPartRec(j));
                            }
                            //collideWithPlayer(30, shadowBoss.getRec());
                        //}
                    }
                }

                if (kb.IsKeyDown(Keys.K))
                {
                    //if (gameClock % 2 == 0)
                    shadowBoss.turnDown();
                }

                if (kb.IsKeyDown(Keys.O))
                {
                    shadowBoss.resetPosition();
                }

                if (kb.IsKeyDown(Keys.R))
                {
                    //shadowBoss.rotateToUpper(gameClock);
                }
                if (kb.IsKeyDown(Keys.D4))
                {
                    //shadowBoss.clumpUpFromHorizontal();
                    shadowBoss.transitionToDownwardVerticalFromLeft();
                }

                if (kb.IsKeyDown(Keys.D5))
                {
                    //shadowBoss.clumpUpFromHorizontal();
                    shadowBoss.transitionToDownwardVerticalFromRight();
                }

                if (kb.IsKeyDown(Keys.Y))
                {
                    //shadowBoss.clumpUpFromVertical();
                    shadowBoss.transitionToLeftHorizontalFromDown();
                }

                if (kb.IsKeyDown(Keys.D6))
                {
                    //shadowBoss.clumpUpFromVertical();
                    shadowBoss.transitionToLeftHorizontalFromUp();
                }

                if (kb.IsKeyDown(Keys.D7))
                {
                    //shadowBoss.clumpUpFromVertical();
                    shadowBoss.transitionToUpwardVertical();
                }

                if (kb.IsKeyDown(Keys.D8))
                {
                    //shadowBoss.clumpUpFromVertical();
                    shadowBoss.transitionToRightHorizontalFromUp();
                }

                if (kb.IsKeyDown(Keys.M) && oldkb.IsKeyUp(Keys.M))
                {
                    //shadowBoss.clumpUpFromVertical();
                    shadowBoss.shouldMoveToPoint = !shadowBoss.shouldMoveToPoint;
                    //shadowBoss.movementDestination = new Point(player.getRecX(), player.getRecY());
                }

                if (kb.IsKeyDown(Keys.D1) && oldkb.IsKeyUp(Keys.D1))
                {
                    //shadowBoss.clumpUpFromVertical();
                    //shadowBoss.straightenInPlace();
                    //shadowBoss.shouldDoSweepAttack = !shadowBoss.shouldDoSweepAttack;
                    shadowBoss.currentPhase = ShadowBoss.phase.flail;
                }

                if (kb.IsKeyDown(Keys.G))
                {
                    //shadowBoss.clumpUpFromVertical();
                    player.health = 100;
                }


                if (kb.IsKeyDown(Keys.P) && oldkb.IsKeyUp(Keys.P))
                {
                    //shouldMakeShadowPaths = !shouldMakeShadowPaths;
                    shadowBoss.currentPhase = ShadowBoss.phase.shadowPaths;
                }

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

        public void useWeapon()
        {
            
        }

        public void drawText(SpriteFont fontToUse, string text, int centerX, int centerY, Color color)
        {
            Vector2 fontSize = fontToUse.MeasureString(text);
            spriteBatch.DrawString(fontToUse, text,
                new Vector2(centerX / 2 - fontSize.X / 2, centerY / 2 - fontSize.Y / 2), color);
        }

        public void drawText(string text, int centerX, int centerY, Color color)
        {
            Vector2 fontSize = debugFont.MeasureString(text);
            spriteBatch.DrawString(debugFont, text,
                new Vector2(centerX / 2 - fontSize.X / 2, centerY / 2 - fontSize.Y / 2), color);
        }

        public void drawTitleScreen()
        {
            GraphicsDevice.Clear(Color.Black);

            drawText(titleFont, "Shadow Snake's Nest", screenWidth, screenHeight - 200, Color.White);

            drawText("Use WASD or arrow keys to move", screenWidth, screenHeight * 2 - 350, Color.White);
            drawText("Don't let the Shadow Snake touch you", screenWidth, screenHeight * 2 - 280, Color.White);
            drawText("Collect powerups to hurt the Shadow Snake", screenWidth, screenHeight * 2 - 200, Color.White);


            if (fontClock % 45 == 0)
                shouldShowText = !shouldShowText;
            if (shouldShowText)
                drawText("Press Enter To Start", screenWidth, screenHeight * 2 - 100, Color.White);

            //spriteBatch.DrawString(debugFont, "Test", new Vector2(200, 0), Color.Green);

        }

        public void drawWin()
        {
            GraphicsDevice.Clear(Color.Black);
            if(gameClock > 3600)
            { 
            drawText("You beat the boss! It took you " +gameClock / 3600 + " minutes and " +(gameClock % 3600) / 60 +
                " seconds!", screenWidth, screenHeight, Color.White);
                drawText("Press Enter to play again or Escape to go to title!", screenWidth, screenHeight + 100, Color.White);


            }
            else
            {
                drawText("You beat the boss! It took you " + (gameClock / 60) +
                " seconds!", screenWidth, screenHeight, Color.White);
                drawText("Press Enter to play again or Escape to go to title!", screenWidth, screenHeight + 100, Color.White);
            }
        }

        public void drawLose()
        {
            GraphicsDevice.Clear(Color.Black);

            drawText("You ran out of health!", screenWidth, screenHeight, Color.White);
            drawText("Press Enter to restart", screenWidth, screenHeight + 80, Color.White);
            drawText("or Escape to go to title.", screenWidth, screenHeight + 160, Color.White);
        }

        public void drawShadowBossLevel()
        {
            
            if (gameClock % 600 == 0 && gameClock != 0)
                colorCountingUp = !colorCountingUp;

            if (gameClock % 10 == 0 && gameClock != 0)
            {
                if (colorCountingUp)
                    colorNum++;
                else
                    colorNum--;
            }

            if (lightCooldown > 0)
            {
                GraphicsDevice.Clear(Color.LightGray);
                    
            }
            else
            {
                GraphicsDevice.Clear(new Color(colorNum * 2, colorNum * 2, colorNum * 2));

            }

            //shadowBoss.drawCharacter(spriteBatch, Color.Black);
            //shadowBoss.drawCharacter(spriteBatch, Color.Black);
            shadowBoss.drawCharacter(spriteBatch, gameClock);

            if (player.hitCooldown > 0)
            {
                if (gameClock % 8 != 0)
                {
                    player.drawCharacter(spriteBatch, Color.White);
                    spriteBatch.Draw(bandana, new Rectangle(player.getRecX() - playerWidth /2, player.getRecY() + 5, playerWidth / 2, 8), Color.White);
                }
            }
            else
            {
                player.drawCharacter(spriteBatch, Color.White);
                spriteBatch.Draw(bandana, new Rectangle(player.getRecX() - playerWidth /2 , player.getRecY() + 5, playerWidth / 2, 8), Color.White);

            }

            //pathMaker.drawCharacter(spriteBatch, new Color(20, 20, 30));

            for (int i = 0; i < shadowPathList.Count; i++)
            {
                shadowPathList[i].drawCharacter(spriteBatch, new Color(0, 20, 0));
            }

            int alpha = 200;

            playerHealthBar.drawCharacter(spriteBatch, new Color(255, 0, 0, alpha), new Color(10, 50, 150, alpha));
            bossHealthBar.drawCharacter(spriteBatch, new Color(128, 128, 157, alpha), new Color(200, 200, 255, alpha));

            if (player.currentWeapon == Player.weapon.ring)
            {
                guardRing.drawCharacter(spriteBatch, Color.White);
            }

            for (int i = 0; i < collectables.Count; i++)
            {
                if (maxTimeCollectableOnScreen - collectables[i].timeExisted > 120)
                {
                    collectables[i].drawCharacter(spriteBatch, collectables[i].getColor());
                }
                else
                {
                    if(gameClock % 8 != 0)
                    {
                        collectables[i].drawCharacter(spriteBatch, collectables[i].getColor());

                    }
                }

            }

            //spriteBatch.DrawString(debugFont, "MouseX: " + mousePos.X + "MouseY: " + mousePos.Y, new Vector2(100, screenHeight - 120), Color.Green);
            //spriteBatch.DrawString(debugFont, "Hitcooldown: " + player.hitCooldown, new Vector2(100, screenHeight - 100), Color.Green);
            spriteBatch.DrawString(debugFont, "Health: " + player.health, new Vector2(100, screenHeight - 120), Color.Green);
            spriteBatch.DrawString(debugFont, "Boss Health: " + shadowBoss.health, new Vector2(100, screenHeight - 100), Color.Green);
            //spriteBatch.DrawString(debugFont, "currentShadowPath: " + currentShadowPathNum, new Vector2(100, screenHeight - 60), Color.Green);
            //spriteBatch.DrawString(debugFont, "shadowPathStartTime: " + shadowPathStartTime, new Vector2(100,  60), Color.Green);
            //spriteBatch.DrawString(debugFont, "patrolPosX: " + shadowBoss.patrolPos.X + " Y: " + shadowBoss.patrolPos.Y, new Vector2(100,  120), Color.Green);
            //spriteBatch.DrawString(debugFont, "bossHeadX: " + shadowBoss.getPartRec(0).X + "bossHeadY: " + shadowBoss.getPartRec(0).Y,
            //    new Vector2(100, screenHeight - 40), Color.Green);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            switch (state)
            {
                case gameState.shadowBoss:
                    drawShadowBossLevel();
                    break;
                case gameState.titleScreen:
                    drawTitleScreen();
                    break;
                case gameState.win:
                    drawWin();
                    break;
                case gameState.lose:
                    drawLose();
                    break;

            }

            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
