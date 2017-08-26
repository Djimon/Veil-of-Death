using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using VeilofDeath.SpecialFX;

namespace VeilofDeath.Core.GameStates
{
    enum Button
    {
        Start,
        Credits,
        Winnerlist,
        Exit,

        Count
    }
    class MainMenu : IGameState
    {
        SpriteBatch spriteBatch;
        bool ispressed = false;

        private Texture2D background;
        private Texture2D bubble;

        public EState newState { get; set; }

        public bool canLeave { get; set; }

        private Texture2D[] Buttons;

        private Texture2D[] SelectedButtons;

        private Button m_selected;

        AParticleEnginge ParticleTest, ParticleTest2;

        public MainMenu()
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
            background = GameConstants.Content.Load<Texture2D>("titleBG");
            Buttons = new Texture2D[(int)Button.Count];
            Buttons[(int)Button.Start] = GameConstants.Content.Load<Texture2D>("Textures/StartButton");
            Buttons[(int)Button.Credits] = GameConstants.Content.Load<Texture2D>("Textures/CreditsB");
            Buttons[(int)Button.Winnerlist] = GameConstants.Content.Load<Texture2D>("Textures/Winnerlist");
            Buttons[(int)Button.Exit] = GameConstants.Content.Load<Texture2D>("Textures/ExitButton");
            //more buttons
            SelectedButtons = new Texture2D[(int)Button.Count];
            SelectedButtons[(int)Button.Start] = GameConstants.Content.Load<Texture2D>("Textures/StartButtonSelected");
            SelectedButtons[(int)Button.Credits] = GameConstants.Content.Load<Texture2D>("Textures/CreditsBS");
            SelectedButtons[(int)Button.Winnerlist] = GameConstants.Content.Load<Texture2D>("Textures/WinnerListS");
            SelectedButtons[(int)Button.Exit] = GameConstants.Content.Load<Texture2D>("Textures/ExitButtonSelected");

            //Load other Textures, like Buttons
            
        }

        public void UnloadContent()
        {
            
        }

        public void Update(GameTime time)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                newState = EState.Ingame;

            if (!ispressed && Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                m_selected = (Button)(((int)m_selected + 1) % (int)Button.Count);
                ispressed = true;
            }

            if (!ispressed && Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                m_selected = (Button)(((int)m_selected +(int)Button.Count - 1) % (int)Button.Count);
                ispressed = true;
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Down) && Keyboard.GetState().IsKeyUp(Keys.Up)) 
                ispressed = false;

            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                switch (m_selected)
                {
                    case Button.Start:
                        newState = EState.Ingame;
                        break;

                    case Button.Exit:
                        GameConstants.currentGame.Exit();
                        break;
                        

                    default:
                        newState = EState.none;
                        break;
                }

                canLeave = true;
            }


            //ParticleTest.Update(time);

        }

        public void Draw(GameTime time)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Rectangle(0, 0, (int)GameConstants.WINDOWSIZE.X, (int)GameConstants.WINDOWSIZE.Y), Color.White);
            //Draw more 2D stuff here
            Vector2 Anker = new Vector2(410, 200);
            for (int i = 0; i < (int)Button.Count; i++) 
            {
                if (i == (int)m_selected)
                {
                    spriteBatch.Draw(SelectedButtons[i], Anker + new Vector2(0, i * 100), Color.White);
                }
                else
                {
                    spriteBatch.Draw(Buttons[i], Anker + new Vector2(0, i * 100), Color.White);
                }
            }

            //ParticleTest.Draw(spriteBatch);

            spriteBatch.End();
        }
    }
}
