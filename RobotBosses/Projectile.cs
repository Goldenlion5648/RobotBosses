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
    class Projectile : Character
    {
        public int damage { get; set; }
        public Projectile(ref Texture2D tex, Rectangle rectangle) : base(ref tex, rectangle)
        {
            this.rec = rectangle;
            this.texture = tex;
            //this.startingHealth = 100;
        }

        public virtual void move(int gameClock)
        {

        }
    }
}
