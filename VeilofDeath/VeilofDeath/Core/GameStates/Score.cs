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

        SpriteBatch spriteBatch;
        IGameState _callingGameState;


        public Score(IGameState callingState)
        {
            _callingGameState = callingState;
            spriteBatch = GameConstants.SpriteBatch;

            Initialize();
            LoadContent();
        }

        public void Initialize()
        {
            //
        }

        public void LoadContent()
        {
            //
        }

        public void UnloadContent()
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime time)
        {
            throw new NotImplementedException();
        }

        public void Draw(GameTime time)
        {
            spriteBatch.Begin();

            //TODO: still Draw the _callinGameState

            //then Draw the ScoreScreen (transparent Background)

            spriteBatch.End();
        }
    }
}
