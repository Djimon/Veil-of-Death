using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Animations;
using Microsoft.Xna.Framework.Media;

namespace VeilofDeath.Objects
{
    class Coin : AGameObject, IStaticEntity
    {
        public bool isActive = true;

        private Vector3 rotationAxis = new Vector3(0,0,1);

        /// <summary>
        /// CONSTRUCTOR
        /// </summary>
        /// <param name="pos">Position of the coin</param>
        public Coin(Vector3 pos)
        {
            Position = pos;
            Initialize();

        }

        public void DeSpawn()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// sets model and bounding box
        /// </summary>
        public void Initialize()
        {
            model = GameConstants.Content.Load<Model>("Models/coin");
            box = new MyBoundingBox(this);
        }

        public void Interact()
        {
            throw new NotImplementedException();
        }

        public void Spawn(Vector3 pos)
        {
            throw new NotImplementedException();
        }

        public void Tick()
        {           
            
        }

        public override void Draw()
        {
            //TODO: handle Drehung auf Model
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

                    effect.World = GameConstants.MainCam.X_World * Matrix.CreateScale(fModelScale) * (Matrix.CreateFromAxisAngle(rotationAxis, GameConstants.rotation) * Matrix.CreateTranslation(Position));
                    effect.View = GameConstants.MainCam.X_View;
                    effect.Projection = GameConstants.MainCam.X_Projection;
                }

                mesh.Draw();
            }
        }
    }
}
