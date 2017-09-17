using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using VeilofDeath.PanelStuff;

namespace VeilofDeath.Core.GameStates
{

    enum btn
    {
        Retry,
        confirm,

        Count
    }

    class Score : IGameState
    {
        public bool canLeave { get; set; }
        public EState newState { get; set; }

        private Texture2D background;

        SpriteBatch spriteBatch;
        private Vector2 GUI_Pos = new Vector2(1000, 20);
        private Vector2 GUI_Pos2 = new Vector2(960, 60);
        private Vector2 GUI_Pos3 = new Vector2(920, 100);

        private float fcompletenes;
        private int ilevelscore, ilevelCoins, iTimeBonus;
        private bool mustRetry = true;
        private bool isPlayed = false;

        //PANEL STUFF
        Panel MainPanel;
        PanelElement peCoin, // Icon of Coin
                        ptCoin, // Text: number of Coins
                        ptScore, // Text: actual Score
                        ptTimebonus, //Text: actual TimeBonus
                        ptScoreSum, // Text Score with TimeBonus
                        ptText, // Text: Main-Info Text
                        pePictuer, // Sprite of Warden or Door
                        pbtRetry, // Button: retry
                        ptRetryS, // Button: retry selected
                        pbtConfirm, // Button: pay fee and go on
                        ptConfirmS, // Button Confirm Selected 
                        ptRetry, //Buttontect: Retry
                        ptConfirm; // Buttontext: confirm

        private btn m_selected;
        private bool ispressed = true;

        /// <summary>
        /// constructor for the Score Screen at the nd of each level
        /// </summary>
        public Score()
        {
            //_callingGameState = callingState;
            spriteBatch = GameConstants.SpriteBatch;

            Initialize();
            LoadContent();

            GameManager.Instance.Save();
        }

        public void Initialize()
        {
            GameConstants.isRetryQuickJoinOn = false;
            newState = EState.none;
            ilevelscore = GameManager.Instance.score;
            ilevelCoins = GameManager.Instance.iCoinScore[GameManager.Instance.Level];
            iTimeBonus = GameManager.Instance.iTimeBonus[GameManager.Instance.Level];
            fcompletenes = GameManager.Instance.fStageCleared[GameManager.Instance.Level];


            // last Call
            if (fcompletenes > 0.5f)
            {
                mustRetry = false;
                if (GameConstants.isDebugMode)
                    Console.WriteLine("Success");
            }
            else
            {
                if (GameConstants.isDebugMode)
                    Console.WriteLine("Have to Retry!");
            }

        }

