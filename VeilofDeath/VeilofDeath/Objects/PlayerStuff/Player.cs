using Animations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeilofDeath.Core;
using VeilofDeath.Level;
using VeilofDeath.Objects;
using VeilofDeath.Objects.Traps;

namespace VeilofDeath.Objects.PlayerStuff
{
     public class Player : AGameObject,ILivingEntity
    {

        #region member, variables

        //Velocity of the model, applied each frame to the model's position
        public Vector3 Velocity;
        public bool isActive = false;
        public float fSpeed = 0.1f * GameConstants.fMovingSpeed;

        public float fProgress;
        /// <summary>
        /// triggers the jumping animation
        /// </summary>
        public bool isJumping = false;

        private float speedtime = GameConstants.fSpeedTime;

        /// <summary>
        /// triggers the Sliding animation
        /// </summary>
        public bool isSliding = false;

        public bool isSlowed = false;

        // Scheitelpunkt-Form für Sprung-Kurve
        Vector2 S;
        float a;
        float afterJumpY;
        private bool isSpeeded;


        //public Quaternion Rotation;

        #endregion

        #region constructor, initialize

        /// <summary>
        /// Constructor, needs model
        /// </summary>
        /// <param name="m">model of the player</param>
        public Player(AnimatedModel m)
        {
            AniModel = m;
            Position = new Vector3(GameConstants.fLaneCenter, 0, -3f);
            //Console.WriteLine("Startposition: (" + this.Position.X + "/ " + this.Position.Y + "/" + this.Position.Z + ")");
            Initialize();
            this.name = "Player";
        }

        /// <summary>
        /// Initializes the bounding box of the player
        /// </summary>
        public void Initialize()
        {
            box = new MyBoundingBox(this);
        }

        #endregion

        #region spawn, despawn methods

        /// <summary>
        /// Spawns the player
        /// </summary>
        /// <param name="pos">position where to spawn</param>
        public void Spawn(Vector3 pos)
        {
            Position = pos;
            Velocity = Vector3.Zero;
            isActive = true;
        }

        /// <summary>
        /// Spawns the player
        /// </summary>
        public void Spawn()
        {
            Velocity = Vector3.Zero;
            isActive = true;
            Position = GameManager.Instance.StartPos;
        }

        /// <summary>
        /// Deactivates the player.
        /// Model is no longer drawn 
        /// </summary>
        public void DeSpawn()
        {
            isActive = false; //wird nciht mehr gedrawt
            Position = Vector3.Zero;
        }

        #endregion

        #region update, move

        /// <summary>
        /// Update method for the Player
        /// </summary>
        public void Tick(Map map)
        {
            Move(map);

            box.updatePlayer(this);
            //if (! isJumping)
            HandleCollision();

            AniModel.Position = this.Position;
        }

        /// <summary>
        /// Handles the different moving states like: run, jump, glide
        /// </summary>
        public void Move(Map map)
        {
            if (isSpeeded)
            {
                speedtime--;
                if (speedtime <= 0)
                    isSpeeded = false;
            }

            Velocity = Vector3.Zero;
            if (isJumping)
            {
                Jump();
            }

            if (CheckFrontIsWalkable(map))
                Velocity = isSlowed ? 
                            fSpeed / 5 * Vector3.Up : 
                           isSpeeded? 
                            fSpeed *1.5f *Vector3.Up :
                           fSpeed * Vector3.Up;

            Position += Velocity;

        }

        private bool CheckFrontIsWalkable(Map map)
        {
            int GridX = (int)(Position.X - (GameConstants.iBlockSize / 2)) / GameConstants.iBlockSize;
            int GridY = (int)(Position.Y - (GameConstants.iBlockSize / 2)) / GameConstants.iBlockSize;
            Vector2 GridPos = new Vector2(GridX, GridY);
            if (GameConstants.isDebugMode)
            {
                Console.WriteLine("GridPos:" + GridPos);
                Console.WriteLine("MapPos:" + map.map[GridX, GridY].position + " - " + map.map[GridX, GridY].isWalkable);
            }

            if (!map.map[GridX, GridY + 1].isWalkable)
            {
                if (GameConstants.isDebugMode)
                    Console.WriteLine("Front blocked");
                return false;
            }
            return true;
        }

        #endregion

        #region everything for setting up the jump and the jump itself

        /// <summary>
        /// performs the calculation oh the position during a jump
        /// </summary>
        public void Jump()
        {
            // Scheitelpunktform für Parabel 
            // using S.X like Position.Y because S is 2d and Position is 3D
            // same with S.Y and Position.Z

            Position.Z = a * ((Position.Y - S.X) * (Position.Y - S.X)) + S.Y;

            if (GameConstants.isDebugMode)
                Console.WriteLine("jumpheight: " + Position.Z);
            if (Position.Z < -1f)
            {
                isJumping = false;
                Position.Z = -1f;
                UnsetJCurve();

            }
        }

        /// <summary>
        /// Sets the parameters for the jumping curve to calculate the Player.Position
        /// </summary>
        /// <param name="jumpMid">S(x,y) - center point of the jump</param>
        /// <param name="m">the factor for parabel-function</param>
        /// <param name="endPos">position (y) where the jump ends</param>
        internal void SetJumpingCurve(Vector2 jumpMid, float m, float endPos)
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
            GameConstants.Landing.Play();
        }

        #endregion

        #region TrapsHandler, CollisionHandler

        /// <summary>
        /// handles collision with the traps
        /// </summary>
        protected void HandleCollision()
        {
            HandleSpikes();
            HandleSlowtraps();
            HandleCoins();
            HandleSpikeRolls();
            HandleSpeedtraps();
        }

