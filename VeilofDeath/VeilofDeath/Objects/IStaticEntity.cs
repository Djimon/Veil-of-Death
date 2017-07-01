using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeilofDeath.Objects
{
    interface IStaticEntity
    {

        void Spawn(Vector3 pos);

        /// <summary>
        /// Initializes the entity with the given model
        /// </summary>
        /// <param name="model">Model for the entity</param>
        void Initialize();

        /// <summary>
        /// Behaves simmilar to an update function
        /// </summary>
        void Tick();

        /// <summary>
        /// Basic Interaction
        /// </summary>
        void Interact();

        /// <summary>
        /// Deletes the creature in the world
        /// </summary>
        void DeSpawn();

    }
}
