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
        private Texture2D background;
        public bool canLeave { get; set; }       
        public EState newState { get; set; }

        int iStatus = 0;


        public GameOver(int status)
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
            background = GameConstants.Content.Load<Texture2D>("menuBG");
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
                case 1: // Siegerbildschrm (nextLevt oder neuer Versuch
                    //Anzeige der Zeit und der Sterne
					UpdateWIN();
                    break;
                default: break;
            }
        }

        private void UpdateWIN()
        {
            GameManager.Instance.LevelUp();
            if(Keyboard.GetState().IsKeyDown(Keys.Enter))
                newState = EState.Ingame;
        }

        private void UpdateLOSE()
        {
            if(Keyboard.GetState().IsKeyDown(Keys.Enter))
                newState = EState.Ingame; 
        }

        public void Draw(GameTime time)
        {
            spriteBatch.Begin();
            //TODO: Draw background

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

            spriteBatch.End();

        }

        private void DrawWIN()
        {
            throw new NotImplementedException();
        }

        private void DrawLOSE()
        {
            throw new NotImplementedException();
        }
    }
}
