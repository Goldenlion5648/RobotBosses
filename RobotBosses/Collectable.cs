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
    class Collectable : Character
    {
        Player player;

        public bool shouldDraw { get; set; }

        public int timeExisted { get; set; } = 0;

        public enum collectableType
        {
            ring = 0, healthPack, speed, lights
        }

        public collectableType typeOfCollectable { get; set; } = 0;


        public Collectable(ref Texture2D tex, Rectangle rectangle, ref Player player)
        {
            rec = rectangle;
            texture = tex;
            this.player = player;
        }

        public Color getColor()
        {
            if (typeOfCollectable == collectableType.ring)
                return Color.Green;
            else if (typeOfCollectable == collectableType.healthPack)
                return Color.Red;
            else if (typeOfCollectable == collectableType.speed)
                return Color.Blue;
            else
                return Color.LightGoldenrodYellow;
        }

        public bool onCollect()
        {
            if (rec.Intersects(player.getRec()) == false)
                return false;

            switch (typeOfCollectable)
            {
                case collectableType.ring:
                    player.currentWeapon = Player.weapon.ring;
                    player.weaponCooldown = 570;
                    break;
                case collectableType.healthPack:
                    player.health += 20;
                    if (player.health > player.startingHealth)
                        player.health = player.startingHealth;
                    break;
                case collectableType.speed:
                    player.speed = player.startingSpeed;
                    player.speed += 3;
                    player.speedCooldown = 700;
                    break;
                case collectableType.lights:
                    Game1.lightCooldown = 600;
                    break;
            }
            return true;

        }
    }
}
