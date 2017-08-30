using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VeilofDeath.Level;
using VeilofDeath.Core;

namespace VeilofDeath.Objects
{
    class SpikeRoll : AGameObject, IStaticEntity
    {
        public bool isActive = true;
        private Vector3 rotationAxis = new Vector3(0, 0, 1);

        private float fAngle;
        private float fSpeed = 0.1f;
        private bool moveright = true;
        Random rnd;

        public SpikeRoll(Vector3 pos)
        {
            Position = pos;
            Initialize();
        }

        public void DeSpawn()
        {
            throw new NotImplementedException();
        }

        public void Initialize()
        {
            model = GameConstants.Content.Load<Model>("Models/spikeroll");
            box = new MyBoundingBox(this);
            rnd = new Random();
            fSpeed = Math.Min(0.08f *GameConstants.iDifficulty ,Math.Max((float) rnd.NextDouble(),0.04f));
        }

        public void Interact()
        {
            throw new NotImplementedException();
        }

        public void Spawn(Vector3 pos)
        {
            throw new NotImplementedException();
        }

        public void Update(Map map)
        {
            fAngle++;
            Move(map);
            box.update(this);
        }

        private void Move(Map map)
        {
            Vector2 GridPos = new Vector2((int)(Position.X - (GameConstants.iBlockSize / 2)) / GameConstants.iBlockSize,
                                          (int)(Position.Y - (GameConstants.iBlockSize / 2)) / GameConstants.iBlockSize);

            if (map.map[(int)GridPos.X + 1, (int)GridPos.Y].isWalkable
                && (!map.map[(int)GridPos.X - 1, (int)GridPos.Y].isWalkable))
                moveright = true;
            else if (!map.map[(int)GridPos.X + 1, (int)GridPos.Y].isWalkable
                && (map.map[(int)GridPos.X - 1, (int)GridPos.Y].isWalkable))
                moveright = false;


            if (moveright && map.map[(int)GridPos.X + 1, (int)GridPos.Y].isWalkable)
            {
                //right
                Position.X += fSpeed;
            }
            if (!moveright && map.map[(int)GridPos.X - 1, (int)GridPos.Y].isWalkable)
            {
                //left
                Position.X -= fSpeed; 
            }
        }

        public override void Draw()
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

        public void Tick()
        {
            throw new NotImplementedException();
        }
    }
}
