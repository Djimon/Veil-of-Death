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
        private int iminX;
        private int iminY;
        private int iminZ;
        private int imaxX;
        private int imaxY;
        private int imaxZ;
        private Vector3 center;

        private AGameObject O_Parent;

        public MyBoundingBox(AGameObject gameObject)
        {
            //ToDO: boxsize nciht von der Position abhängig machen -> Kracht beim Player, da sich die Pos stets ändert
            O_Parent = gameObject;
            center = new Vector3(gameObject.Position.X + 2, gameObject.Position.Y + 2, gameObject.Position.Z);
            iminX = (int) center.X - GameConstants.iBlockSize / 4;
            imaxX = (int) center.X + GameConstants.iBlockSize / 4;
            iminY = (int) center.Y - GameConstants.iBlockSize / 4;            
            imaxY = (int) center.Y + GameConstants.iBlockSize / 4;
            iminZ = (int) center.Z - GameConstants.iBlockSize ;
            imaxZ = (int) center.Z + GameConstants.iBlockSize ;
        }

        public bool intersect(MyBoundingBox other)
        {
            
           O_Parent.hasCollided = ((this.iminX <= other.imaxX && this.imaxX >= other.iminX) &&
                                      (this.iminY <= other.imaxY && this.imaxY >= other.iminY) &&
                                      (this.iminZ <= other.imaxZ && this.imaxZ >= other.iminZ));

            //Console.WriteLine("Collision: " + GameConstants.isCollided);

            return O_Parent.hasCollided;
        }

    }
}
