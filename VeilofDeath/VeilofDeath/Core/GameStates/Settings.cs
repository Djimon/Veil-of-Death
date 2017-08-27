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
    enum SettingState
    {
        Difficulty,
        Volume,
        Controls,
        back
    }

    enum Difficulty
    {
        easy,
        medium,
        hard,
        extrem
    }

    class Settings : IGameState
    {
        Texture2D background;
        private SpriteBatch spriteBatch;

        Panel DifficultyPanel,VolumePanel,ControlsPanel;
        PanelElement ptHeader,
                        ptEasy,
                        ptMedium,
                        ptHard,
                        ptExtreme,
                        ptEasy2,
                        ptMedium2,
                        ptHard2,
                        ptExtreme2,
                        ptWarning,
                        ptConfirm,
                        peEnter,
                        ptBack,
                        peEsc;

        SettingState State;
        Difficulty DiffMode;

        private bool ispressed = false;

        public bool canLeave { get; set; }

        public EState newState { get; set;}

        public Settings()
        {
            spriteBatch = GameConstants.SpriteBatch;
            Initialize();
            LoadContent();
        }
      
        public void Initialize()
        {
            newState = EState.none;
            State = SettingState.Difficulty;
            DiffMode = (Difficulty) Math.Max(0,GameConstants.iDifficulty - 1);
        }

        public void LoadContent()
        {
            background = GameConstants.Content.Load<Texture2D>("Menu/BGmenu");
            LoadSubmenu();
            LoadDifficultyPanel();
            LoadVolumePanel();
            LoadControlsPanel();

        }

        private void LoadSubmenu()
        {
            //TODO: Continue here
            /*
             -> Difficulty
             -> Volume
             -> Controls

             -> back
                          
             */
        }

        private void LoadVolumePanel()
        {
            VolumePanel = new Panel(GameConstants.Content.Load<Texture2D>("Panels/menu/panelMenu"),
                                                new Vector2(0.35f * GameConstants.WINDOWSIZE.X,
                                                            0.1f * GameConstants.WINDOWSIZE.Y));
        }

        private void LoadControlsPanel()
        {
            ControlsPanel = new Panel(GameConstants.Content.Load<Texture2D>("Panels/menu/panelMenu"),
                                                new Vector2(0.35f * GameConstants.WINDOWSIZE.X,
                                                            0.1f * GameConstants.WINDOWSIZE.Y));
        }

        private void LoadDifficultyPanel()
        {
            DifficultyPanel = new Panel(GameConstants.Content.Load<Texture2D>("Panels/menu/panelMenu"),
                                                new Vector2(0.35f * GameConstants.WINDOWSIZE.X,
                                                            0.1f * GameConstants.WINDOWSIZE.Y));


            ptHeader = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/void"), true);
            ptHeader.AddText(GameConstants.lucidaConsole, "Back", Color.White);
            peEnter = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/menu/Enter"), true);
            peEsc = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/menu/esc"), true);
            ptBack = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/void"), true); // Text: number of Coins
            ptBack.AddText(GameConstants.lucidaConsole, "Back", Color.White);
            ptConfirm = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/void"), true);
            ptConfirm.AddText(GameConstants.lucidaConsole, "Back", Color.White);
            ptEasy = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/void"), false);
            ptEasy.AddText(GameConstants.lucidaConsole, "Back", Color.White, 2.2f);
            ptExtreme = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/void"), false);
            ptExtreme.AddText(GameConstants.lucidaConsole, "Back", Color.Violet, 2.2f);
            ptHard = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/void"), false);
            ptHard.AddText(GameConstants.lucidaConsole, "Back", Color.White, 2.2f);
            ptMedium = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/void"), false);
            ptMedium.AddText(GameConstants.lucidaConsole, "Back", Color.White);
            ptEasy2 = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/void"), false);
            ptEasy2.AddText(GameConstants.lucidaConsole, "Back", Color.Red, 2.2f);
            ptExtreme2 = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/void"), false);
            ptExtreme2.AddText(GameConstants.lucidaConsole, "Back", Color.Red, 2.2f);
            ptHard2 = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/void"), false);
            ptHard2.AddText(GameConstants.lucidaConsole, "Back", Color.Red, 2.2f);
            ptMedium2 = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/void"), false);
            ptMedium2.AddText(GameConstants.lucidaConsole, "Back", Color.Red);

            DifficultyPanel.Add(ptHeader, new Vector2(0.1f, 0.1f));
            DifficultyPanel.Add(peEnter, new Vector2(0.13f, 0.8f));
            DifficultyPanel.Add(ptConfirm, new Vector2(0.25f, 0.8f));
            DifficultyPanel.Add(peEsc, new Vector2(0.7f, 0.8f));
            DifficultyPanel.Add(ptBack, new Vector2(0.82f, 0.8f));
            DifficultyPanel.Add(ptEasy, new Vector2(0.4f, 0.2f));
            DifficultyPanel.Add(ptEasy2, new Vector2(0.4f, 0.2f));
            DifficultyPanel.Add(ptMedium, new Vector2(0.4f, 0.34f));
            DifficultyPanel.Add(ptMedium2, new Vector2(0.4f, 0.34f));
            DifficultyPanel.Add(ptHard, new Vector2(0.4f, 0.48f));
            DifficultyPanel.Add(ptHard2, new Vector2(0.4f, 0.48f));
            DifficultyPanel.Add(ptExtreme, new Vector2(0.4f, 0.62f));
            DifficultyPanel.Add(ptExtreme2, new Vector2(0.4f, 0.62f));
        }

        public void UnloadContent()
        {
            
        }

        public void Update(GameTime time)
        {
            switch (State)
            {
                case SettingState.Difficulty:
                    UpdateDifficulty();
                    break;
                case SettingState.Volume:
                    UpdateVolume();
                    break;
                case SettingState.Controls:
                    UpdateControls();
                    break;
                case SettingState.back:
                    newState = EState.MainMenu;
                    canLeave = true;
                    break;
            }
            //Pfeiltasten hoch runter


        }

        private void UpdateControls()
        {
            throw new NotImplementedException();
        }

        private void UpdateVolume()
        {
            throw new NotImplementedException();
        }

        private void UpdateDifficulty()
        {
            if (!ispressed && Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                DiffMode = (Difficulty) Math.Min((int)DiffMode+1, 3);
                ispressed = true;
            }
            if (!ispressed && Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                DiffMode = (Difficulty)Math.Max((int)DiffMode - 1, 0);
                ispressed = true;
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Down) && Keyboard.GetState().IsKeyUp(Keys.Down))
                ispressed = false;

            if (Keyboard.GetState().IsKeyUp(Keys.Enter))
            {
                GameConstants.iDifficulty =  Math.Min(1, (int)DiffMode + 1);
            }
            if (Keyboard.GetState().IsKeyUp(Keys.Escape))
            {
                DiffMode = (Difficulty) Math.Max(0, GameConstants.iDifficulty - 1);
            }


            switch (DiffMode)
            {
                case Difficulty.easy:
                    ptEasy2.isActive = true;
                    ptMedium2.isActive = false;
                    ptHard2.isActive = false;
                    ptExtreme2.isActive = false;
                    break;
                case Difficulty.medium:
                    ptEasy2.isActive = false;
                    ptMedium2.isActive = true;
                    ptHard2.isActive = false;
                    ptExtreme2.isActive = false;
                    break;
                case Difficulty.hard:
                    ptEasy2.isActive = false;
                    ptMedium2.isActive = false;
                    ptHard2.isActive = true;
                    ptExtreme2.isActive = false;
                    break;
                case Difficulty.extrem:
                    ptEasy2.isActive = false;
                    ptMedium2.isActive = false;
                    ptHard2.isActive = false;
                    ptExtreme2.isActive = true;
                    break;
            }



        }

        public void Draw(GameTime time)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(background, new Rectangle(0, 0, (int)GameConstants.WINDOWSIZE.X, (int)GameConstants.WINDOWSIZE.Y), Color.White);

            switch (State)
            {
                case SettingState.Difficulty:
                    DifficultyPanel.Draw(spriteBatch);
                    break;
                case SettingState.Volume:
                    //VolumePanel.Draw(spriteBatch);
                    break;
                case SettingState.Controls:
                    //ControlsPanel.Draw(spriteBatch);
                    break;
            }

            spriteBatch.End();
        }


    }
}
