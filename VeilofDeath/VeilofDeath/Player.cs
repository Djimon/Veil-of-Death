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
    class Player : AGameObject,ILivingEntity
    {

        public Matrix[] x_Transforms;

        //Position of the model in world space
        public Vector3 Position;
        //Velocity of the model, applied each frame to the model's position
        public Vector3 Velocity;

        public Quaternion Rotation;

        public Player()
        {

        }

        public void Initilize(Model m)
        {
            model = m;            
        }

        public void Spawn(Vector3 pos)
        {
            Position = pos;
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
