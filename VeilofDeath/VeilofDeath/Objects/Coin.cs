using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Animations;

namespace VeilofDeath.Objects
{
    class Coin : AGameObject, IStaticEntity
    {
        public bool isActive = true;

        /// <summary>
        /// CONSTRUCTOR
        /// </summary>
        /// <param name="pos">Position of the coin</param>
        public Coin(Vector3 pos)
        {
            Position = pos;
            Initialize();

        }

        public void DeSpawn()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// sets model and bounding box
        /// </summary>
        public void Initialize()
        {
            model = GameConstants.Content.Load<Model>("Models/coin");
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
            
        }

        public override void Draw()
        {
            //TODO: handle Drehung auf Model
           
            base.Draw();
        }
    }
}
