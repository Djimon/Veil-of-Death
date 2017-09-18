using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeilofDeath.Core;

namespace VeilofDeath.Objects
{
   public class MyBoundingBox
    {
        private float iminX;
        private float iminY;
        public float iminZ;
        private float imaxX;
        private float imaxY;
        public float imaxZ;


        private AGameObject O_Parent;

        /// <summary>
        /// set up the bounding box for a object
        /// </summary>
        /// <param name="gameObject">object on which the box is constructed</param>
        public MyBoundingBox(AGameObject gameObject)
        {
            O_Parent = gameObject;
            iminX = gameObject.Position.X - GameConstants.iBlockSize / 4;
            iminY = gameObject.Position.Y - GameConstants.iBlockSize / 4;
            iminZ = gameObject.Position.Z;
            imaxX = gameObject.Position.X + GameConstants.iBlockSize / 4;
            imaxY = gameObject.Position.Y + GameConstants.iBlockSize / 4;
            imaxZ = gameObject.Position.Z + GameConstants.iBlockSize;

        }
        public MyBoundingBox(AGameObject gameObject, float factor)
        {
            O_Parent = gameObject;
            iminX = gameObject.Position.X - factor * GameConstants.iBlockSize / 4;
            iminY = gameObject.Position.Y - factor * GameConstants.iBlockSize / 4;
            iminZ = gameObject.Position.Z;
            imaxX = gameObject.Position.X + factor * GameConstants.iBlockSize / 4;
            imaxY = gameObject.Position.Y + factor * GameConstants.iBlockSize / 4;
            imaxZ = gameObject.Position.Z + GameConstants.iBlockSize;

        }

        public MyBoundingBox(AGameObject gameObject, float factor, float Z)
        {
            O_Parent = gameObject;
            iminX = gameObject.Position.X - factor * GameConstants.iBlockSize / 4;
            iminY = gameObject.Position.Y - factor * GameConstants.iBlockSize / 4;
            iminZ = gameObject.Position.Z;
            imaxX = gameObject.Position.X + factor * GameConstants.iBlockSize / 4;
            imaxY = gameObject.Position.Y + factor * GameConstants.iBlockSize / 4;
            imaxZ = gameObject.Position.Z + Z * GameConstants.iBlockSize;

        }

        /*
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
        */

        /// <summary>
        /// calculates intersection of two objects
        /// </summary>
        /// <param name="other">other object which should be proved on intersection</param>
        /// <returns></returns>
        public bool intersect(MyBoundingBox other)
        {
            
           O_Parent.hasCollided = ((this.iminX <= other.imaxX && this.imaxX >= other.iminX) &&
                                      (this.iminY <= other.imaxY && this.imaxY >= other.iminY) &&
                                      (this.iminZ <= other.imaxZ && this.imaxZ >= other.iminZ));

            /*
            Console.WriteLine("Collision: " + GameConstants.isCollided)
            Console.WriteLine("Player x: " + iminX + " y: " + iminY + " z: " + iminZ);
            Console.WriteLine("Trap x: " + other.imaxX + " y: " + other.imaxY + " z: " + other.imaxZ);
            */

            return O_Parent.hasCollided;
        }


        public void updatePlayer(AGameObject gameObject)
        {
            //TODO: drin lassen->  trotzdem constructor 2 mit factor = 0.85f
            iminX = gameObject.Position.X + 0.2f - GameConstants.iBlockSize / 4;
            iminY = gameObject.Position.Y + 0.2f - GameConstants.iBlockSize / 4;
            iminZ = gameObject.Position.Z;
            imaxX = gameObject.Position.X + 0.2f + GameConstants.iBlockSize / 4;
            imaxY = gameObject.Position.Y + 0.2f + GameConstants.iBlockSize / 4;
            imaxZ = gameObject.Position.Z + GameConstants.iBlockSize;
        }

        /// <summary>
        /// updates bounding box of the object
        /// </summary>
        /// <param name="gameObject">object which bounding box should be updated</param>
        public void updateSpikeTrap(AGameObject gameObject)
        {
            //TODO: is fest -> constructor 2 mit factor = 0.85f
            iminX = gameObject.Position.X - 0.85f * GameConstants.iBlockSize / 4;
            iminY = gameObject.Position.Y - 0.85f * GameConstants.iBlockSize / 4;
            iminZ = gameObject.Position.Z ;
            imaxX = gameObject.Position.X + 0.85f * GameConstants.iBlockSize / 4;
            imaxY = gameObject.Position.Y + 0.85f * GameConstants.iBlockSize / 4;
            imaxZ = gameObject.Position.Z + GameConstants.iBlockSize;
        }

        public void updateSlide(AGameObject gameObject)
        {
            //TODO: ist fest -> nutze Konstruktur 3 mit (factor = 1 und Z = 0.5)
            iminX = gameObject.Position.X - GameConstants.iBlockSize / 4;
            iminY = gameObject.Position.Y - GameConstants.iBlockSize / 4;
            iminZ = gameObject.Position.Z;
            imaxX = gameObject.Position.X + GameConstants.iBlockSize / 4;
            imaxY = gameObject.Position.Y + GameConstants.iBlockSize / 4;
            imaxZ = gameObject.Position.Z + 0.5f * GameConstants.iBlockSize ;
        }

        public void updateSpikeRoll(AGameObject gameObject)
        {
            //TODO: drin lassen->  trotzdem constructor 3 mit factor = 0.9f, Z = 3
            iminX = gameObject.Position.X - 0.9f * GameConstants.iBlockSize / 4;
            iminY = gameObject.Position.Y - 0.9f * GameConstants.iBlockSize / 4;
            iminZ = gameObject.Position.Z;
            imaxX = gameObject.Position.X + 0.9f * GameConstants.iBlockSize / 4;
            imaxY = gameObject.Position.Y + 0.9f * GameConstants.iBlockSize / 4;
            imaxZ = gameObject.Position.Z + 3 * GameConstants.iBlockSize;
        }


    }
}
