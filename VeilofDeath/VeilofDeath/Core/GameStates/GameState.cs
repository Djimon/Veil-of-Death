using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeilofDeath.Core
{
    enum EState
    {
        none = -1,
        Start,
        MainMenu,
        Settings,
        Controlls,
        Ingame,
        Score,
        GameOver,
        Credits
    }

    interface IGameState
    {
        EState newState { get; set; }
        bool canLeave { get; set; }

        void Initialize();

        void LoadContent();

        void UnloadContent();

        void Update(GameTime time);

        void Draw(GameTime time);

    }
}
