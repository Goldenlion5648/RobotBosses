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
    class HealthBar : Character
    {
        Rectangle background;
        public HealthBar(ref Texture2D tex, Rectangle rectangle, int border) : base(ref tex, rectangle)
        {
            this.rec = rectangle;
            this.texture = tex;

            this.background = new Rectangle(rec.X - border, rec.Y - border, rec.Width + border * 2, rec.Height+ border * 2);
        }

        public virtual void drawCharacter(SpriteBatch sb, Color foreground, Color backgroundColor)
        {
            sb.Draw(texture, background, backgroundColor);
            sb.Draw(texture, rec, foreground);
        }
    }
}
