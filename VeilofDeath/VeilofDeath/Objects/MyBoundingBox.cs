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
        int iminX, imaxX, iminY, imaxY, iminZ, imaxZ;
        
        private AGameObject O_Parent;

        public MyBoundingBox(AGameObject gameObject)
        {
            O_Parent = gameObject;
            
            center = new Vector3(gameObject.Position.X + 2, gameObject.Position.Y + 2, gameObject.Position.Z);
            iminX = (int) center.X - GameConstants.iBlockSize / 2;
            imaxX = (int) center.X + GameConstants.iBlockSize / 2;
            iminY = (int) center.Y - GameConstants.iBlockSize / 2;            
            imaxY = (int) center.Y + GameConstants.iBlockSize / 2;
            iminZ = (int) center.Z - GameConstants.iBlockSize ;
            imaxZ = (int) center.Z + GameConstants.iBlockSize ;
            
        }

        /*
        public MyBoundingBox(SpikeTrap trap)
        {
            center = new Vector3(trap.Position.X + 2, trap.Position.Y + 2, trap.Position.Z);
            iminX = (int) center.X - GameConstants.iBlockSize / 4;
            iminY = (int) center.Y - GameConstants.iBlockSize / 4;
            iminZ = (int) center.Z - GameConstants.iBlockSize;
            imaxX = (int) center.X + GameConstants.iBlockSize / 4;
            imaxY = (int) center.Y + GameConstants.iBlockSize / 4;
            imaxZ = (int) center.Z + GameConstants.iBlockSize;
            Console.WriteLine("TrapCenterPosition: (" + center.X + "/ " + center.Y + "/ " + center.Z + ")");
        }
        */

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


        public void update(AGameObject gameObject)
        {
            iminX = (int) gameObject.Position.X - GameConstants.iBlockSize / 4;
            iminY = (int) gameObject.Position.Y - GameConstants.iBlockSize / 4;
            iminZ = (int) gameObject.Position.Z;// - GameConstants.iBlockSize * 2;
            imaxX = (int) gameObject.Position.X + GameConstants.iBlockSize / 4;
            imaxY = (int) gameObject.Position.Y + GameConstants.iBlockSize / 4;
            imaxZ = (int) gameObject.Position.Z;// + GameConstants.iBlockSize * 2;
        }


    }
}
