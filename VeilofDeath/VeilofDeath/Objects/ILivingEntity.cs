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

        /// <summary>
        /// Initializes the entity with the given model
        /// </summary>
        /// <param name="model">Model for the entity</param>
        void Initialize();

        /// <summary>
        /// Behaves simmilar to an update function
        /// </summary>
        void Tick(Map map);

        /// <summary>
        /// Basic movement handling
        /// </summary>
        void Move(Map map);

        /// <summary>
        /// Deletes the creature in the world
        /// </summary>
        void DeSpawn();
    }
}
