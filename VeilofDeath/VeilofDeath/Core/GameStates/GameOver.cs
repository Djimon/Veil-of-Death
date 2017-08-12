﻿using System;
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


        int iStatus = 0;

        int sumscore, sumTime;
        float sumCompl;

        public GameOver(int status)
        {
            spriteBatch = GameConstants.SpriteBatch;
            Initialize();
            iStatus = status;
            LoadContent();
        }


        public void Initialize()
        {
            newState = EState.none;
        }

        public void LoadContent()
        {
            background = GameConstants.Content.Load<Texture2D>("noob");
            win = GameConstants.Content.Load<Texture2D>("winner");

            sumscore = 0;
            foreach (int A in GameManager.Instance.iCoinScore)
            {
                sumscore += A;
            }
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
            // TODO
        }

        private void UpdateLOSE()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                GameConstants.MainCam.ResetCamera();
                GameManager.Instance.ResetScore();                
                newState = EState.Ingame;

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
            //TODO: Draw Statistics
    

        }

        private void DrawWIN()
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
            //Debug - Anzeige
            spriteBatch.Draw(win, new Rectangle(0, 0, (int)GameConstants.WINDOWSIZE.X, (int)GameConstants.WINDOWSIZE.Y), Color.White);


            spriteBatch.DrawString(GameConstants.lucidaConsole, "Score: " + sumscore,
                new Vector2(200,200) , Color.White);
            spriteBatch.DrawString(GameConstants.lucidaConsole, "Completeness: " + (int)100 * sumCompl + "%",
                new Vector2(200, 240), Color.White);
            spriteBatch.DrawString(GameConstants.lucidaConsole, "ZeitBonus: " + sumTime,
                new Vector2(200, 280), Color.White);


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
