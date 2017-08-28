using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace VeilofDeath.Core.GameStates
{
    class Statistics : IGameState
    {
        public bool canLeave { get; set; }
        

        public EState newState { get; set; }

        public Statistics()
        {
            Initialize();
            LoadContent();
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void LoadContent()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}
