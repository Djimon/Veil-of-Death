using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VeilofDeath.Core.GameStates
{
    class Score : IGameState
    {
        public bool canLeave { get; set; }
        public EState newState { get; set; }

        private Texture2D background;

        SpriteBatch spriteBatch;
        IGameState _callingGameState;


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
        }

        public void LoadContent()
        {
            background = GameConstants.Content.Load<Texture2D>("winner");
        }

        public void UnloadContent()
        {
            
        }

        public void Update(GameTime time)
        {
            
        }

        public void Draw(GameTime time)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(background, new Rectangle(0, 0, (int)GameConstants.WINDOWSIZE.X, (int)GameConstants.WINDOWSIZE.Y), Color.White);

            spriteBatch.End();
        }
    }
}
