using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace VeilofDeath.Objects.Traps
{
    public class SpikeTrap : AGameObject, StaticEntity
    {
        public MyBoundingBox box;

        public SpikeTrap(Vector3 pos)
        {
            Position = pos;
            Initialize();
        }

        public void DeSpawn()
        {
            throw new NotImplementedException();
        }

        public void Initialize()
        {
            box = new MyBoundingBox(this);
        }

        public void Interact()
        {
            throw new NotImplementedException();
        }

        public void Spawn(Vector3 pos)
        {
            throw new NotImplementedException();
        }

        public void Tick()
        {
            throw new NotImplementedException();
        }
    }
}
