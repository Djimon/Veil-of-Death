using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeilofDeath
{
    public abstract class AGameObject
    {
        public Model model;
        public float angle;

        /// <summary>
        /// Primary collision
        /// </summary>
        protected void HandleCollision()
        {

        }

        /// <summary>
        /// Method to delete an object fully with all its dependent effects
        /// </summary>
        protected void Kill()
        {
            //Destroy GameObject and all it's Effects and Dependencies
        }

        public void Draw(Camera cam)
        {
            foreach (var mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;

                    effect.World = GetWorldMatrix(); 
                    effect.View = cam.ViewMatrix;
                    effect.Projection = cam.SetProjectionsMatrix();
                }

                mesh.Draw();
            }
        }

        /// <summary>
        /// generates world matrix for this object
        /// </summary>
        /// <returns>WolrdMatrix</returns>
        Matrix GetWorldMatrix()
        {
            const float circleRadius = 8;
            const float heightOffGround = 3;

            // this matrix moves the model "out" from the origin
            Matrix translationMatrix = Matrix.CreateTranslation(
                circleRadius, 0, heightOffGround);

            // this matrix rotates everything around the origin
            Matrix rotationMatrix = Matrix.CreateRotationZ(angle);

            // We combine the two to have the model move in a circle:
            Matrix combined = translationMatrix * rotationMatrix;

            return combined;
        }

    }
}
