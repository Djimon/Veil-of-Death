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

        private AGameObject O_Parent;

        public MyBoundingBox(AGameObject gameObject)
        {
            //ToDO: size nciht von der Position abhängig machen -> Kracht beim Player, da sich die Pos stets ändert
            O_Parent = gameObject;
            iminX = (int)gameObject.Position.X - GameConstants.iBlockSize / 2;
            iminY = (int)gameObject.Position.Y - GameConstants.iBlockSize * 2;
            iminZ = (int)gameObject.Position.Z - GameConstants.iBlockSize * 4;
            imaxX = (int)gameObject.Position.X + GameConstants.iBlockSize / 2;
            imaxY = (int)gameObject.Position.Y + GameConstants.iBlockSize * 2;
            imaxZ = (int)gameObject.Position.Z + GameConstants.iBlockSize * 4;
        }

        public bool intersect(MyBoundingBox other)
        {
            //TODO: attach "isCollided" to Player not global
           O_Parent.hasCollided = ((this.iminX <= other.imaxX && this.imaxX >= other.iminX) &&
                                      (this.iminY <= other.imaxY && this.imaxY >= other.iminY) &&
                                      (this.iminZ <= other.imaxZ && this.imaxZ >= other.iminZ));

            //Console.WriteLine("Collision: " + GameConstants.isCollided);

            return O_Parent.hasCollided;
        }

    }
}
