using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using VeilofDeath.Level;

namespace VeilofDeath.Objects.Traps
{
    class SlowTrap : AGameObject, IStaticEntity
    {
        private Block identity;

        public SlowTrap(Vector3 pos, Block self)
        {
            Position = pos;
            identity = self;
            Initialize();
        }

        public void DeSpawn()
        {
            box = null;
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
            if (identity == null)
            {
                DeSpawn();
            }

        }
    }
}
