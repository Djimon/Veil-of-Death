using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeilofDeath
{
    interface LivingEntity
    {
        /// <summary>
        /// instantiates the creature in the world
        /// </summary>
        void Spawn();

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
