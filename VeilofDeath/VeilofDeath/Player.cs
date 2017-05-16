using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeilofDeath
{
    class Player : LivingEntity
    {
        public Model Model;
        public Matrix[] Transforms;

        //Position of the model in world space
        public Vector3 Position;
        //Velocity of the model, applied each frame to the model's position
        public Vector3 Velocity;

        public Quaternion Rotation;

        public Player()
        {

        }

        public void Spawn()
        {
            Position = new Vector3(0, 1, 1);
            Rotation = Quaternion.Identity;
            Velocity = Vector3.Zero;
            //TODO: Reset Level, all Buffs and Debuffs
        }

        public void Tick()
        {
            Move();
        }

        public void Move()
        {
            //TODO: implement Bewegung
        }

        public void DeSpawn()
        {
            //TODO: Model deaktivieren, nicht löschen, da öfter benötigt
        }
    }
}
