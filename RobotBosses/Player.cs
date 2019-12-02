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
    class Player: Character
    {
        //public int hitCooldown { get; set; }
        public enum weapon
        {
            fist, ring, 
        }

        public weapon currentWeapon { get; set; } = weapon.fist;


        public Player(ref Texture2D tex, Rectangle rectangle) : base( ref tex, rectangle)
        {
            this.rec = rectangle;
            this.texture = tex;
            this.health = 100;
            this.startingHealth = 100;
        }

    }
}
