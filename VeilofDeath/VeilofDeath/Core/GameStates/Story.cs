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

        Panel MainPanel;
        PanelElement peHeader;

        public bool canLeave { get; set; }


        public EState newState { get; set; }

        public Story(int level)
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
            //TODO: Lade hier 3 verschiedene Story Texte, die je nach Story abgspeichert werden


            MainPanel.Add(peHeader, new Vector2(0.1f, 0.1f));
        }

        public void UnloadContent()
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime time)
        {
            if (GameManager.Instance.Level == 0)
                UpdateStory1();

            if (GameManager.Instance.Level == 3)
                UpdateStory2();

            if (GameManager.Instance.Level >= GameConstants.iMaxLevel)
                UpdateStory3();


            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                newState = EState.MainMenu;
                canLeave = true;
            }
        }

        private void UpdateStory3()
        {
            /* Unser Held hat es Tatsächlich geschafft.
             * Wir sind den Klauen des Herrschers entkommen
             * und können nun in unsere Heimat zurückkehren.
             * 
             * Andere Abenteuer warten.
             */
        }

        private void UpdateStory2()
        {
            /* Wir haben die erste Schicht der Festung durchbrochen.
             * der Held befindet sich nun eine höhere Ebene der Festung.
             * Hier warten allerdings neue Arten von Fallen und vielleicht auch Gegner
             * Der Schleier rückt immer Näher. Laufen, laufen, laufen!
             */
        }

        private void UpdateStory1()
        {
            /* Unser Held wurde gefangen genommen in einer dunklen Festung
             * und versucht nun dieser Festun zu entkommen.
             * Der Herscher dieser Festung, "Dreg Naffets", ist ein dunkler Magier
             * und verfügt über den dunklen Zauber des Todesschleiers. Dieser breitet
             * sich aus wie eine Wolke und verzerrt Alles und jeden um sich herum
             * Als der Magier merkte, dass wir auf der Flucht sind, beschwört er seinen schleier,
             * der unseren Helden verfolgt un aufhalten will.
             */
        }

        public void Draw(GameTime time)
        {
            spriteBatch.Begin();

            MainPanel.Draw(spriteBatch);

            if (GameManager.Instance.Level == 0)
                DrawStory1();

            if (GameManager.Instance.Level == 3)
                DrawStory2();

            if (GameManager.Instance.Level >= GameConstants.iMaxLevel)
                DrawStory3();



            spriteBatch.End();
        }

        private void DrawStory1()
        {
            throw new NotImplementedException();
        }

        private void DrawStory2()
        {
            throw new NotImplementedException();
        }

        private void DrawStory3()
        {
            throw new NotImplementedException();
        }
    }
}
