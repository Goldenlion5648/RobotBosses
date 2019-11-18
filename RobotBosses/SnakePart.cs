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
    class SnakePart : Character
    {

        List<Point> cornerList = new List<Point>();

        public SnakePart(ref Texture2D tex, Rectangle rectangle)
        {
            this.rec = rectangle;
            this.texture = tex;

            originalX = rectangle.X;
            originalY = rectangle.Y;
        }

        public SnakePart(Rectangle rectangle)
        {
            this.rec = rectangle;
            
        }

    }
}
