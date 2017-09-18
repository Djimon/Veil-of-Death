using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Animations;
using VeilofDeath.Objects;
using VeilofDeath.Objects.Traps;
using VeilofDeath.Core;

namespace VeilofDeath.Objects
{
    public abstract class AGameObject
    {
        public AnimatedModel AniModel;
        public Model model;
        public float angle;
        public Vector3 Position;
        public MyBoundingBox box;
        public bool hasCollided = false;
        public bool isDead = false;

        public float fModelScale = 1;

        public string name="";

        /// <summary>
        /// Method to delete an object fully with all its dependent effects
        /// </summary>
        protected void Kill()
        {
            //Destroy GameObject and all it's Effects and Dependencies
        }


        /// <summary>
        /// Draw Method for AnimatedModels
        /// </summary>
        /// <param name="gameTime">important for the drawing because it depends on it</param>
        public virtual void Draw(GameTime gameTime)
        {
            AniModel.Draw(GameConstants.MainCam.X_View, GameConstants.MainCam.X_Projection);
        }

        /// <summary>
        /// Draw Method for normal Models
        /// </summary>
        public virtual void Draw()
        {
            foreach (var mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    effect.DirectionalLight0.DiffuseColor = new Vector3(1, 1, 1); // a red light
                    effect.DirectionalLight0.Direction = new Vector3(-1, 0, -1);  // coming along the x-axis
                    effect.DirectionalLight0.SpecularColor = new Vector3(0.2f, 0.2f, 0.2f); // with green highlights

                    effect.AmbientLightColor = new Vector3(0f, 0f, 0f);
                    effect.EmissiveColor = new Vector3(0.15f, 0.15f, 0.0f);

                    effect.World = GameConstants.MainCam.X_World * Matrix.CreateScale(fModelScale) * Matrix.CreateTranslation(this.Position);
                    effect.View = GameConstants.MainCam.X_View;
                    effect.Projection = GameConstants.MainCam.X_Projection;
                }

                mesh.Draw();
            }
        }
    }
}