        public void LoadContent()
        {

            background = GameConstants.Content.Load<Texture2D>("Panels/BGScore");

            MainPanel = new Panel(  GameConstants.Content.Load<Texture2D>("Panels/panel"),
                                    new Vector2(0.1f * GameConstants.WINDOWSIZE.X,
                                                0.1f * GameConstants.WINDOWSIZE.Y));

            string score = " Score: " + (ilevelscore - iTimeBonus); //TODO: Replaxe Spaces with \#  code
            string tbon = "Time Bonus:  " + iTimeBonus;
            string retry = "   Retry   ";
            string sumscore = " Result: " + ilevelscore;

            peCoin = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/coin"),true); // Icon of Coin
            ptCoin = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/void"), true); // Text: number of Coins
            ptCoin.AddText(GameConstants.lucidaConsole, "   " + ilevelCoins, Color.White);
            ptScore = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/void"), true); // Text: actual Score
            ptScore.AddText(GameConstants.lucidaConsole, score , Color.White);
            ptTimebonus = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/void"), true); //Text: actual TimeBonus
            ptTimebonus.AddText(GameConstants.lucidaConsole, tbon , Color.White);
            ptScoreSum = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/void"), true); // Text: Result:
            ptScoreSum.AddText(GameConstants.lucidaConsole, sumscore, Color.White);

            pbtRetry = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/button"), true); // Button: retry
            ptRetry = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/void"), true);
            ptRetry.AddText(GameConstants.lucidaConsole, retry, Color.White);
            ptRetryS = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/void"),true); // Button: retry selected
            ptRetryS.AddText(GameConstants.lucidaConsole, retry, Color.OrangeRed);

            //LOADNG THE ELEMENTS
            if (!mustRetry)
            {
                string pay = "Pay $" + (ilevelCoins * 1 / 2);
                ptText = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/ConfirmText"), true); // Text: Main-Info Text
                pePictuer = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/door_0-0_short"), true); // Sprite of Warden or Door
                pbtConfirm = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/button"), true); // Button: pay fee and go on
                ptConfirm = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/void"), true);
                ptConfirm.AddText(GameConstants.lucidaConsole, pay, Color.White);
                ptConfirmS = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/void"), true); // Button Confirm Selected
                ptConfirmS.AddText(GameConstants.lucidaConsole, pay, Color.OrangeRed);
            }
            else
            {
                ptText = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/RetryText"), true); // Text: Main-Info Text
                pePictuer = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/door_riegel_short"), true); // Sprite of Warden or Door
                pePictuer.AddTexture(GameConstants.Content.Load<Texture2D>("Panels/door_0-0_short"));
            }

            // FILLING THE PANEL
            MainPanel.Add( peCoin, new Vector2(0.07f, 0.1f));
            MainPanel.Add( ptCoin, new Vector2(0.15f, 0.1f));
            MainPanel.Add( ptScore, new Vector2(0.6f, 0.1f));
            MainPanel.Add( ptTimebonus, new Vector2(0.6f, 0.18f));
            MainPanel.Add(ptScoreSum, new Vector2(0.6f, 0.26f));
            MainPanel.Add( ptText, new Vector2(0.45f, 0.3f));
            MainPanel.Add( pePictuer, new Vector2(0.07f, 0.3f));
            MainPanel.Add( pbtRetry, new Vector2(0.6f, 0.75f));
            MainPanel.Add( ptRetry, new Vector2(0.65f, 0.76f));
            MainPanel.Add( ptRetryS, new Vector2(0.65f, 0.76f));
            if (!mustRetry)
            {
                MainPanel.Add( pbtConfirm, new Vector2(0.2f, 0.75f));
                MainPanel.Add( ptConfirm, new Vector2(0.25f, 0.76f));
                MainPanel.Add( ptConfirmS, new Vector2(0.25f, 0.76f));
            }


        }

        public void UnloadContent()
        {
            
        }

        public void Update(GameTime time)
        {

            if (!isPlayed)
            {
                if (mustRetry)
                    GameConstants.Loser.Play();//LARS: Play Sound: verloren
                else
                    GameConstants.Winner.Play();//LARS: play sound: gewonnen

                isPlayed = true;
            }


            if (!mustRetry)
            {
                UpdateRetry();
            } 
            else
            {
                ptRetry.isActive = false;
                ptRetryS.isActive = true;
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    Retry();
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                GameConstants.MainCam.ResetCamera();
                GameManager.Instance.ResetLevel();
                newState = EState.MainMenu;
                canLeave = true;
            }

        }

        private void UpdateRetry()
        {
            if (!ispressed && Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                m_selected = (btn)(((int)m_selected + 1) % (int)btn.Count);
                ispressed = true;
                GameConstants.Switch.Play(); //LARS: Play sound: switch selected Button
            }
            if (!ispressed && Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                m_selected = (btn)(((int)m_selected + 1) % (int)btn.Count);
                ispressed = true;
                GameConstants.Switch.Play();//LARS: Play sound: switch selected Button
            }
            if (Keyboard.GetState().IsKeyUp(Keys.Left) && Keyboard.GetState().IsKeyUp(Keys.Right) && Keyboard.GetState().IsKeyUp(Keys.Enter))
                ispressed = false;



            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                GameConstants.Select.Play();//LARS: play sound: Menüpunkt bestätigen
                ispressed = true;
                switch (m_selected)
                {
                    case btn.confirm:
                        if (GameManager.Instance.Level + 1 < GameConstants.iMaxLevel)
                        {
                            GameConstants.MainCam.ResetCamera();
                            GameManager.Instance.iCoinScore[GameManager.Instance.Level] -= ilevelCoins * 1 / 2;
                            GameManager.Instance.LevelUp();
                            if (GameManager.Instance.Level == 4 && ! GameManager.Instance.hasStoryRead[1])
                              newState = EState.Story;
                            else
                              newState = EState.Ingame;
                            canLeave = true;
                        }
                        else
                        {
                            GameConstants.iWinStatus = 1;
                            GameConstants.hasGameWon = true;
                            newState = EState.Story;
                            canLeave = true;
                        }
                        break;
                    case btn.Retry:
                        Retry();
                        break;
                    default:
                        newState = EState.none;
                        break;
                }
            }

