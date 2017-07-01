using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeilofDeath.Objects;

namespace VeilofDeath
{
     public class Player : AGameObject,ILivingEntity
    {
        //Velocity of the model, applied each frame to the model's position
        public Vector3 Velocity;
        public bool isActive = false;
        public float fSpeed = 0.1f * GameConstants.fMovingSpeed;        

        /// <summary>
        /// triggers the jumping animation
        /// </summary>
        public bool isJumping = false;
        //TODO: Animationszeit = GameCOnstants.fJumpingWidth / GameConstants.fJumpSpeed (für Jump und Slide gleich)

        /// <summary>
        /// triggers the Sliding animation
        /// </summary>
        public bool isSliding = false;


        // Scheitelpunkt-Form für Sprung-Kurve
        Vector2 S;
        float a;
        float afterJumpY;
       

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
            box = new MyBoundingBox(this);
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
            box.update(this);
            Move();
            HandleCollision();

        }

        /// <summary>
        /// Handles the different moving states like: run, jump, glide
        /// </summary>
        public void Move()
        {

            if (isJumping)
            {
                Jump();
            }

            Velocity = fSpeed * Vector3.Up;
            Position += Velocity;

        }

        /// <summary>
        /// performs the calculation oh the position during a jump
        /// </summary>
        public void Jump()
        {
            // Scheitelpunktform für Parabel 
            // using S.X like Position.Y because S is 2d and Position is 3D
            // same with S.Y and Position.Z


            //TODO: fix quadratical Jumping
            
            Position.Z = a * ( (Position.Y - S.X)*(Position.Y - S.X))   + S.Y;

            if (GameConstants.isDebugMode)
                Console.WriteLine("jumpheight: " + Position.Z);
            if (Position.Z < 0 )
            {
                isJumping = false;
                Position.Z = 0;
                UnsetJCurve();
                
            }
        }

        /// <summary>
        /// Sets the parameters for the jumping curve to calculate the Player.Position
        /// </summary>
        /// <param name="jumpMid">S(x,y) - center point of the jump</param>
        /// <param name="m">the factor for parabel-function</param>
        /// <param name="endPos">position (y) where the jump ends</param>
        internal void SetJumpingCurve(Vector2 jumpMid,float m, float endPos)
        {
            S = jumpMid;
            a = m;

            afterJumpY = endPos;
            if (GameConstants.isDebugMode)
                Console.WriteLine("S(" + S.X + "/" + S.Y + ") , a = " + a);
        }

        /// <summary>
        /// Resets the jumping curve parameters
        /// </summary>
        public void UnsetJCurve()
        {
            S = Vector2.Zero;
            a = 0;
            afterJumpY = 0;
        }

        private float calculateFactor(Vector2 mid, float height)
        {
            Vector2 landing = new Vector2(height, 0);
            return (landing.Y - mid.Y) / (landing.X - mid.X);
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
