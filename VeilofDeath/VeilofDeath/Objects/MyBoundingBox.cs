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

        
        //private AGameObject O_Parent;

        public MyBoundingBox(AGameObject gameObject)
        {


            center = new Vector3(gameObject.Position.X + 2, gameObject.Position.Y + 2, gameObject.Position.Z);
            iminX = (int) center.X - GameConstants.iBlockSize / 4;
            imaxX = (int) center.X + GameConstants.iBlockSize / 4;
            iminY = (int) center.Y - GameConstants.iBlockSize / 4;            
            imaxY = (int) center.Y + GameConstants.iBlockSize / 4;
            iminZ = (int) center.Z - GameConstants.iBlockSize ;
            imaxZ = (int) center.Z + GameConstants.iBlockSize ;
            */
        }

        public MyBoundingBox(SpikeTrap trap)
        {
            center = new Vector3(trap.Position.X + 2, trap.Position.Y + 2, trap.Position.Z);
            iminX = center.X - GameConstants.iBlockSize / 4;
            iminY = center.Y - GameConstants.iBlockSize / 4;
            iminZ = center.Z - GameConstants.iBlockSize;
            imaxX = center.X + GameConstants.iBlockSize / 4;
            imaxY = center.Y + GameConstants.iBlockSize / 4;
            imaxZ = center.Z + GameConstants.iBlockSize;
            Console.WriteLine("TrapCenterPosition: (" + center.X + "/ " + center.Y + "/ " + center.Z + ")");
        }

        public bool intersect(MyBoundingBox other)
        {
            
           O_Parent.hasCollided = ((this.iminX <= other.imaxX && this.imaxX >= other.iminX) &&
                                      (this.iminY <= other.imaxY && this.imaxY >= other.iminY) &&
                                      (this.iminZ <= other.imaxZ && this.imaxZ >= other.iminZ));

            /*
            Console.WriteLine("Collision: " + GameConstants.isCollided);
            Console.WriteLine("Player x: " + iminX + " y: " + iminY + " z: " + iminZ);
            Console.WriteLine("Trap x: " + other.imaxX + " y: " + other.imaxY + " z: " + other.imaxZ);
            */

            return O_Parent.hasCollided;
        }

/*
        public void update(Player player)
        {
            iminX = player.Position.X - GameConstants.iBlockSize / 4;
            iminY = player.Position.Y - GameConstants.iBlockSize / 4;
            iminZ = player.Position.Z;// - GameConstants.iBlockSize * 2;
            imaxX = player.Position.X + GameConstants.iBlockSize / 4;
            imaxY = player.Position.Y + GameConstants.iBlockSize / 4;
            imaxZ = player.Position.Z;// + GameConstants.iBlockSize * 2;
        }
*/

    }
}
