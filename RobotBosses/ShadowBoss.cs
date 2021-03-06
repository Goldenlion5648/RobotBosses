﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RobotBosses
{
    class ShadowBoss : Enemy
    {

        enum facing
        {
            up = 1, right, down, left
        }

        facing facingDirection = facing.left;

        public enum phase
        {
            patrol = 0, flail, shadowPaths, debug
        }

        public phase currentPhase = phase.debug;

        //List<Rectangle> parts = new List<Rectangle>();

        bool widthLarger;
        bool hasStartedRotating = false;
        bool isFinishedRotating = false;

        //bool hasMovedInTick = false;

        public bool hasMovedInTick { get; set; } = false;

        Player player;
        Ring ringProjectile;

        List<SnakePart> bodyPartList = new List<SnakePart>();
        public int numParts { get; set; } = 16;
        public int damageToInflict { get; set; } = 30;
        //int numParts = 10;
        int partDimension = 40;

        public bool hasStartedAcrossAttack { get; set; }
        public bool shouldDoAcrossAttack { get; set; }

        private int currentYPart = 0;
        private int currentTransitionPart = 1;
        private bool isStraightened = true;
        private bool hasSetUpMoving = false;


        public bool shouldDoSweepAttack { get; set; } = false;
        bool isFlailingDown = false;
        bool isFlailingUp = true;

        public Point movementDestination { get; set; } = new Point(0, 0);
        public bool shouldMoveToPoint { get; set; } = false;

        public bool hasFinishedMoving { get; set; }
        
        private int colorCounter = 0;
        private bool colorCountUp = true;

        public int colorSwitchCount { get; set; } = 10;

        public int phaseCooldown { get; set; } = 0;

        int partsOnCooldown = 0;
        public int maxPossiblePartsOnCooldown { get; set; } = 4;
        //private int colorSwitchCount = 10;


        public Point startingPos { get; set; }
        public Point patrolPos { get; set; }

        public int patrolCooldown { get; set; } = 0;
        //private Point startingPos;

        public ShadowBoss(ref Texture2D tex, Point startingPos, ref Player player, ref Ring ringProjectile)
        {
            //this.rec = rectangle;
            this.texture = tex;
            this.health = 150;
            this.startingHealth = 150;
            this.speed = 6;
            this.player = player;
            this.startingPos = startingPos;
            this.ringProjectile = ringProjectile;

            for (int i = 0; i < numParts; i++)
            {
                bodyPartList.Add(new SnakePart(new Rectangle(startingPos.X + partDimension * i, startingPos.Y, partDimension, partDimension)));
            }

        }

        public SnakePart getPart(int index)
        {
            return bodyPartList[index];
        }

        public Rectangle getPartRec(int index)
        {
            return bodyPartList[index].getRec();
        }

        public void setPartX(int index, int x)
        {
            bodyPartList[index].setRecX(x);
        }

        public void setPartY(int index, int y)
        {
            bodyPartList[index].setRecY(y);
        }

        public void circleSpin()
        {

        }

        public void turnUp()
        {

            for (int i = numParts - 2; i >= 0; i--)
            {
                for (int j = 0; j < (int)Math.Pow(numParts - i, 1.9) / 2; j++)
                {
                    bodyPartList[i].incrementRecY(-1);
                    inflictDamageToPlayer();
                    takeDamage(ringProjectile);

                }
            }
            isStraightened = false;
        }

        public void turnDown()
        {

            for (int i = numParts - 2; i >= 0; i--)
            {
                for (int j = 0; j < (int)Math.Pow(numParts - i, 1.9) / 2; j++)
                {
                    bodyPartList[i].incrementRecY(1);
                    inflictDamageToPlayer();

                }
            }
            isStraightened = false;
        }

        public void verticalSweepAttack()
        {
            if (currentPhase != phase.flail)
                return;



            if (colorSwitchCount == 10)
            {
                colorSwitchCount = 0;
                straightenInPlace();
                phaseCooldown = 1000;
            }
            //if(bodyPartList[numParts - 1].getRecX() > (numParts - 1) * partDimension + startingPos.X)
            if (phaseCooldown > 998)
            {
                if (new Point(bodyPartList[0].getRecX(), bodyPartList[0].getRecY()) != startingPos)
                {
                    for (int i = 0; i < speed; i++)
                    {
                        hasMovedInTick = false;
                        moveToPoint(startingPos);
                        if (bodyPartList[numParts - 2].getRecX() - 2 == (numParts - 1) * partDimension + startingPos.X)
                        {
                            //resetPos();
                            return;
                        }

                    }
                    phaseCooldown += 1;
                    return;

                }
                else
                {
                    resetPos();
                    straightenInPlace();
                    spaceOut();
                }
            }
            if (colorSwitchCount < 3)
            {
                return;
                //shouldDoSweepAttack = false;
            }

            //if (shouldDoSweepAttack == false)
            //return;

            if (bodyPartList[0].getRecY() < -4000)
            {
                isFlailingDown = true;
                isFlailingUp = false;
            }

            if (bodyPartList[0].getRecY() > 4200)
            {
                isFlailingDown = false;
                isFlailingUp = true;
            }

            if (isFlailingDown)
            {
                turnDown();
            }

            if (isFlailingUp)
            {
                turnUp();
            }
        }

        public void moveLeft(int sideSpeed)
        {
            for (int i = 0; i < numParts; i++)
            {
                for (int j = 0; j < sideSpeed; j++)
                {
                    bodyPartList[i].incrementRecX(-1);
                    inflictDamageToPlayer();
                }
            }
        }

        public void moveRight(int sideSpeed)
        {
            for (int i = 0; i < numParts; i++)
            {
                for (int j = 0; j < sideSpeed; j++)
                {
                    bodyPartList[i].incrementRecX(1);
                    inflictDamageToPlayer();
                }
            }
        }

        public void inflictDamageToPlayer()
        {
            for (int i = numParts - 1; i >= 0; i--)
            {
                if (bodyPartList[i].getRec().Intersects(player.getRec()))
                {
                    if (player.hitCooldown == 0)
                    {
                        player.hitCooldown = 120;
                        player.health -= damageToInflict;
                    }
                }
            }
        }

        public void takeDamage(Projectile projectile)
        {
            partsOnCooldown = 0;

            for (int i = 0; i < numParts; i++)
            {
                if (bodyPartList[i].hitCooldown > 0)
                {
                    partsOnCooldown++;
                }
            }

            for (int i = 0; i < numParts; i++)
            {
                if (bodyPartList[i].getRec().Intersects(projectile.getRec()) && bodyPartList[i].hitCooldown == 0)
                {
                    if (partsOnCooldown <= maxPossiblePartsOnCooldown)
                    {
                        bodyPartList[i].hitCooldown = 120;
                        this.health -= projectile.damage;
                    }

                }
            }
        }

        public void patrol()
        {
            if (currentPhase != phase.patrol)
                return;

            if (colorSwitchCount == 10)
            {
                colorSwitchCount = 0;
                straightenInPlace();
                phaseCooldown = 1000;

                if (Math.Abs(bodyPartList[0].getRecX() - player.getRecX()) > Math.Abs(bodyPartList[0].getRecY() - player.getRecY()))
                {
                    patrolPos = new Point(player.getRecX(), bodyPartList[0].getRecY());

                }
                else
                {
                    patrolPos = new Point(bodyPartList[0].getRecX(), player.getRecY());
                }
                //if (player.getRecX() > Game1.screenWidth / 2)
                //{
                //    patrolPos = new Point(Game1.screenWidth / 2, player.getRecY());
                //}
                //else
                //{
                //    patrolPos = new Point(player.getRecX(), player.getRecY());
                //}
            }

            if (new Point(bodyPartList[0].getRecX(), bodyPartList[0].getRecY()) != patrolPos)
            {
                if (patrolCooldown == 0)
                {
                    for (int i = 0; i < speed / 2; i++)
                    {

                        hasMovedInTick = false;
                        moveToPoint(patrolPos);
                    }
                }
                else
                {
                    if (patrolCooldown > 0)
                        patrolCooldown--;
                }
            }
            else
            {
                patrolCooldown = 5;

                if (Math.Abs(bodyPartList[0].getRecX() - player.getRecX()) > Math.Abs(bodyPartList[0].getRecY() - player.getRecY()))
                {
                    patrolPos = new Point(player.getRecX(), bodyPartList[0].getRecY());

                }
                else
                {
                    patrolPos = new Point(bodyPartList[0].getRecX(), player.getRecY());
                }

            }

        }

        public void moveToPoint(Point destination)
        {
            //if (shouldMoveToPoint == false)
            //    return;
            //resetToStraight();
            if (hasSetUpMoving == false)
            {
                //int displacement = 0;
                if ((destination.Y - bodyPartList[0].getRecY()) < -50)
                {
                    turnUp();
                    //displacement = -1;

                }
                else if ((destination.Y - bodyPartList[0].getRecY()) > 50)
                {
                    turnDown();
                    //displacement = 1;

                }
                hasSetUpMoving = true;
                currentYPart = 0;

            }


            int xDirection = -1;

            if ((destination.X - bodyPartList[0].getRecX()) < 0)
                xDirection = -1;
            else if ((destination.X - bodyPartList[0].getRecX()) > 0)
                xDirection = 1;
            else
                xDirection = 0;

            for (int j = numParts - 1; j >= 1; j--)
            {
                for (int i = 0; i < speed; i++)
                {
                    bodyPartList[j].setRecX(bodyPartList[j - 1].getRecX() + bodyPartList[0].getRec().Width - xDirection * 3);
                    //bodyPartList[j].incrementRecX(xDirection * 3);
                }
            }


            if (bodyPartList[0].getRec().X != destination.X)
            {
                bodyPartList[0].incrementRecX(xDirection);
            }

            int yDirection = -1;
            if ((destination.Y - bodyPartList[0].getRecY()) < 0)
                yDirection = -1;
            else if ((destination.Y - bodyPartList[0].getRecY()) > 0)
                yDirection = 1;
            else
                yDirection = 0;

            //if (yDirection == 0 || Math.Abs(destination.Y - bodyPartList[0].getRecY()) < this.speed)
            {
                for (int i = 0; i < speed; i++)
                {
                    if ((destination.Y - bodyPartList[currentYPart].getRecY()) < 0)
                        yDirection = -1;
                    else if ((destination.Y - bodyPartList[currentYPart].getRecY()) > 0)
                        yDirection = 1;
                    else
                        yDirection = 0;

                    bodyPartList[currentYPart].incrementRecY(yDirection);
                }
                bool isDone = true;
                for (int i = 0; i < numParts; i++)
                {
                    if ((destination.Y - bodyPartList[i].getRecY()) < 0)
                        yDirection = -1;
                    else if ((destination.Y - bodyPartList[i].getRecY()) > 0)
                        yDirection = 1;
                    else
                        yDirection = 0;

                    if (bodyPartList[i].getRecY() != destination.Y)
                    {
                        bodyPartList[i].incrementRecY(yDirection);
                        isDone = false;
                        //break;
                    }
                }
                if (isDone)
                {
                    currentYPart = 0;
                    hasSetUpMoving = false;
                }

            }


            if (currentYPart < numParts)
            {
                currentYPart++;
            }
            if (currentYPart == numParts)
            {
                currentYPart = 0;
            }

            if ((destination.Y - bodyPartList[0].getRecY()) < 0)
                yDirection = -1;
            else if ((destination.Y - bodyPartList[0].getRecY()) > 0)
                yDirection = 1;
            else
                yDirection = 0;

        }

        public void resetPosition()
        {

            for (int i = 0; i < numParts; i++)
            {
                bodyPartList[i] = (new SnakePart(new Rectangle(startingPos.X + partDimension * i, startingPos.Y, partDimension, partDimension)));
            }
        }

        public void spaceOut()
        {
            for (int i = 1; i < numParts; i++)
            {
                bodyPartList[i].setRecX(bodyPartList[i - 1].getRec().Right);
            }
        }

        public void straightenInPlace()
        {

            for (int i = 0; i < numParts; i++)
            {
                bodyPartList[i].setRecY(bodyPartList[numParts - 1].getRecY());
            }
            isStraightened = true;
        }

        public void acrossScreenAttack()
        {
            if (hasStartedAcrossAttack == false)
            {
                if (rec.Bottom + 50 > 0)
                {
                    this.rec.Y -= 1;
                    //adjustSections();
                }
                else
                {
                    hasStartedAcrossAttack = true;
                }
            }

        }

        public void spinAttack()
        {

        }

        public void splitUpAttack()
        {

        }

        public void clumpUpFromLeftHorizontal()
        {
            for (int i = 1; i < numParts; i++)
            {
                for (int j = 0; j < speed; j++)
                {
                    if (bodyPartList[i].getRecX() > bodyPartList[0].getRecX())
                    {
                        bodyPartList[i].incrementRecX(-1);
                        inflictDamageToPlayer();
                    }
                }
            }
        }

        public void clumpUpFromRightHorizontal()
        {
            for (int i = 1; i < numParts; i++)
            {
                for (int j = 0; j < speed; j++)
                {
                    if (bodyPartList[i].getRecX() < bodyPartList[0].getRecX())
                    {
                        bodyPartList[i].incrementRecX(1);
                        inflictDamageToPlayer();
                    }
                }
            }
        }

        public void clumpUpFromDownwardVertical()
        {
            for (int i = 1; i < numParts; i++)
            {
                for (int j = 0; j < speed; j++)
                {
                    if (bodyPartList[i].getRecY() < bodyPartList[0].getRecY())
                    {
                        bodyPartList[i].incrementRecY(1);
                        inflictDamageToPlayer();
                    }
                }
            }
        }

        public void clumpUpFromUpwardVertical()
        {
            for (int i = 1; i < numParts; i++)
            {
                for (int j = 0; j < speed; j++)
                {
                    if (bodyPartList[i].getRecY() > bodyPartList[0].getRecY())
                    {
                        bodyPartList[i].incrementRecY(-1);
                        inflictDamageToPlayer();
                    }
                }
            }
        }

        public void transitionToDownwardVerticalFromLeft()
        {
            clumpUpFromLeftHorizontal();
            hasFinishedMoving = true;
            for (int j = 0; j <= currentTransitionPart - 1; j++)
            {
                for (int i = 0; i < speed; i++)
                {
                    //{
                    if (bodyPartList[j].getRec().Intersects(bodyPartList[j + 1].getRec()))
                    {
                        bodyPartList[j].incrementRecY(1);
                        hasFinishedMoving = false;
                    }
                }
            }
            if (hasFinishedMoving)
            {
                facingDirection = facing.down;

                currentTransitionPart++;
                if (currentTransitionPart == numParts)
                    currentTransitionPart = 1;
            }
        }

        public void transitionToDownwardVerticalFromRight()
        {
            clumpUpFromRightHorizontal();
            hasFinishedMoving = true;
            for (int j = 0; j <= currentTransitionPart - 1; j++)
            {
                for (int i = 0; i < speed; i++)
                {
                    //{
                    if (bodyPartList[j].getRec().Intersects(bodyPartList[j + 1].getRec()))
                    {
                        bodyPartList[j].incrementRecY(1);
                        hasFinishedMoving = false;
                    }
                }
            }
            if (hasFinishedMoving)
            {
                facingDirection = facing.down;

                currentTransitionPart++;
                if (currentTransitionPart == numParts)
                    currentTransitionPart = 1;
            }
        }

        public void transitionToUpwardVertical()
        {
            clumpUpFromLeftHorizontal();
            hasFinishedMoving = true;

            for (int j = 0; j <= currentTransitionPart - 1; j++)
            {
                for (int i = 0; i < speed; i++)
                {
                    //{
                    if (bodyPartList[j].getRec().Intersects(bodyPartList[j + 1].getRec()))
                    {
                        bodyPartList[j].incrementRecY(-1);
                        hasFinishedMoving = false;
                    }
                }
            }
            if (hasFinishedMoving)
            {
                facingDirection = facing.up;

                currentTransitionPart++;
                if (currentTransitionPart == numParts)
                    currentTransitionPart = 1;
            }
        }

        public void transitionToLeftHorizontalFromDown()
        {
            clumpUpFromDownwardVertical();



            hasFinishedMoving = true;

            for (int j = 0; j <= currentTransitionPart - 1; j++)
            {
                for (int i = 0; i < speed; i++)
                {
                    //{
                    if (bodyPartList[j].getRec().Intersects(bodyPartList[j + 1].getRec()))
                    {
                        bodyPartList[j].incrementRecX(-1);
                        hasFinishedMoving = false;
                    }
                }
            }
            if (hasFinishedMoving)
            {
                facingDirection = facing.left;

                currentTransitionPart++;
                if (currentTransitionPart == numParts)
                    currentTransitionPart = 1;
            }
        }

        public void transitionToLeftHorizontalFromUp()
        {
            clumpUpFromUpwardVertical();




            hasFinishedMoving = true;

            for (int j = 0; j <= currentTransitionPart - 1; j++)
            {
                for (int i = 0; i < speed; i++)
                {
                    //{
                    if (bodyPartList[j].getRec().Intersects(bodyPartList[j + 1].getRec()))
                    {
                        bodyPartList[j].incrementRecX(-1);
                        hasFinishedMoving = false;
                    }
                }
            }
            if (hasFinishedMoving)
            {
                facingDirection = facing.left;

                currentTransitionPart++;
                if (currentTransitionPart == numParts)
                    currentTransitionPart = 1;
            }
        }

        public void transitionToRightHorizontalFromUp()
        {
            clumpUpFromUpwardVertical();

            hasFinishedMoving = true;

            for (int j = 0; j <= currentTransitionPart - 1; j++)
            {
                for (int i = 0; i < speed; i++)
                {
                    //{
                    if (bodyPartList[j].getRec().Intersects(bodyPartList[j + 1].getRec()))
                    {
                        bodyPartList[j].incrementRecX(1);
                        hasFinishedMoving = false;
                    }
                }
            }
            if (hasFinishedMoving)
            {
                facingDirection = facing.right;

                currentTransitionPart++;
                if (currentTransitionPart == numParts)
                    currentTransitionPart = 1;
            }
        }

        public void drawPhase(SpriteBatch sb, SnakePart part, int gameClock, int num1, int num2, int num3)
        {

                if (partsOnCooldown >= maxPossiblePartsOnCooldown)
                {
                    if (gameClock % 8 != 0)
                    {
                        sb.Draw(texture, part.getRec(), new Color(num1, num2, num3));

                    }
                }
                else
                {
                    if (part.hitCooldown > 0)
                    {
                        if (gameClock % 8 != 0)
                        {
                            sb.Draw(texture, part.getRec(), new Color(num1, num2, num3));

                        }
                    }
                    else
                    {
                        sb.Draw(texture, part.getRec(), new Color(num1, num2, num3));
                    }
                }
            
        }

        public void drawCharacter(SpriteBatch sb, int gameClock)
        {

            //sb.Draw(texture, head, Color.Red);
            //sb.Draw(texture, body, Color.Green);
            //sb.Draw(texture, tail, Color.Blue);
            sb.Draw(texture, rec, Color.White);


            if (currentPhase == phase.flail)
            {
                for (int i = 0; i < numParts; i++)
                {
                    drawPhase(sb, bodyPartList[i], gameClock, i * colorCounter, i * colorCounter, i);
                }

            }
            else if (currentPhase == phase.patrol)
            {

                for (int i = 0; i < numParts; i++)
                {
                    drawPhase(sb, bodyPartList[i], gameClock, i * colorCounter, i, i);
                }

            }
            else if (currentPhase == phase.shadowPaths)
            {

                for (int i = 0; i < numParts; i++)
                {
                    drawPhase(sb, bodyPartList[i], gameClock, i, i, i * colorCounter);
                }

            }
            else
            {
                for (int i = 0; i < numParts; i++)
                {
                    drawPhase(sb, bodyPartList[i], gameClock, i * 10, i * colorCounter, i);
                }
            }

            //for (int i = 0; i < numParts; i++)
            //{
            //    sb.Draw(texture, bodyPartList[i].getRec(), new Color(i * colorCounter, i * colorCounter, i));
            //}

            if (colorSwitchCount < 3)
            {
                if (gameClock % 2 == 0)
                {
                    if (colorCountUp)
                    {
                        colorCounter++;
                        if (colorCounter == 16)
                        {
                            colorCountUp = false;
                            colorSwitchCount++;
                        }
                    }
                    else
                    {
                        colorCounter--;
                        if (colorCounter == -1)
                            colorCountUp = true;
                    }
                }
            }
            else
            {
                colorCounter = 10;
            }


        }

    }
}
