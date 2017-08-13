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
        IGameState _callingGameState;
        private Vector2 GUI_Pos = new Vector2(1000, 20);
        private Vector2 GUI_Pos2 = new Vector2(960, 60);
        private Vector2 GUI_Pos3 = new Vector2(920, 100);

        private float fcompletenes;
        private int ilevelscore, ilevelCoins, iTimeBonus;
        private bool mustRetry = true;

        //PANEL STUFF
        Panel MainPanel;
        PanelElement    peCoin, // Icon of Coin
                        ptCoin, // Text: number of Coins
                        ptScore, // Text: actual Score
                        ptTimebonus, //Text: actual TimeBonus
                        ptText, // Text: Main-Info Text
                        pePictuer, // Sprite of Warden or Door
                        pbtRetry, // Button: retry
                        pbtRetryS, // Button: retry selected
                        pbtConfirm, // Button: pay fee and go on
                        pbtConfirmS; // Button Confirm Selected 
        private btn m_selected;
        private bool ispressed;

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

            //background = GameConstants.Content.Load<Texture2D>("Panels/BG");

            MainPanel = new Panel(  GameConstants.Content.Load<Texture2D>("Panels/panel"),
                                    new Vector2(0.1f * GameConstants.WINDOWSIZE.X,
                                                0.1f * GameConstants.WINDOWSIZE.Y));

            peCoin = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/coin"),true); // Icon of Coin
            ptCoin = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/void"), true); // Text: number of Coins
            ptCoin.AddText(GameConstants.lucidaConsole, "   " + ilevelCoins, Color.White);
            ptScore = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/void"), true); // Text: actual Score
            ptScore.AddText(GameConstants.lucidaConsole, "Score:    " + ilevelscore, Color.White);
            ptTimebonus = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/void"), true); //Text: actual TimeBonus
            ptTimebonus.AddText(GameConstants.lucidaConsole, "Time Bonus:    " + iTimeBonus, Color.White);
            pbtRetry = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/button"), true); // Button: retry
            pbtRetry.AddText(GameConstants.lucidaConsole, "     Retry  ", Color.White);
            pbtRetryS = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/button"),true); // Button: retry selected
            pbtRetryS.AddText(GameConstants.lucidaConsole, "    Retry  ", Color.OrangeRed);

            //LOADNG THE ELEMENTS
            if (!mustRetry)
            {                
                ptText = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/ConfirmText"), true); // Text: Main-Info Text
                pePictuer = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/door_0-0_short"), true); // Sprite of Warden or Door
                pbtConfirm = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/button"), true); // Button: pay fee and go on
                pbtConfirm.AddText(GameConstants.lucidaConsole, "   Confirm " + ilevelCoins *2/3, Color.White);
                pbtConfirmS = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/button"), true); // Button Confirm Selected
                pbtConfirmS.AddText(GameConstants.lucidaConsole, "   Confirm "+ ilevelCoins * 2 / 3, Color.OrangeRed);
            }
            else
            {
                ptText = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/RetryText"), true); // Text: Main-Info Text
                pePictuer = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/door_riegel_short"), true); // Sprite of Warden or Door
                pePictuer.AddTexture(GameConstants.Content.Load<Texture2D>("Panels/door_0-0_short"));
            }

            // FILLING THE PANEL
            MainPanel.Add(peCoin, new Vector2(0.07f, 0.1f));
            MainPanel.Add( ptCoin, new Vector2(0.15f, 0.12f));
            MainPanel.Add( ptScore, new Vector2(0.45f, 0.12f));
            MainPanel.Add( ptTimebonus, new Vector2(0.45f, 0.2f));
            MainPanel.Add( ptText, new Vector2(0.45f, 0.3f));
            MainPanel.Add( pePictuer, new Vector2(0.07f, 0.3f));
            MainPanel.Add( pbtRetry, new Vector2(0.6f, 0.75f));
            MainPanel.Add( pbtRetryS, new Vector2(0.6f, 0.75f));
            if (!mustRetry)
            {
                MainPanel.Add( pbtConfirm, new Vector2(0.2f, 0.75f));
                MainPanel.Add( pbtConfirmS, new Vector2(0.2f, 0.75f));
            }


        }

        public void UnloadContent()
        {
            
        }

        public void Update(GameTime time)
        {

            if (!mustRetry)
            {
                if (!ispressed && Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    m_selected = (btn)(((int)m_selected + 1) % (int)btn.Count);
                    ispressed = true;
                }
                if (!ispressed && Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    m_selected = (btn)(((int)m_selected + 1) % (int)btn.Count);
                    ispressed = true;
                }
                if (Keyboard.GetState().IsKeyUp(Keys.Left) && Keyboard.GetState().IsKeyUp(Keys.Right))
                    ispressed = false;



                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    switch (m_selected)
                    {
                        case btn.confirm:
                            if (GameManager.Instance.Level + 1 < GameConstants.iMaxLevel)
                            {        
                                GameConstants.MainCam.ResetCamera();
                                GameManager.Instance.ResetScore();
                                GameManager.Instance.iCoinScore[GameManager.Instance.Level] -= ilevelCoins * 2 / 3;
                                GameManager.Instance.LevelUp();
                                newState = EState.Ingame;
                            }
                            else
                            {
                                GameConstants.iWinStauts = 1;
                                newState = EState.GameOver;
                            }
                            break;
                        case btn.Retry:
                            GameConstants.MainCam.ResetCamera();
                            GameManager.Instance.ResetScore();
                            newState = EState.Ingame;
                            break;
                        default:
                            newState = EState.none;
                            break;
                    }
                }

                switch (m_selected)
                {
                    case btn.confirm:
                        pbtRetry.isActive = true;
                        pbtRetryS.isActive = false;
                        pbtConfirm.isActive = false;
                        pbtConfirmS.isActive = true;
                        break;
                    case btn.Retry:
                        pbtRetry.isActive = false;
                        pbtRetryS.isActive = true;
                        pbtConfirm.isActive = true;
                        pbtConfirmS.isActive = false;
                        break;
                    default:
                        pbtRetry.isActive = false;
                        pbtRetryS.isActive = true;
                        pbtConfirm.isActive = true;
                        pbtConfirmS.isActive = false;
                        break;
                }

            } /* if (!mustretry) */
            else
            {
                pbtRetry.isActive = false;
                pbtRetryS.isActive = true;

                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    GameConstants.MainCam.ResetCamera();
                    GameManager.Instance.ResetScore();
                    newState = EState.Ingame;
                }
            }

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
            //spriteBatch.Draw(background, new Vector2(0, 0), Color.White);

            MainPanel.Draw(spriteBatch);
            

            spriteBatch.End();
        }

        private void DrawRetry()
        {
            //ALT
            spriteBatch.Draw(background, new Rectangle(0, 0, (int)GameConstants.WINDOWSIZE.X, (int)GameConstants.WINDOWSIZE.Y), Color.White);
            spriteBatch.DrawString(GameConstants.lucidaConsole, "Score: " + ilevelscore+ iTimeBonus,
                GUI_Pos, Microsoft.Xna.Framework.Color.White);
            spriteBatch.DrawString(GameConstants.lucidaConsole, "Completeness: " + (int)100 * fcompletenes + "%",
                GUI_Pos2, Microsoft.Xna.Framework.Color.White);
            spriteBatch.DrawString(GameConstants.lucidaConsole, "gesammelte Coins: " + ilevelCoins,
                GUI_Pos3, Microsoft.Xna.Framework.Color.White);

            // NEU


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
