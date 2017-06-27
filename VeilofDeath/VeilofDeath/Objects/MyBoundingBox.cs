using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


        //private AGameObject O_Parent;

        public MyBoundingBox(Player player)
        {
            center = new Vector3(player.Position.X, player.Position.Y, player.Position.Z);
            iminX = center.X - GameConstants.iBlockSize / 4;
            iminY = center.Y - GameConstants.iBlockSize / 4;
            iminZ = center.Z - GameConstants.iBlockSize ;
            imaxX = center.X + GameConstants.iBlockSize / 4;
            imaxY = center.Y + GameConstants.iBlockSize / 4;
            imaxZ = center.Z + GameConstants.iBlockSize ;
            //ToDO: size nciht von der Position abhängig machen -> Kracht beim Player, da sich die Pos stets ändert
            /*O_Parent = gameObject;
            iminX = (int)gameObject.Position.X - GameConstants.iBlockSize / 2;
            iminY = (int)gameObject.Position.Y - GameConstants.iBlockSize * 2;
            iminZ = (int)gameObject.Position.Z - GameConstants.iBlockSize * 4;
            imaxX = (int)gameObject.Position.X + GameConstants.iBlockSize / 2;
            imaxY = (int)gameObject.Position.Y + GameConstants.iBlockSize * 2;
            imaxZ = (int)gameObject.Position.Z + GameConstants.iBlockSize * 4;
            */
        }

        public MyBoundingBox(SpikeTrap trap)
        {
            center = new Vector3(trap.Position.X , trap.Position.Y , trap.Position.Z);
            iminX = center.X - GameConstants.iBlockSize / 2;
            iminY = center.Y - GameConstants.iBlockSize / 2;
            iminZ = center.Z - GameConstants.iBlockSize;
            imaxX = center.X + GameConstants.iBlockSize / 2;
            imaxY = center.Y + GameConstants.iBlockSize / 2;
            imaxZ = center.Z + GameConstants.iBlockSize;
            Console.WriteLine("TrapCenterPosition: (" + center.X + "/ " + center.Y + "/ " + center.Z + ")");
        }

        public bool intersect(MyBoundingBox other)
        {
            return GameConstants.isCollided = ((this.iminX <= other.imaxX && this.imaxX >= other.iminX)&&
                                      (this.iminY <= other.imaxY && this.imaxY >= other.iminY) &&
                                      (this.iminZ <= other.imaxZ && this.imaxZ >= other.iminZ));

            /*
            Console.WriteLine("Collision: " + GameConstants.isCollided);
            Console.WriteLine("Player x: " + iminX + " y: " + iminY + " z: " + iminZ);
            Console.WriteLine("Trap x: " + other.imaxX + " y: " + other.imaxY + " z: " + other.imaxZ);
            */


        }

        public void update(Player player)
        {
            iminX = player.Position.X - GameConstants.iBlockSize / 4;
            iminY = player.Position.Y - GameConstants.iBlockSize / 4;
            iminZ = player.Position.Z - GameConstants.iBlockSize;
            imaxX = player.Position.X + GameConstants.iBlockSize / 4;
            imaxY = player.Position.Y + GameConstants.iBlockSize / 4;
            imaxZ = player.Position.Z + GameConstants.iBlockSize;
        }

    }
}