        private void HandleCoins()
        {
            List<Coin> lc = GameManager.Instance.getCoinList();
            // in einer foreach kann ich nciht löschen
            // For schleife von hinten nach vorne, da dann selbst bei einer löschung der Index korrekt blebit
            for (int i = lc.Count-1 ; i >= 0; i--)
            {
                if (lc[i].Position.Y > (this.Position.Y + 2f * GameConstants.iBlockSize)
                    || lc[i].Position.Y < (this.Position.Y - 1.5* GameConstants.iBlockSize))
                {
                    continue;
                }

                if (this.box.intersect(lc[i].box))
                {
                    if (GameConstants.isDebugMode)
                        Console.WriteLine("Coin collected");
                    GameManager.Instance.Delete(lc[i]);
                    GameManager.Instance.iCoinScore[GameManager.Instance.Level]++;
                    GameManager.Instance.AddtoScore(GameConstants.ScorePerCoin  + 5 * (GameConstants.iDifficulty - 1));
                }
            }

        }


        /// <summary>
        /// handles collison with traps which lead to death
        /// </summary>
        private void HandleSpikes()
        {
            foreach (SpikeTrap trap in GameManager.Instance.getSpikeList())
            {

                if (trap.Position.Y > (this.Position.Y + 2 * GameConstants.iBlockSize)
                    || trap.Position.Y < (this.Position.Y - GameConstants.iBlockSize))
                {
                    continue;
                }


                if (this.box.intersect(trap.box))
                {
                    //GameConstants.currentGame.Exit();
                    if (GameConstants.isDebugMode)
                    {
                        Console.WriteLine("Collision");
                        Console.WriteLine("player: " + this.box.iminZ
                                          + " box: " + trap.box.imaxZ);
                    }
                    this.isDead = true;
                }
            }
        }

        private void HandleSpikeRolls()
        {
            foreach (SpikeRoll trap in GameManager.Instance.getRollList())
            {

                if (trap.Position.Y > (this.Position.Y + 2 * GameConstants.iBlockSize)
                    || trap.Position.Y < (this.Position.Y - GameConstants.iBlockSize))
                {
                    continue;
                }


                if (this.box.intersect(trap.box))
                {
                    //GameConstants.currentGame.Exit();
                    if (GameConstants.isDebugMode)
                    {
                        Console.WriteLine("Collision");
                        Console.WriteLine("player: " + this.box.iminZ
                                          + " box: " + trap.box.imaxZ);
                    }
                    this.isDead = true;

                    //TODO: insert Sound "Zerquetschen oder Aufspießen" hier
                }
            }
        }

        /// <summary>
        /// handles collision which lead to slow effects
        /// </summary>
        private void HandleSlowtraps()
        {
            isSlowed = false;

            foreach (SlowTrap slow in GameManager.Instance.getSlowList())
            {
                if (slow.Position.Y > (this.Position.Y + 2 * GameConstants.iBlockSize)
                    || slow.Position.Y < (this.Position.Y - GameConstants.iBlockSize))
                {
                    continue;
                }

                if (this.box.intersect(slow.box))
                {
                    if (GameConstants.isDebugMode)
                        Console.WriteLine("Slowdown");
                    this.isSlowed = true;
                    GameManager.Instance.GUIFX.SpawnSlow();
                }

            }
        }

        private void HandleSpeedtraps()
        {
            List<SpeedTrap> lc = GameManager.Instance.getSpeedList();
            // in einer foreach kann ich nciht löschen
            // For schleife von hinten nach vorne, da dann selbst bei einer löschung der Index korrekt blebit
            for (int i = lc.Count - 1; i >= 0; i--)
            {
                if (lc[i].Position.Y > (this.Position.Y + 2f * GameConstants.iBlockSize)
                    || lc[i].Position.Y < (this.Position.Y - 1.5 * GameConstants.iBlockSize))
                {
                    continue;
                }

                if (this.box.intersect(lc[i].box))
                {
                    //if (GameConstants.isDebugMode)
                        Console.WriteLine("SpeedBuff");
                    GameManager.Instance.DeleteBuff(lc[i]);
                    this.isSpeeded = true;
                    speedtime = GameConstants.fSpeedTime;
                    GameManager.Instance.GUIFX.SpawnSpeed();
                    GameManager.Instance.GUIFX.SpawnSpeed();
                    GameManager.Instance.GUIFX.SpawnSpeed();
                    GameManager.Instance.GUIFX.SpawnSpeed();
                }
            }
        }

        #endregion

        private float calculateFactor(Vector2 mid, float height)
        {
            Vector2 landing = new Vector2(height, 0);
            return (landing.Y - mid.Y) / (landing.X - mid.X);
        }

        /// <summary>
        /// Draws the player if active
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            if (!isActive)
                return;

            //foreach (ModelMesh mesh in this.model.Meshes)
            //{
            //    foreach (BasicEffect effect in mesh.Effects)
            //    {
            //        effect.EnableDefaultLighting();

            //        effect.DirectionalLight0.DiffuseColor = new Vector3(0.5f, 0, 0); // a red light
            //        effect.DirectionalLight0.Direction = new Vector3(-1, 0, -1);  // coming along the x-axis
            //        effect.DirectionalLight0.SpecularColor = new Vector3(0, 1, 0); // with green highlights

            //        effect.AmbientLightColor = new Vector3(0.01f, 0.15f, 0.6f);
            //        effect.EmissiveColor = new Vector3(0f, 0.1f, 0.2f);


            //        effect.World = GameConstants.MainCam.X_World * Matrix.CreateTranslation(this.Position);
            //        effect.View = GameConstants.MainCam.X_View;
            //        effect.Projection = GameConstants.MainCam.X_Projection;
            //    }

            //    mesh.Draw();
            //}

           base.Draw(gameTime);
        }
    }
}
