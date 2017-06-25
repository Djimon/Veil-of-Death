using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeilofDeath.Objects.Traps;

namespace VeilofDeath.Objects
{
   public class MyBoundingBox
    {
        private Vector3 center;
        private float iminX;
        private float iminY;
        private float iminZ;
        private float imaxX;
        private float imaxY;
        private float imaxZ;

        public MyBoundingBox(Player player)
        private AGameObject O_Parent;

        public MyBoundingBox(AGameObject gameObject)
        {
            iminX = player.Position.X - GameConstants.iBlockSize / 2;
            iminY = player.Position.Y - GameConstants.iBlockSize / 2;
            iminZ = player.Position.Z - GameConstants.iBlockSize * 2;
            imaxX = player.Position.X + GameConstants.iBlockSize / 2;
            imaxY = player.Position.Y + GameConstants.iBlockSize / 2;
            imaxZ = player.Position.Z + GameConstants.iBlockSize * 2;
            //ToDO: size nciht von der Position abhängig machen -> Kracht beim Player, da sich die Pos stets ändert
            O_Parent = gameObject;
            iminX = (int)gameObject.Position.X - GameConstants.iBlockSize / 2;
            iminY = (int)gameObject.Position.Y - GameConstants.iBlockSize * 2;
            iminZ = (int)gameObject.Position.Z - GameConstants.iBlockSize * 4;
            imaxX = (int)gameObject.Position.X + GameConstants.iBlockSize / 2;
            imaxY = (int)gameObject.Position.Y + GameConstants.iBlockSize * 2;
            imaxZ = (int)gameObject.Position.Z + GameConstants.iBlockSize * 4;
        }

        public MyBoundingBox(SpikeTrap trap)
        {
            center = new Vector3(trap.Position.X + 1.5f, trap.Position.Y + 1.5f, trap.Position.Z);
            iminX = center.X - GameConstants.iBlockSize / 2;
            iminY = center.Y - GameConstants.iBlockSize / 2;
            iminZ = center.Z - GameConstants.iBlockSize * 8;
            imaxX = center.X + GameConstants.iBlockSize / 2;
            imaxY = center.Y + GameConstants.iBlockSize / 2;
            imaxZ = center.Z + GameConstants.iBlockSize * 8;
        }

        public bool intersect(MyBoundingBox other)
        {
            //TODO: attach "isCollided" to Player not global
           O_Parent.hasCollided = ((this.iminX <= other.imaxX && this.imaxX >= other.iminX) &&
                                      (this.iminY <= other.imaxY && this.imaxY >= other.iminY) &&
                                      (this.iminZ <= other.imaxZ && this.imaxZ >= other.iminZ));

            //Console.WriteLine("Collision: " + GameConstants.isCollided);

            
            //Console.WriteLine("Trap x: " + other.imaxX + " y: " + other.imaxY + " z: " + other.imaxZ);

            return GameConstants.isCollided;
            return O_Parent.hasCollided;
        }

        public void update(Player player)
        {
            iminX = player.Position.X - GameConstants.iBlockSize / 2;
            iminY = player.Position.Y - GameConstants.iBlockSize / 2;
            iminZ = player.Position.Z - GameConstants.iBlockSize * 2;
            imaxX = player.Position.X + GameConstants.iBlockSize / 2;
            imaxY = player.Position.Y + GameConstants.iBlockSize / 2;
            imaxZ = player.Position.Z + GameConstants.iBlockSize * 2;

            Console.WriteLine("Player x: " + iminX + " y: " + iminY + " z: " + iminZ);
        }
    }
}
