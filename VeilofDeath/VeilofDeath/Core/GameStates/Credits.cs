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
    class Credits : IGameState
    {
        private SpriteBatch spriteBatch;

        Panel MainPanel;
        PanelElement peHeader;

        public bool canLeave { get; set; }
        

        public EState newState { get; set; }

        public Credits()
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

            peHeader = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/menu/creditscomplete"), true);
            //TODO: mit den einzelnen Elementen umsetzen, hier aus Faulheit ein großes .png für die Controls


            MainPanel.Add(peHeader, new Vector2(0.1f, 0.1f));
        }

        public void UnloadContent()
        {
            throw new NotImplementedException();
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
