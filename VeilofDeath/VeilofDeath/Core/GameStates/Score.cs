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
    class Score : IGameState
    {
        public bool canLeave { get; set; }
        public EState newState { get; set; }

        private Texture2D background,background2;

        SpriteBatch spriteBatch;
        IGameState _callingGameState;
        private Vector2 GUI_Pos = new Vector2(1000, 20);
        private Vector2 GUI_Pos2 = new Vector2(960, 60);
        private Vector2 GUI_Pos3 = new Vector2(920, 100);

        private float fcompletenes;
        private int ilevelscore, ilevelCoins, iTimeBonus;
        private bool isLevelup = false;
        private bool mustRetry = true;

        public Score()
        {
            //_callingGameState = callingState;
            spriteBatch = GameConstants.SpriteBatch;

            Initialize();
            LoadContent();
        }

        public void Initialize()
        {
            newState = EState.none;
            ilevelscore = GameManager.Instance.score;
            ilevelCoins = GameManager.Instance.iCoinScore[GameManager.Instance.Level];
            iTimeBonus = GameManager.Instance.iTimeBonus[GameManager.Instance.Level];
            fcompletenes = GameManager.Instance.fStageCleared[GameManager.Instance.Level];


            // last Call
            if (fcompletenes > 0.5f)
            {
                mustRetry = false;
                Console.WriteLine("Success");

                if (GameManager.Instance.Level+1 < GameConstants.iMaxLevel)
                {
                    GameManager.Instance.LevelUp();
                    isLevelup = true;
                }
                else
                {
                    GameConstants.iWinStauts = 1;
                    newState = EState.GameOver;
                }              
            }
            else
            {
                Console.WriteLine("Have to Retry!");
            }

        }

        public void LoadContent()
        {
            background = GameConstants.Content.Load<Texture2D>("winner");
            background2 = GameConstants.Content.Load<Texture2D>("noob");
        }

        public void UnloadContent()
        {
            
        }

        public void Update(GameTime time)
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
            spriteBatch.Begin();

            if (mustRetry)
            { DrawRetry(); }
            else
            { DrawNoRetry(); }
            

            spriteBatch.End();
        }

        private void DrawRetry()
        {
            spriteBatch.Draw(background2, new Rectangle(0, 0, (int)GameConstants.WINDOWSIZE.X, (int)GameConstants.WINDOWSIZE.Y), Color.White);
            spriteBatch.DrawString(GameConstants.lucidaConsole, "Score: " + ilevelscore+ iTimeBonus,
                GUI_Pos, Microsoft.Xna.Framework.Color.White);
            spriteBatch.DrawString(GameConstants.lucidaConsole, "Completeness: " + (int)100 * fcompletenes + "%",
                GUI_Pos2, Microsoft.Xna.Framework.Color.White);
            spriteBatch.DrawString(GameConstants.lucidaConsole, "gesammelte Coins: " + ilevelCoins,
                GUI_Pos3, Microsoft.Xna.Framework.Color.White);
        }

        private void DrawNoRetry()
        {
            spriteBatch.Draw(background, new Rectangle(0, 0, (int)GameConstants.WINDOWSIZE.X, (int)GameConstants.WINDOWSIZE.Y), Color.White);
            spriteBatch.DrawString(GameConstants.lucidaConsole, "Score: " + ilevelscore,
                GUI_Pos, Microsoft.Xna.Framework.Color.White);
            spriteBatch.DrawString(GameConstants.lucidaConsole, "Completeness: " + (int)100 * fcompletenes + "%",
                GUI_Pos2, Microsoft.Xna.Framework.Color.White);
            spriteBatch.DrawString(GameConstants.lucidaConsole, "gesammelte Coins: " + ilevelCoins,
                GUI_Pos3, Microsoft.Xna.Framework.Color.White);
        }
    }
}
