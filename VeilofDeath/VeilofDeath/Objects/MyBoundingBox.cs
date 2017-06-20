using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeilofDeath.Objects
{
   public class MyBoundingBox
    {
        private int iminX;
        private int iminY;
        private int iminZ;
        private int imaxX;
        private int imaxY;
        private int imaxZ;

        public MyBoundingBox(AGameObject gameObject)
        {
            iminX = (int)gameObject.Position.X - GameConstants.iBlockSize / 2;
            iminY = (int)gameObject.Position.Y - GameConstants.iBlockSize * 2;
            iminZ = (int)gameObject.Position.Z - GameConstants.iBlockSize * 4;
            imaxX = (int)gameObject.Position.X + GameConstants.iBlockSize / 2;
            imaxY = (int)gameObject.Position.Y + GameConstants.iBlockSize * 2;
            imaxZ = (int)gameObject.Position.Z + GameConstants.iBlockSize * 4;
        }

        public bool intersect(MyBoundingBox other)
        {
           GameConstants.isCollided = ((this.iminX <= other.imaxX && this.imaxX >= other.iminX) &&
                                      (this.iminY <= other.imaxY && this.imaxY >= other.iminY) &&
                                      (this.iminZ <= other.imaxZ && this.imaxZ >= other.iminZ));

            //Console.WriteLine("Collision: " + GameConstants.isCollided);

            return GameConstants.isCollided;
        }

    }
}
