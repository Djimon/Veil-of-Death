using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using VeilofDeath.Core;
using Microsoft.Xna.Framework.Graphics;

namespace VeilofDeath.Objects.Traps
{
    class BrettTrap : AGameObject, IStaticEntity
    {
        public bool isActive = true;
        public MyBoundingBox SwordBox;

        public BrettTrap(Vector3 pos)
        {
            Position = pos;
            Initialize();
        }

        public void Spawn(Vector3 pos)
        {
            throw new NotImplementedException();
        }

        public void Initialize()
        {
            //model = GameConstants.Content.Load<Model>("Models/holzwand");
            box = new MyBoundingBox(this, 1f, 3f);
            //SwordBox = new MyBoundingBox(this);
        }

        public void Tick()
        {
            throw new NotImplementedException();
        }

        public void Interact()
        {
            throw new NotImplementedException();
        }

        public void DeSpawn()
        {
            throw new NotImplementedException();
        }
    }
}
