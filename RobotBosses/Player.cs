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
    class Player: Character
    {
        public int hitCooldown { get; set; }

        public Player(Texture2D tex, Rectangle rectangle) : base(tex, rectangle)
        {
            this.rec = rectangle;
            this.texture = tex;
            this.health = 100;
        }

        public void Update(GameTime gameTime)
        {
            //if(rec.Intersects())
            hitCooldown--;
        }
    }
}
