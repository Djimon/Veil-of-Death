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
        public bool isActive = false;
        public float fSpeed = 0.1f;

        //public Quaternion Rotation;

        /// <summary>
        /// Constructor, needs model
        /// </summary>
        /// <param name="m">model of the player</param>
        public Player(Model m)
        {
            model = m;
            Position = new Vector3(GameConstants.fLaneCenter, 0,0);
            Initialize();            
        }

        public void Initialize()
        {
                        
        }

        /// <summary>
        /// Spawns the player
        /// </summary>
        /// <param name="pos">position where to spawn</param>
        public void Spawn(Vector3 pos)
        {
            Position = pos;
            //Rotation = Quaternion.Identity;
            Velocity = Vector3.Zero;
            //TODO: Reset Level, all Buffs and Debuffs
            isActive = true;
        }

        /// <summary>
        /// Spawns the player
        /// </summary>
        public void Spawn()
        {
            //Rotation = Quaternion.Identity;
            Velocity = Vector3.Zero;
            //TODO: Reset Level, all Buffs and Debuffs
            isActive = true;
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
            Velocity = fSpeed * Vector3.Up;
            Position += Velocity;
        }

        /// <summary>
        /// Draws the player if active
        /// </summary>
        public new void Draw()
        {
            if (!isActive)
                return;

            foreach (ModelMesh mesh in this.model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    effect.DirectionalLight0.DiffuseColor = new Vector3(0.5f, 0, 0); // a red light
                    effect.DirectionalLight0.Direction = new Vector3(-1, 0, -1);  // coming along the x-axis
                    effect.DirectionalLight0.SpecularColor = new Vector3(0, 1, 0); // with green highlights

                    effect.AmbientLightColor = new Vector3(0.01f, 0.15f, 0.6f);
                    effect.EmissiveColor = new Vector3(0f, 0.1f, 0.2f);

                    effect.World = GameConstants.MainCam.X_World * Matrix.CreateTranslation(this.Position);
                    effect.View = GameConstants.MainCam.X_View;
                    effect.Projection = GameConstants.MainCam.X_Projection;
                }

                mesh.Draw();
            }
        }


        /// <summary>
        /// Deactivates the player.
        /// Model is no longer drawn 
        /// </summary>
        public void DeSpawn()
        {
            //TODO: Model deaktivieren, nicht löschen, da öfter benötigt
            isActive = false; //wird nciht mehr gedrawt
            Position = Vector3.Zero;
        }

    }
}
