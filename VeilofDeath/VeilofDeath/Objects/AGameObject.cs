using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeilofDeath.Objects;

namespace VeilofDeath
{
    public abstract class AGameObject
    {
        public Model model;
        public float angle;
        public Vector3 Position;
        public MyBoundingBox box;
        public bool hasCollided = false;
        public string name="";
        /// <summary>
        /// Primary collision
        /// </summary>
        protected void HandleCollision()
        {
            foreach (SpikeTrap trap in GameManager.Instance.SpikeList)
            {
                if (this.box.intersect(trap.box))
                    //GameConstants.currentGame.Exit();
                    Console.WriteLine("Collision");
            }

        }

        /// <summary>
        /// Method to delete an object fully with all its dependent effects
        /// </summary>
        protected void Kill()
        {
            //Destroy GameObject and all it's Effects and Dependencies
        }

        public void Draw()
        {
            //TODO: Überprüfen, ob so wie in Player auch für normale GameObjects geeignet?
            foreach (var mesh in model.Meshes)
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

        

    }
}
