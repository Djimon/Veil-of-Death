using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using VeilofDeath.SpecialFX;
using VeilofDeath.PanelStuff;

namespace VeilofDeath.Core.GameStates
{
    enum Button
    {
        Start,
        Settings,
        Statistics,
        Credits,
        Exit,

        Count
    }
    class MainMenu : IGameState
    {
        SpriteBatch spriteBatch;
        bool ispressed = false;
        private bool isEnterDown = true;

        private Texture2D background;

        public EState newState { get; set; }

        public bool canLeave { get; set; }

        private Texture2D[] Buttons;

        private Texture2D[] SelectedButtons;

        private Button m_selected;

        private bool showQuestion;
        private bool isNewGame = true;

        Panel Panel;
        PanelElement yes, no;

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
            //TODO: Rearragne the TitleMenu
            /*
             -> Start Game
             -> Settings
             -> Statistics
             -> Credits
             -> Quit
             */

            background = GameConstants.Content.Load<Texture2D>("titleBG");
            Buttons = new Texture2D[(int)Button.Count];
            Buttons[(int)Button.Start] = GameConstants.Content.Load<Texture2D>("Textures/StartB");
            Buttons[(int)Button.Settings] = GameConstants.Content.Load<Texture2D>("Textures/SettingsB");
            Buttons[(int)Button.Statistics] = GameConstants.Content.Load<Texture2D>("Textures/StatsB");
            Buttons[(int)Button.Credits] = GameConstants.Content.Load<Texture2D>("Textures/CreditsB");
            Buttons[(int)Button.Exit] = GameConstants.Content.Load<Texture2D>("Textures/ExiB");

            //more buttons
            SelectedButtons = new Texture2D[(int)Button.Count];
            SelectedButtons[(int)Button.Start] = GameConstants.Content.Load<Texture2D>("Textures/StartBS");
            SelectedButtons[(int)Button.Credits] = GameConstants.Content.Load<Texture2D>("Textures/CreditsBS");
            SelectedButtons[(int)Button.Settings] = GameConstants.Content.Load<Texture2D>("Textures/SettingsBS");
            SelectedButtons[(int)Button.Exit] = GameConstants.Content.Load<Texture2D>("Textures/ExiBS");
            SelectedButtons[(int)Button.Statistics] = GameConstants.Content.Load<Texture2D>("Textures/StatsBS");

            //Load other Textures, like Buttons
            LoadQuestionPanel();
            //Sounds for buttons
            
        }

        private void LoadQuestionPanel()
        {
            Panel = new Panel(GameConstants.Content.Load<Texture2D>("Panels/panel"),
                                    new Vector2(0.1f * GameConstants.WINDOWSIZE.X,
                                                0.1f * GameConstants.WINDOWSIZE.Y));

            yes = new PanelElement("yes", GameConstants.lucidaConsole, Color.Red, true);
            no = new PanelElement("no", GameConstants.lucidaConsole, Color.Red, false);

            Panel.Add(new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/newGame")), new Vector2(0.08f,0.18f));
            Panel.Add(new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/button")), new Vector2(0.2f,0.75f));
            Panel.Add(new PanelElement("yes", GameConstants.lucidaConsole, Color.White), new Vector2(0.25f,0.75f));
            Panel.Add(yes, new Vector2(0.25f, 0.75f));
            Panel.Add(new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/button")), new Vector2(0.60f, 0.75f));
            Panel.Add(new PanelElement("no", GameConstants.lucidaConsole, Color.White), new Vector2(0.67f, 0.75f));
            Panel.Add(no, new Vector2(0.67f, 0.75f));
        }

        public void UnloadContent()
        {
            
        }

        public void Update(GameTime time)
        {

            if (showQuestion)
            {
                UpdateQuestion();
            }
            else
            {
                UpdateMainMenu();
            }
            //ParticleTest.Update(time);

        }

        private void UpdateQuestion()
        {
            if (!ispressed && Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                yes.isActive = !yes.isActive;
                no.isActive = !no.isActive;
                ispressed = true;
                GameConstants.Switch.Play();//LARS: Play sound: switch menu selection
            }

            if (!ispressed && Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                yes.isActive = !yes.isActive;
                no.isActive = !no.isActive;
                ispressed = true;
                GameConstants.Switch.Play();//LARS: Play sound: switch menu selection
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Left) && Keyboard.GetState().IsKeyUp(Keys.Right))
                ispressed = false;

            if (!isEnterDown && Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                isEnterDown = true;
                GameConstants.Select.Play();//LARS: Play sound: Menüpunkt bestätigen
                if (yes.isActive)
                {
                    GameManager.Instance.FlushStats();
                    GameManager.Instance.ResetToLevel0();
                    newState = EState.Ingame;
                    canLeave = true;
                }
                else
                    showQuestion = false;

            }

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                showQuestion = false;

            if (Keyboard.GetState().IsKeyUp(Keys.Enter))
                isEnterDown = false;

        }

        private void UpdateMainMenu()
        {
            if (!ispressed && Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                m_selected = (Button)(((int)m_selected + 1) % (int)Button.Count);
                ispressed = true;
                GameConstants.Switch.Play();//LARS: Play sound: switch menu selection
            }

            if (!ispressed && Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                m_selected = (Button)(((int)m_selected + (int)Button.Count - 1) % (int)Button.Count);
                ispressed = true;
                GameConstants.Switch.Play();//LARS: Play sound: switch menu selection
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Down) && Keyboard.GetState().IsKeyUp(Keys.Up))
                ispressed = false;

            if (!isEnterDown && Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                isEnterDown = true;
                GameConstants.Select.Play();//LARS: Play sound: Menüpunkt bestätigen
                switch (m_selected)
                {
                    case Button.Start:
                        if (GameConstants.hasGameWon)
                        {
                            showQuestion = true;
                            isNewGame = false;
                        }
                        if (isNewGame)
                        {
                            //TODO: Story1 einfügen
                            //if GameManager.Instance.Level == 0
                            //  newState = EState.Story(1)
                            newState = EState.Ingame;
                            canLeave = true;
                        }

                        break;
                    case Button.Exit:
                        GameConstants.currentGame.Exit();
                        break;
                    case Button.Settings:
                        newState = EState.Settings;
                        canLeave = true;
                        break;
                    case Button.Statistics:
                        newState = EState.Statistics;
                        canLeave = true;
                        break;
                    case Button.Credits:
                        newState = EState.Credits;
                        canLeave = true;
                        break;
                    default:
                        newState = EState.none;
                        break;
                }
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Enter))
                isEnterDown = false;
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

            if (showQuestion)
                Panel.Draw(spriteBatch);

            //ParticleTest.Draw(spriteBatch);

            spriteBatch.End();
        }
    }
}
