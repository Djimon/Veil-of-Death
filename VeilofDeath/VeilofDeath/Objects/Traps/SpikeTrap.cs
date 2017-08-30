using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using VeilofDeath.Objects;
using VeilofDeath.Level;
using VeilofDeath.Core;

namespace VeilofDeath.Objects.Traps
{
    public class SpikeTrap : AGameObject, IStaticEntity
    {
        public bool isFloating;
        public float fStartFloating;
        private Block identity;

        public SpikeTrap(Vector3 pos, Block self)
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
            fModelScale = 2;
            model = GameConstants.levelDictionary["stachelfalle"];
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

        public void Tick( GameTime time)
        {
            
            if (identity == null)
            {
                DeSpawn();
            }
        }

        private void DoFloat()
        {
            
        }
    }
}
