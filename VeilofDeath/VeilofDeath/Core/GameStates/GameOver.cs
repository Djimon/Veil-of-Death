using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VeilofDeath.Core.GameStates
{
    class GameOver : IGameState
    {
        public bool canLeave { get; set; }       
        public EState newState { get; set; }

        int iStatus = 0;
        private SpriteBatch spriteBatch;

        public GameOver(SpriteBatch batch, int status)
        {
            spriteBatch = batch;
            iStatus = status;
            Initialize();
            LoadContent();
        }


        public void Initialize()
        {
            
        }

        public void LoadContent()
        {
            // Load Textures2D for background  etc.
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
                    break;
                case 1: // Siegerbildschrm (nextLevt oder neuer Versuch
                    //Anzeige der Zeit und der Sterne
                    break;
                default: break;
            }
        }

        public void Draw(GameTime time)
        {
            spriteBatch.Begin();
            //TODO: Draw background

            switch (iStatus)
            {
                case 0: // Verliererbildschirm, Neuer Versuch
                    break;
                case 1: // Siegerbildschrm (nextLevt oder neuer Versuch
                    //Anzeige der Zeit und der Sterne
                    break;
                default: break;
            }

            //TODO: Draw Statistics

            spriteBatch.End();

        }
    }
}
