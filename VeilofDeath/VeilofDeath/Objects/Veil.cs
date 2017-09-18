using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeilofDeath.SpecialFX;
using VeilofDeath.Objects.PlayerStuff;
using VeilofDeath.Core;

namespace VeilofDeath.Objects
{
    class Veil
    {
        public Vector3 Position { get; private set; }        
        public float fSpeed;
        public bool hasReachedPlayer = false;

        private Player  Victim;
        private Vector3 Velocity;
        public float fDistance { get; private set; }
        private float fStartDistance;

        private Texture2D Top, Left, Right, Bottom;
        private AParticleEnginge particleEngine;

        public Veil(int toughness, Player victim, AParticleEnginge PE)
        {
            Initialize(toughness, victim, PE);
            LoadContent();
        }

        private void LoadContent()
        {
            Top = GameConstants.Content.Load<Texture2D>("Textures/TopShrink");
            Left = GameConstants.Content.Load<Texture2D>("Textures/LeftShrink");
            Right = GameConstants.Content.Load<Texture2D>("Textures/RightShrink");
            Bottom = GameConstants.Content.Load<Texture2D>("Textures/DownShrink");
        }

        private void Initialize(int toughness, Player victim, AParticleEnginge PE)
        {
            Position = new Vector3(GameManager.Instance.StartPos.X,
                                                (GameManager.Instance.Level > 0 ?
                                                    GameManager.Instance.StartPos.Y - ((180 - 12 * toughness) + GameManager.Instance.fVeilDistance)/2 :
                                                    GameManager.Instance.StartPos.Y - (180 - 12 * toughness)),
                                                0);
            //float temp = GameConstants.fMovingSpeed + (0.25f* (float)Math.Log(0.3f * 1));
            //float temp2 = GameConstants.fMovingSpeed + (0.25f * (float)Math.Log(0.3f * 3));
            //float temp3 = GameConstants.fMovingSpeed + (0.25f * (float)Math.Log(0.3f * 5));
            //Console.WriteLine("Log_test: "+temp+","+temp2+","+temp3 );
            fSpeed = Math.Max(0.1f * GameConstants.fMovingSpeed, 0.1f * (GameConstants.fMovingSpeed + (0.25f * (float)Math.Log(0.1f* GameConstants.iDifficulty * 5))));
            Victim = victim;
            if (GameConstants.isDebugMode)
                Console.WriteLine("PlayerSpeed: "+ GameConstants.fMovingSpeed + "  VeilSpeed: " + fSpeed);
            fStartDistance = fDistance = Victim.Position.Y - Position.Y;
            if (GameConstants.isDebugMode)
                Console.WriteLine("Distance: " + fDistance);
            particleEngine = PE;
        }

        public void Update(GameTime time)
        {
            Velocity = fSpeed * Vector3.Up;
            Position += Velocity;            
            fDistance = Victim.Position.Y - Position.Y;

            if (fDistance <= 0)
            {
                hasReachedPlayer = true;
                fDistance = 0;
            }
                

            //Console.WriteLine("V:" + Position.Y + "  P:" + Victim.Position.Y + " = " + fDistance);


            if (time.TotalGameTime.Seconds % 2 == 0 && GameConstants.isDebugMode)
                Console.WriteLine(GameManager.Instance.iPhase);

            HandleCurse();

            if (GameManager.Instance.iPhase > 0 )
                particleEngine.Update(time);
        }

 

        private void HandleCurse()
        {
            float fPercentage = fDistance / fStartDistance;
        

            if (fPercentage <= 0.90f && fPercentage > 0.60f && GameManager.Instance.iPhase < 1)
                GameManager.Instance.EnterNextPhase();
            if (fPercentage <= 0.60f && fPercentage > 0.40f && GameManager.Instance.iPhase < 2)
                GameManager.Instance.EnterNextPhase();
            if (fPercentage <= 0.40f && fPercentage > 0.20f && GameManager.Instance.iPhase < 3)
                GameManager.Instance.EnterNextPhase();
            if (fPercentage <= 0.20F && GameManager.Instance.iPhase < 4)
                GameManager.Instance.EnterNextPhase();

        }

        public void Draw(SpriteBatch SB)
        {
            if (GameManager.Instance.iPhase >0)
            {                
                DrawScreenShrink(SB, GameManager.Instance.iPhase);
                particleEngine.Draw(SB);
            }

        }

        private void DrawScreenShrink(SpriteBatch SB, int iPhase)
        {
            Vector2[] pos = CalculatePositions(iPhase);


            /*******************************
               DRAWING THE SCRENE SHRINKING   
             *******************************/
            
            SB.Draw(Left, pos[1], Color.White);
            SB.Draw(Right, pos[2], Color.White);
            SB.Draw(Top, pos[0], Color.White);
            SB.Draw(Bottom, pos[3], Color.White);
           
            //foreach (Vector2 XX in pos)
            //{
            //    Console.WriteLine(XX.ToString());
            //}
            
        }

        private Vector2[] CalculatePositions(int iPhase)
        {
            Vector2[] P = new Vector2[4];
            Console.WriteLine(iPhase);
            switch (iPhase)
            {
                case 1:
                    P[0] = new Vector2(0, 0 -  (1.0f / 3.0f) *Top.Height);
                    P[1] = new Vector2(0 - (1.0f / 3.0f) * Left.Width,0);
                    P[2] = new Vector2(0 + (1.0f / 3.0f) * Right.Width,0);
                    P[3] = new Vector2(0, 0 + (1.0f / 3.0f) * Bottom.Height);
                    break;
                case 2:
                    P[0] = new Vector2(0, 0 - (1.0f / 5.0f) * Top.Height);
                    P[1] = new Vector2(0 - (1.0f / 5.0f) * Left.Width, 0);
                    P[2] = new Vector2(0 + (1.0f / 5.0f) * Right.Width, 0);
                    P[3] = new Vector2(0, 0 + (1.0f / 5.0f) * Bottom.Height);
                    break;
                case 3:
                    P[0] = new Vector2(0, 0 - (1.0f / 8.0f) * Top.Height);
                    P[1] = new Vector2(0 - (1.0f / 8.0f) * Left.Width, 0);
                    P[2] = new Vector2(0 + (1.0f / 8.0f) * Right.Width, 0);
                    P[3] = new Vector2(0, 0 + (1.0f / 8.0f) * Bottom.Height);
                    break;
                case 4:
                    P[0] = new Vector2(0, 0);
                    P[1] = new Vector2(0, 0);
                    P[2] = new Vector2(0, 0);
                    P[3] = new Vector2(0, 0);
                    break;
                default:
                    break;
            }

            return P;
        }
    }
}
