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
    class Ring : Projectile, IProjectile
    {
        Player player;
        public Ring(Texture2D tex, Rectangle rectangle, ref Player player, int damage) : base(ref tex, rectangle)
        {
            this.rec = rectangle;
            this.texture = tex;
            this.player = player;
            this.damage = damage;
            //this.startingHealth = 100;
        }

        public override void move(int gameClock)
        {
            float time = gameClock; 
            float speed = MathHelper.Pi / 48; 
            float radius = 120.0f;
            Vector2 origin = player.getRec().Center.ToVector2(); 


            //this.rec.X = (int)(Math.Sin(time * speed + 5) * radius + origin.X);
            //this.rec.Y = (int)(Math.Cos(time * speed + 5) * radius + origin.Y);

            this.rec.X = (int)(Math.Cos(time * speed) * radius + origin.X) - rec.Width / 2;
            this.rec.Y = (int)(Math.Sin(time * speed) * radius + origin.Y) - rec.Height / 2;
        }

    }
}
