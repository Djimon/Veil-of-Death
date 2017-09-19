using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace VeilofDeath.Core.GameStates
{
    class GameOver : IGameState
    {
        SpriteBatch spriteBatch;
        private Texture2D background,win;
        public bool canLeave { get; set; }       
        public EState newState { get; set; }

        private bool isPlayed = false;

        int iStatus = 0;

        int sumscore, sumTime;
        float sumCompl;
        private bool isppressed = true;

        public GameOver(int status)
        {
            iStatus = status;
            spriteBatch = GameConstants.SpriteBatch;
            Initialize();            
            LoadContent();
        }


        public void Initialize()
        {
            newState = EState.none;
            isPlayed = false;

            if (iStatus == 0)
                GameManager.Instance.DeathUp();
        }

        public void LoadContent()
        {
            background = GameConstants.Content.Load<Texture2D>("noob");
            win = GameConstants.Content.Load<Texture2D>("gamewon");

            sumscore = 0;
            foreach (int A in GameManager.Instance.iCoinScore)
            {
                sumscore += A;
            }
            sumscore *= 25;
            sumCompl = 0f;
            for (int i = 0; i < GameManager.Instance.fStageCleared.Count<float>(); i++)
            {
                sumCompl += GameManager.Instance.fStageCleared[i];
            }
            sumCompl = sumCompl / GameManager.Instance.fStageCleared.Count<float>();
            sumTime = 0;
            foreach (int A in GameManager.Instance.iTimeBonus)
            {
                sumTime += A;
            }

            //Load other Textures, like Buttons
        }

        public void UnloadContent()
        {
            //
        }

        public void Update(GameTime time)
        {           
            switch (iStatus)
            {
                case 0: // Verliererbildschirm, Neuer Versuch
				    UpdateLOSE();
                    break;
                case 1: // Siegerbildschrm (nextLevt oder neuer Versuch)
                    //Anzeige der Zeit und der Sterne
					UpdateWIN();
                    break;
                default: break;
            }
        }

        private void UpdateWIN()
        {
            if (!isPlayed)
            {
                GameConstants.TotalWinner.Play();//LARS: Play Sound: Gewonnen! Spiel durchgepsielt

                isPlayed = true;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.Enter))
                isppressed = false;

            //TODO: Platzhalter
            if (!isppressed && Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                isppressed = true;
                GameConstants.MainCam.ResetCamera();
                GameManager.Instance.Save();
                newState = EState.Statistics;
                canLeave = true;
                GameConstants.savedSeconds = 0;
                GameConstants.isPlayedAtmosphere = false;
            }
        }

        private void UpdateLOSE()
        {
            if (!isPlayed)
            {
                GameConstants.Loser.Play();//LARS: Play Sound: Sterbesound

                isPlayed = true;
            }
                

            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                GameConstants.MainCam.ResetCamera();
                GameManager.Instance.ResetScore();                
                newState = EState.Ingame;
                canLeave = true;
                GameConstants.isRetryQuickJoinOn = true;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                GameConstants.MainCam.ResetCamera();
                GameManager.Instance.ResetScore();
                GameManager.Instance.Save();
                newState = EState.MainMenu;
                canLeave = true;
                GameConstants.isRetryQuickJoinOn = false;
                GameConstants.savedSeconds = 0;
                GameConstants.isPlayedAtmosphere = false;
            }

        }

        public void Draw(GameTime time)
        {
            switch (iStatus)
            {
                case 0: // Verliererbildschirm, Neuer Versuch
				    DrawLOSE();
                    break;
                case 1: // Siegerbildschrm (nextLevt oder neuer Versuch
                    //Anzeige der Zeit und der Sterne
					DrawWIN();
                    break;
                default: break;
            }    

        }

        private void DrawWIN()
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
            //Debug - Anzeige
            spriteBatch.Draw(win, new Rectangle(0, 0, (int)GameConstants.WINDOWSIZE.X, (int)GameConstants.WINDOWSIZE.Y), Color.White);


            spriteBatch.End();

        }

        private void DrawLOSE()
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
            //Debug - Anzeige
            spriteBatch.Draw(background, new Rectangle(0, 0, (int)GameConstants.WINDOWSIZE.X, (int)GameConstants.WINDOWSIZE.Y), Color.White);

            spriteBatch.End();
        
    }


    }
}
