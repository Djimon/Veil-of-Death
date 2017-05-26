using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeilofDeath
{
    interface ILivingEntity
    {
        /// <summary>
        /// instantiates the creature in the world
        /// </summary>
        void Spawn(Vector3 pos);

        void Initilize(Model model);

        /// <summary>
        /// Behaves simmilar to an update function
        /// </summary>
        void Tick();

        /// <summary>
        /// Basic movement handling
        /// </summary>
        void Move();

        /// <summary>
        /// Deletes the creature in the world
        /// </summary>
        void DeSpawn();
    }
}
