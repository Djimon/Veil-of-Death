using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using VeilofDeath.Level;
using Microsoft.Xna.Framework.Graphics;
using VeilofDeath.Core;

namespace VeilofDeath.Objects.Traps
{
    class SpeedTrap : AGameObject, IStaticEntity
    {
        private Vector3 rotationAxis = new Vector3(0, 0, 1);
        public bool isActive = true;
        Effect effect;

        public SpeedTrap(Vector3 pos)
        {
            Position = pos ;

            Initialize();
        }

        public void DeSpawn()
        {
            box = null;
        }

        public void Initialize()
        {
            model = GameConstants.Content.Load<Model>("Models/cube");
            box = new MyBoundingBox(this);
            effect = GameConstants.Content.Load<Effect>("Shader/Transparency");
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

            //DrawModelWithEffect(model, effect);

            foreach (var mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.AmbientLightColor = new Vector3(0.5f, 0.5f, 0.5f);
                    effect.EmissiveColor = new Vector3(0.15f, 0.15f, 0.0f);

                    effect.World = GameConstants.MainCam.X_World *
                                   Matrix.CreateScale(fModelScale) *
                                   (Matrix.CreateFromAxisAngle(rotationAxis, GameConstants.rotation) *
                                   Matrix.CreateTranslation(this.Position));
                    effect.View = GameConstants.MainCam.X_View;
                    effect.Projection = GameConstants.MainCam.X_Projection;
                }

                mesh.Draw();
            }
        }
        private void DrawModelWithEffect(Model model, Effect eff)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = eff;
                    eff.CurrentTechnique =  eff.Techniques["Transparent"];
                    eff.Parameters["World"].SetValue(GameConstants.MainCam.X_World *
                                   Matrix.CreateScale(fModelScale) *
                                   (Matrix.CreateFromAxisAngle(rotationAxis, GameConstants.rotation) *
                                   Matrix.CreateTranslation(this.Position)) * mesh.ParentBone.Transform);
                    eff.Parameters["View"].SetValue(GameConstants.MainCam.X_View);
                    eff.Parameters["Projection"].SetValue(GameConstants.MainCam.X_Projection);
                }
                mesh.Draw();
            }
        }

    }
}