            switch (m_selected)
            {
                case btn.confirm:
                    ptRetry.isActive = true;
                    ptRetryS.isActive = false;
                    ptConfirm.isActive = false;
                    ptConfirmS.isActive = true;
                    break;
                case btn.Retry:
                    ptRetry.isActive = false;
                    ptRetryS.isActive = true;
                    ptConfirm.isActive = true;
                    ptConfirmS.isActive = false;
                    break;
                default:
                    ptRetry.isActive = false;
                    ptRetryS.isActive = true;
                    ptConfirm.isActive = true;
                    ptConfirmS.isActive = false;
                    break;
            }
        }

        private void Retry()
        {
            GameConstants.Select.Play();//LARS: play sound: Menüpunkt bestätigen
            GameConstants.MainCam.ResetCamera();
            GameManager.Instance.ResetLevel();
            //GameConstants.isQuickJoin = true;
            newState = EState.Ingame;
            canLeave = true;
        }

        public void Draw(GameTime time)
        {
            spriteBatch.Begin();
            //ALT
            //if (mustRetry)
            //{ DrawRetry(); }
            //else
            //{ DrawNoRetry(); }
            //NEU
          
            spriteBatch.Draw(background, new Rectangle(0, 0, (int)GameConstants.WINDOWSIZE.X, (int)GameConstants.WINDOWSIZE.Y), Color.White);
            MainPanel.Draw(spriteBatch);
            

            spriteBatch.End();
        }

        //private void DrawRetry()
        //{
        //    //ALT
        //    spriteBatch.Draw(background, new Rectangle(0, 0, (int)GameConstants.WINDOWSIZE.X, (int)GameConstants.WINDOWSIZE.Y), Color.White);
        //    spriteBatch.DrawString(GameConstants.lucidaConsole, "Score: " + ilevelscore+ iTimeBonus,
        //        GUI_Pos, Microsoft.Xna.Framework.Color.White);
        //    spriteBatch.DrawString(GameConstants.lucidaConsole, "Completeness: " + (int)100 * fcompletenes + "%",
        //        GUI_Pos2, Microsoft.Xna.Framework.Color.White);
        //    spriteBatch.DrawString(GameConstants.lucidaConsole, "gesammelte Coins: " + ilevelCoins,
        //        GUI_Pos3, Microsoft.Xna.Framework.Color.White);
        //}
        //private void DrawNoRetry()
        //{
        //    spriteBatch.Draw(background, new Rectangle(0, 0, (int)GameConstants.WINDOWSIZE.X, (int)GameConstants.WINDOWSIZE.Y), Color.White);
        //    spriteBatch.DrawString(GameConstants.lucidaConsole, "Score: " + ilevelscore,
        //        GUI_Pos, Microsoft.Xna.Framework.Color.White);
        //    spriteBatch.DrawString(GameConstants.lucidaConsole, "Completeness: " + (int)100 * fcompletenes + "%",
        //        GUI_Pos2, Microsoft.Xna.Framework.Color.White);
        //    spriteBatch.DrawString(GameConstants.lucidaConsole, "gesammelte Coins: " + ilevelCoins,
        //        GUI_Pos3, Microsoft.Xna.Framework.Color.White);
        //}
    }
}
