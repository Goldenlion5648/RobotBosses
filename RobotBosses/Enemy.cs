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
    class Enemy : Character
    {

        public Enemy(ref Texture2D tex, Rectangle rectangle) : base(ref tex, rectangle)
        {
            this.rec = rectangle;
            this.texture = tex;
        }

        public Enemy()
        {

        }

    }
}
