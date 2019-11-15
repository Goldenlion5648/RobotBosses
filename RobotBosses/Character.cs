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
        Texture2D texture;
        Rectangle rec;

        public Character(Texture2D tex, Rectangle rectangle)
        {
            rec = rectangle;
            texture = tex;
        }


    }
}
