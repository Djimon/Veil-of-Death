using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using VeilofDeath.PanelStuff;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace VeilofDeath.Core.GameStates
{
    class Statistics : IGameState
    {
        public bool canLeave { get; set; }
        
        public EState newState { get; set; }

        private Panel MainPanel;
        private SpriteBatch spriteBatch;

        public Statistics()
        {
            spriteBatch = GameConstants.SpriteBatch;
            Initialize();
            LoadContent();
        }

        public void Initialize()
        {
            newState = EState.none;
        }

        public void LoadContent()
        {
            MainPanel = new Panel(GameConstants.Content.Load<Texture2D>("Panels/menu/MenuBG"), new Vector2(0, 0));
            // new PanelElement(GameConstants.lucidaConsole, "Back", Color.White)

            MainPanel.Add(new PanelElement("Statistics",GameConstants.lucidaConsole, Color.White), new Vector2(0.05f,0.05f));
            MainPanel.Add(new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/menu/esc")),new Vector2(0.8f,0.05f));
            MainPanel.Add(new PanelElement("Back", GameConstants.lucidaConsole, Color.White), new Vector2(0.88f, 0.05f));
            MainPanel.Add(new PanelElement("TimeBonus", GameConstants.lucidaConsole, Color.White), new Vector2(0.35f, 0.15f));
            MainPanel.Add(new PanelElement("Coins", GameConstants.lucidaConsole, Color.White), new Vector2(0.55f, 0.15f));
            MainPanel.Add(new PanelElement("Score", GameConstants.lucidaConsole, Color.White), new Vector2(0.70f, 0.15f));

            int Score = 0;
            int coins = 0;

            for (int i = 0; i < GameConstants.iMaxLevel; i++)
            {
                float Y = i * 0.07f + 0.25f;
                MainPanel.Add(new PanelElement("Level" +(i+1), GameConstants.lucidaConsole, Color.White), new Vector2(0.10f, Y));
                MainPanel.Add(new PanelElement(GameManager.Instance.iTimeBonus[i].ToString(), GameConstants.lucidaConsole, Color.White), new Vector2(0.35f, Y));
                MainPanel.Add(new PanelElement(GameManager.Instance.iCoinScore[i].ToString(), GameConstants.lucidaConsole, Color.White), new Vector2(0.55f, Y));
                MainPanel.Add(new PanelElement(GameManager.Instance.Score[i].ToString(), GameConstants.lucidaConsole, Color.White), new Vector2(0.70f, Y));
                Score += GameManager.Instance.Score[i];
                coins += GameManager.Instance.iCoinScore[i];
            }

            MainPanel.Add(new PanelElement("TotalScore", GameConstants.lucidaConsole, Color.White), new Vector2(0.1f, 0.7f));
            MainPanel.Add(new PanelElement(coins.ToString(), GameConstants.lucidaConsole, Color.White), new Vector2(0.55f, 0.7f));
            MainPanel.Add(new PanelElement(Score.ToString(), GameConstants.lucidaConsole, Color.White), new Vector2(0.7f, 0.7f));
            MainPanel.Add(new PanelElement("Deaths", GameConstants.lucidaConsole, Color.White), new Vector2(0.1f, 0.77f));
            MainPanel.Add(new PanelElement(GameManager.Instance.DeathCounter.ToString(), GameConstants.lucidaConsole, Color.White), new Vector2(0.7f, 0.77f));


        }

        public void UnloadContent()
        {
            // Unload Stuff
        }

        public void Update(GameTime time)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                newState = EState.MainMenu;
                canLeave = true;
            }
        }

        public void Draw(GameTime time)
        {
            spriteBatch.Begin();

            MainPanel.Draw(spriteBatch);

            spriteBatch.End();
        }
    }
}
