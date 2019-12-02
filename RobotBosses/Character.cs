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
    class Character
    {
        public Texture2D texture;
        public Rectangle rec;
        public int health { get; set; }
        public int startingHealth { get; set; }
        public int speed { get; set; } = 5;
        public int startingSpeed { get; set; } = 5;
        public int originalX { get; set; } = 5;
        public int originalY { get; set; } = 5;
        public int hitCooldown { get; set; } = 0;


        //public Rectangle rec2 { get { return rec2}; set; }

        public Character()
        {

        }

        public Character(Texture2D tex, Rectangle rectangle)
        {
            rec = rectangle;
            texture = tex;
        }

        public Character(ref Texture2D tex, Rectangle rectangle)
        {
            rec = rectangle;
            texture = tex;
        }

        public Rectangle getRec()
        {
            return rec;
        }

        public int getRecX()
        {
            return rec.X;
        }

        public int getRecY()
        {
            return rec.Y;
        }

        public void setRecX(int newValue)
        {
            rec.X = newValue;
        }

        public void setRecY(int newValue)
        {
            rec.Y = newValue;
        }

        public void setRecWidth(int newValue)
        {
            rec.Width = newValue;
        }

        public void setRecHeight(int newValue)
        {
            rec.Height = newValue;
        }

        public void incrementRecX(int amount)
        {
            rec.X += amount;
        }

        public void incrementRecWidth(int amount)
        {
            rec.Width += amount;
        }

        public void incrementRecHeight(int amount)
        {
            rec.Height += amount;
        }

        public void incrementRecY(int amount)
        {
            rec.Y += amount;
        }

        public void resetPos()
        {
            rec.X = originalX;
            rec.Y = originalY;
        }


        public virtual void drawCharacter(SpriteBatch sb)
        {
            sb.Draw(texture, rec, Color.White);
        }

        public virtual void drawCharacter(SpriteBatch sb, Color color)
        {
            sb.Draw(texture, rec, color);
        }
    }
}
