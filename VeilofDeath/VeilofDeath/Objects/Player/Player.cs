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
     public class Player : AGameObject,ILivingEntity
    {
        //Position of the model in world space
        public Vector3 Position;
        //Velocity of the model, applied each frame to the model's position
        public Vector3 Velocity;

        //public Quaternion Rotation;

        public Player(Model m)
        {
            model = m;
            Position = Vector3.Zero;
        }

        public void Initialize(Model m)
        {
            model = m;            
        }

        public void Spawn(Vector3 pos)
        {
            Position = pos;
            //Rotation = Quaternion.Identity;
            Velocity = Vector3.Zero;
            //TODO: Reset Level, all Buffs and Debuffs
        }

        /// <summary>
        /// Update method for the Player
        /// </summary>
        public void Tick()
        {
            
            Move();
        }

        /// <summary>
        /// Handles the different moving states like: run, jump, glide
        /// </summary>
        public void Move()
        {
            //TODO: implement Bewegung
        }

        /// <summary>
        /// Draws the Player and set the viewmatrix of the camera
        /// </summary>
        /// <param name="c">main camera</param>
        public new void Draw(Camera c)
        {
     
            c.ViewMatrix = c.SetView(this);
            base.Draw(c);

        }

        public void DeSpawn()
        {
            //TODO: Model deaktivieren, nicht löschen, da öfter benötigt
        }

    }
}
