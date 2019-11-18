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
        Rectangle head;
        Rectangle body;
        Rectangle tail;

        enum facing
        {
            up = 1, right, down, left
        }

        facing direction = facing.left;




        //List<Rectangle> parts = new List<Rectangle>();

        bool widthLarger;
        bool hasStartedRotating = false;
        bool isFinishedRotating = false;

        int xHeadDistance = 0;
        int yHeadDistance = 0;

        int xBodyDistance = 0;
        int yBodyDistance = 0;

        int xTailDistance = 0;
        int yTailDistance = 0;

        List<Rectangle> bodyPartList = new List<Rectangle>();
        int numParts = 10;
        int partDimension = 70;

        public bool hasStartedAcrossAttack { get; set; }
        public bool shouldDoAcrossAttack { get; set; }

        public ShadowBoss(ref Texture2D tex, Rectangle rectangle) : base(ref tex, rectangle)
        {
            this.rec = rectangle;
            this.texture = tex;
            this.health = 150;

            if (rectangle.Width > rectangle.Height)
            {
                widthLarger = true;
                this.head = new Rectangle(rectangle.X, rectangle.Y + 10, rectangle.Width / 3, rectangle.Height);
                this.body = new Rectangle(rectangle.X + rectangle.Width / 3, rectangle.Y - 5, rectangle.Width / 3, rectangle.Height);
                this.tail = new Rectangle(rectangle.X + (rectangle.Width * 2) / 3, rectangle.Y + 10, rectangle.Width / 3, rectangle.Height);

            }
            else
            {
                widthLarger = false;

                this.head = new Rectangle(rectangle.X + 10, rectangle.Y, rectangle.Width, rectangle.Height / 3);
                this.body = new Rectangle(rectangle.X - 5, rectangle.Y + rectangle.Height / 3, rectangle.Width, rectangle.Height / 3);
                this.tail = new Rectangle(rectangle.X + 10, rectangle.Y + (rectangle.Height * 2) / 3, rectangle.Width, rectangle.Height / 3);

            }

            for (int i = 0; i < numParts; i++)
            {
                bodyPartList.Add(new Rectangle(rectangle.X + partDimension * i, rectangle.Y, partDimension, partDimension));
            }

            //parts.Add(head);
            //parts.Add(body);
            //parts.Add(tail);
        }

        public ShadowBoss(Texture2D tex, Rectangle head, Rectangle body, Rectangle tail) : base()
        {
            this.texture = tex;
            this.head = head;
            this.body = body;
            this.tail = tail;
        }

        public void moveUp()
        {

            for (int i = numParts - 2; i >= 0; i--)
            {
                bodyPartList[i] = new Rectangle(bodyPartList[i].X, bodyPartList[i].Y - (int)Math.Pow(numParts - i, 1.7) / 3,
                    bodyPartList[i].Width, bodyPartList[i].Height);
            }
        }

        public void moveDown()
        {

            for (int i = numParts - 2; i >= 0; i--)
            {
                bodyPartList[i] = new Rectangle(bodyPartList[i].X, bodyPartList[i].Y  + (int)Math.Pow(numParts - i, 1.7) / 3,
                    bodyPartList[i].Width, bodyPartList[i].Height);
            }
        }

        public void flailAttack()
        {

        }

        public void resetToStraight()
        {
            for (int i = numParts - 2; i >= 0; i--)
            {
                bodyPartList[i] = new Rectangle(bodyPartList[i].X, bodyPartList[numParts - 1].Y,
                    bodyPartList[i].Width, bodyPartList[i].Height);
            }
        }

        public void animate()
        {
            //if(widthLarger)
            //{
            //    for (int i = 0; i < parts.Count; i++)
            //    {


            //        if (parts[i].Center.Y < rec.Top)
            //        {
            //            parts[i].Y += 10;
            //        }
            //        else
            //        {
            //            parts[i].Y -= 10;
            //        }

            //    }
            //}
            //else
            //{
            //    for (int i = 0; i < parts.Length; i++)
            //    {


            //        if (parts[i].Center.X < rec.Left)
            //        {
            //            parts[i].X += 10;
            //        }
            //        else
            //        {
            //            parts[i].X -= 10;
            //        }

            //    }
            //}
        }

        public void acrossScreenAttack()
        {
            if (hasStartedAcrossAttack == false)
            {
                if (rec.Bottom + 50 > 0)
                {
                    this.rec.Y -= 1;
                    adjustSections();
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
            //head.X = body.X;
            //head.Y = body.Top - head.Height;
            //head.Height = (rec.Width / 3);
            //head.Width = rec.Height;


            //body.X += 20;
            //body.Y = head.Bottom;
            //body.Height = (rec.Width / 3);
            //body.Width = rec.Height;

            //tail.X = body.X - 30;
            //tail.Y = body.Bottom;
            //tail.Height = (rec.Width / 3);
            //tail.Width = rec.Height;


            if (hasStartedRotating == false)
            {
                if (rec.Contains(head.Center) == false ||
                    rec.Contains(body.Center) == false ||
                    rec.Contains(tail.Center) == false)
                {
                    head.X = rec.X;
                    head.Y = rec.Y;
                }
                int temp = rec.Height;

                //int tempPos = rec.Center.X - (rec.Width / 2);
                rec.Height = rec.Width;
                rec.Width = temp;
                //rec.X = head.Center.X - (rec.Width / 2);
                //rec.Y = head.Y;
                isFinishedRotating = false;
                hasStartedRotating = true;
            }

            if (isFinishedRotating == false)
            {
                if (xTailDistance == 0)
                {

                    //xHeadDistance = (int)Math.Pow(tail.Center.X - (rec.Center.X), 2);
                    //yHeadDistance = (int)Math.Pow(tail.Center.X - (rec.Center.X), 2);

                    //xBodyDistance = (int)Math.Pow(tail.Center.X - (rec.Center.X), 2);
                    //yBodyDistance = (int)Math.Pow(tail.Center.X - (rec.Center.X), 2);

                    xTailDistance = Math.Abs(tail.Center.X - (rec.Center.X));
                    yTailDistance = Math.Abs(tail.Center.Y - (rec.Y + rec.Height * 2 / 3));
                }

                //if (gameClock % 2 == 0)
                //{
                if (rec.Contains(tail.Center) == false)
                {
                    tail.X -= xTailDistance / 20;
                    tail.Y += yTailDistance / 20;
                }
                //}
            }




            //rec.X -= 40;

            widthLarger = false;
        }

        public void adjustSections()
        {
            if (widthLarger == true)
            {
                head.X = rec.X;
                body.X = rec.X + rec.Width / 3;
                tail.X = rec.X + rec.Width * 2 / 3;
            }
            else
            {
                head.Y = rec.Y + 10;
                body.Y = rec.Y - 5 + rec.Height / 3;
                tail.Y = rec.Y + 10 + rec.Height * 2 / 3;
            }

        }

        public override void drawCharacter(SpriteBatch sb)
        {
            sb.Draw(texture, head, Color.White);
            sb.Draw(texture, body, Color.White);
            sb.Draw(texture, tail, Color.White);
        }

        public override void drawCharacter(SpriteBatch sb, Color color)
        {
            sb.Draw(texture, head, color);
            sb.Draw(texture, body, color);
            sb.Draw(texture, tail, color);
        }

        public void drawCharacter(SpriteBatch sb, Color color, bool showBody)
        {
            if (showBody)
            {
                //sb.Draw(texture, head, Color.Red);
                //sb.Draw(texture, body, Color.Green);
                //sb.Draw(texture, tail, Color.Blue);
                for (int i = 0; i < numParts; i++)
                {
                    sb.Draw(texture, bodyPartList[i], new Color(i * 10, i * 5, i));
                }
                //sb.Draw(texture, rec, Color.White);
            }
        }

    }
}
