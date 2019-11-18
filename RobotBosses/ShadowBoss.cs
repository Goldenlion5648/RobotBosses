using System;
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

        facing direction = facing.left;

        //List<Rectangle> parts = new List<Rectangle>();

        bool widthLarger;
        bool hasStartedRotating = false;
        bool isFinishedRotating = false;

        //bool hasMovedInTick = false;

        public bool hasMovedInTick { get; set; } = false;

        Player player;

        List<SnakePart> bodyPartList = new List<SnakePart>();
        public int numParts { get; set; } = 14;
        public int damageToInflict { get; set; } = 30;
        //int numParts = 14;
        int partDimension = 50;

        public bool hasStartedAcrossAttack { get; set; }
        public bool shouldDoAcrossAttack { get; set; }

        public ShadowBoss(ref Texture2D tex, Rectangle rectangle, ref Player player)
        {
            this.rec = rectangle;
            this.texture = tex;
            this.health = 150;
            this.speed = 10;
            this.player = player;

            for (int i = 0; i < numParts; i++)
            {
                bodyPartList.Add(new SnakePart(new Rectangle(rectangle.X + partDimension * i, rectangle.Y, partDimension, partDimension)));
            }

        }

        //public ShadowBoss(Texture2D tex, Rectangle head, Rectangle body, Rectangle tail) : base()
        //{
        //    this.texture = tex;
        //    this.head = head;
        //    this.body = body;
        //    this.tail = tail;
        //}

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

        public void turnDown()
        {

            for (int i = numParts - 2; i >= 0; i--)
            {
                bodyPartList[i].incrementRecY((int)Math.Pow(numParts - i, 1.9) / 2);
            }
        }

        public void moveToPoint(Point destination)
        {
            //resetToStraight();

            int xDirection = -1;

            if ((destination.X - bodyPartList[0].getRecX()) < 0)
                xDirection = -1;
            else if ((destination.X - bodyPartList[0].getRecX()) > 0)
                xDirection = 1;
            else
                xDirection = 0;

            //for (int i = 0; i < speed; i++)
            //{
            //    for (int j = 0; j < numParts; j++)
            //    {
            //        if (bodyPartList[j].getRec().X != destination.X)
            //        {
            //            bodyPartList[j].incrementRecX(xDirection);
            //        }
            //    }
            //}

            for (int j = numParts - 1; j >= 1; j--)
            {
                //for (int i = 0; i < speed; i++)
                //{
                bodyPartList[j].setRecX(bodyPartList[j - 1].getRecX() + bodyPartList[j - 1].getRec().Width - 5);
                //}
            }
            if (hasMovedInTick == false)
            {
                for (int i = 0; i < speed; i++)
                {

                    if (bodyPartList[0].getRec().X != destination.X)
                    {
                        bodyPartList[0].incrementRecX(xDirection);
                    }

                }
            }


            int yDirection = -1;
            if ((destination.Y - bodyPartList[0].getRecY()) < 0)
                yDirection = -1;
            else if ((destination.Y - bodyPartList[0].getRecY()) > 0)
                yDirection = 1;
            else
                yDirection = 0;


            for (int j = numParts - 1; j >= 1; j--)
            {
                //for (int i = 0; i < speed; i++)
                //{
                bodyPartList[j].setRecY(bodyPartList[j - 1].getRecY() - yDirection * 4);
                //}
            }
            if (hasMovedInTick == false)
            {
                for (int i = 0; i < speed; i++)
                {
                    if (bodyPartList[0].getRec().Y != destination.Y)
                    {
                        bodyPartList[0].incrementRecY(yDirection);
                    }
                }
                hasMovedInTick = true;
            }
        }

        public void flailAttack()
        {

        }

        public void resetPosition()
        {
            //for (int i = numParts - 2; i >= 0; i--)
            //{
            //    bodyPartList[i] = new Rectangle(bodyPartList[i].X, bodyPartList[numParts - 1].Y,
            //        bodyPartList[i].Width, bodyPartList[i].Height);
            //}

            for (int i = 0; i < numParts; i++)
            {
                bodyPartList[i] = (new SnakePart(new Rectangle(rec.X + partDimension * i, rec.Y, partDimension, partDimension)));
            }
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

        public void rotateToUpper(int gameClock)
        {

        }

        public void drawCharacter(SpriteBatch sb, Color color)
        {

            //sb.Draw(texture, head, Color.Red);
            //sb.Draw(texture, body, Color.Green);
            //sb.Draw(texture, tail, Color.Blue);
            sb.Draw(texture, rec, Color.White);
            for (int i = 0; i < numParts; i++)
            {
                sb.Draw(texture, bodyPartList[i].getRec(), new Color(i * 10, i * 5, i));
            }
        }


    }
}
