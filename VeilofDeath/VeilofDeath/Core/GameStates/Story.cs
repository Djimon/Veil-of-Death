using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VeilofDeath.PanelStuff;
using Microsoft.Xna.Framework.Input;

namespace VeilofDeath.Core.GameStates
{
    class Story : IGameState
    {
        private SpriteBatch spriteBatch;
        private int iLevel;
        private bool isKeyPressed = true;

        Panel MainPanel;
        PanelElement peHeader;

        public bool canLeave { get; set; }


        public EState newState { get; set; }

        public Story(int level)
        {
            spriteBatch = GameConstants.SpriteBatch;
            iLevel = level;
            Initialize();
            LoadContent();
            if (iLevel != 0 && iLevel != 3 && GameConstants.iWinStatus != 1)
                Continue();

            Console.WriteLine("instanciate story: " + iLevel + " won? : " + GameConstants.iWinStatus);
        }

        private void Continue()
        {
            newState = EState.Ingame;
        }

        public void Initialize()
        {
            newState = EState.none;
        }

        public void LoadContent()
        {
            MainPanel = new Panel(GameConstants.Content.Load<Texture2D>("Panels/menu/MenuBG"), new Vector2(0, 0));
            switch (iLevel)
            {
                case 0:
                    peHeader = new PanelElement(GameConstants.Content.Load<Texture2D>("Story/storyTest"), true);
                    break;
                case 3:
                    peHeader = new PanelElement(GameConstants.Content.Load<Texture2D>("Story/storyTest"), true);
                    break;
                case GameConstants.iMaxLevel:
                    peHeader = new PanelElement(GameConstants.Content.Load<Texture2D>("Story/storyTest"), true);
                    break;
                default:
                    peHeader = new PanelElement(GameConstants.Content.Load<Texture2D>("Story/storyTest"), true);
                    break;
            }

            MainPanel.Add(peHeader, new Vector2(0f, 0f));
        }

        public void UnloadContent()
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime time)
        {
            if (!isKeyPressed && Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                if (iLevel == 0)
                {
                    Continue();
                    canLeave = true;
                    GameManager.Instance.hasStoryRead[0] = true;
                    Console.WriteLine("Story 1");
                }          
                if (iLevel == 3)
                {
                    Continue();
                    canLeave = true;
                    GameManager.Instance.hasStoryRead[1] = true;
                    Console.WriteLine("Story 2");
                }
                if (iLevel >= GameConstants.iMaxLevel || GameConstants.iWinStatus == 1)
                {
                    newState = EState.GameOver;
                    canLeave = true;
                    GameManager.Instance.hasStoryRead[2] = true;
                    Console.WriteLine("Story 3");
                }               
            }            

            if (!isKeyPressed && Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                newState = EState.MainMenu;
                canLeave = true;
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Escape) && Keyboard.GetState().IsKeyUp(Keys.Enter))
                isKeyPressed = false;

        }

        public void Draw(GameTime time)
        {
            spriteBatch.Begin();

            MainPanel.Draw(spriteBatch);

            spriteBatch.End();
        }

    }
}
