using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VeilofDeath.PanelStuff;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

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
        private bool enterMenu = false;

        private float volumeterPos;

        private float priorVolume;

        Panel DifficultyPanel,VolumePanel,ControlsPanel,MainPanel;
        //Panelelements for MainPanel;
        PanelElement ptSettings, ptDiff, ptSound, ptCon, ptDiff2, ptSound2, ptCon2, ptMBack, peCursor;
        //PanelElements for all Panels
        PanelElement ptConfirm, peEnter, ptBack, peEsc;
        // PanelElements for DifficultyPanel
        PanelElement ptHeader, ptEasy, ptMedium, ptHard, ptExtreme,ptEasy2, ptMedium2, ptHard2, ptExtreme2;
        //PanelElements for VolumePanel
        PanelElement ptHeader2, ptVolume, ptPlus,ptMinus, pePlusMinus, peLine, peMarker;
        //PanelElements for ControlsPanel
        PanelElement ptHeader3, ptSpace, peSpace, ptShift, peShift, ptLR, peLR;
                     

        SettingState State;
        Difficulty DiffMode;

        private bool ispressed = false;
        private bool isEnterDown = false;

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
            volumeterPos = GetVolPos(GameConstants.Volume);
            enterMenu = false;
            priorVolume = GameConstants.Volume;
        }

        private float GetVolPos(float v)
        {
            return (v * 0.72f) + 0.1f;
        }

        private float GetVolReal(float x)
        {
            return (x - 0.1f) / 0.72f;
        }

        #region LoadContent

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
            MainPanel = new Panel(GameConstants.Content.Load<Texture2D>("Panels/menu/MenuBG"),new Vector2(0,0));
            // ptDiff, ptCol, ptCon, peCursor;

            ptSettings = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/void"), true);
            ptSettings.AddText(GameConstants.lucidaConsole, "SETTINGS", Color.White, 1f);
            ptDiff = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/void"), true);
            ptDiff.AddText(GameConstants.lucidaConsole, "Difficulty", Color.White, 2.2f);
            ptSound = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/void"), true);
            ptSound.AddText(GameConstants.lucidaConsole, "Sound", Color.White, 2.2f);
            ptCon = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/void"), true);
            ptCon.AddText(GameConstants.lucidaConsole, "Controls", Color.White, 2.2f);
            ptMBack = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/void"), true);
            ptMBack.AddText(GameConstants.lucidaConsole, "Back", Color.White);
            ptDiff2 = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/void"), false);
            ptDiff2.AddText(GameConstants.lucidaConsole, "Difficulty", Color.Red, 2.2f);
            ptSound2 = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/void"), false);
            ptSound2.AddText(GameConstants.lucidaConsole, "Sound", Color.Red, 2.2f);
            ptCon2 = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/void"), false);
            ptCon2.AddText(GameConstants.lucidaConsole, "Controls", Color.Red, 2.2f);
            peCursor = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/menu/cursor"), true);

            MainPanel.Add(ptSettings, new Vector2(0.05f, 0.05f));
            MainPanel.Add(ptDiff, new Vector2(0.1f, 0.3f));
            MainPanel.Add(ptSound, new Vector2(0.1f, 0.45f));
            MainPanel.Add(ptCon, new Vector2(0.1f, 0.6f));
            MainPanel.Add(ptDiff2, new Vector2(0.1f, 0.3f));
            MainPanel.Add(ptSound2, new Vector2(0.1f, 0.45f));
            MainPanel.Add(ptCon2, new Vector2(0.1f, 0.6f));
            MainPanel.Add(ptMBack, new Vector2(0.1f, 0.75f));
            // Can be 0.31 ; 0,46; 0,61; 0,76
            MainPanel.Add(peCursor, new Vector2(0.05f, 0.31f));
        }

        private void LoadVolumePanel()
        {
            VolumePanel = new Panel(GameConstants.Content.Load<Texture2D>("Panels/menu/panelMenu"),
                                                new Vector2(0.35f * GameConstants.WINDOWSIZE.X,
                                                            0.1f * GameConstants.WINDOWSIZE.Y));

            // ptHeader2, ptVolume, ptPlus, ptMinus, pePlus, peMinus, peLine, peMarker
            ptHeader2 = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/void"), true);
            ptHeader2.AddText(GameConstants.lucidaConsole, "Sound", Color.White);
            peEnter = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/menu/Enter"), true);
            peEsc = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/menu/esc"), true);
            ptBack = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/void"), true); // Text: number of Coins
            ptBack.AddText(GameConstants.lucidaConsole, "Back", Color.White);
            ptConfirm = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/void"), true);
            ptConfirm.AddText(GameConstants.lucidaConsole, "Confirm", Color.White);

            ptVolume = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/void"), true);
            ptVolume.AddText(GameConstants.lucidaConsole, "Volume", Color.White, 2.2f);
            ptPlus = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/void"), true);
            ptPlus.AddText(GameConstants.lucidaConsole, "++", Color.White);
            ptMinus = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/void"), true);
            ptMinus.AddText(GameConstants.lucidaConsole, "--", Color.White);
            pePlusMinus = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/menu/leftright-small"), true);
            peLine = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/menu/volume"), true);
            peMarker = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/menu/volumeter"), true);

            VolumePanel.Add(ptHeader2, new Vector2(0.1f, 0.1f));
            VolumePanel.Add(peEnter, new Vector2(0.13f, 0.8f));
            VolumePanel.Add(ptConfirm, new Vector2(0.25f, 0.8f));
            VolumePanel.Add(peEsc, new Vector2(0.7f, 0.8f));
            VolumePanel.Add(ptBack, new Vector2(0.82f, 0.8f));
            VolumePanel.Add(ptVolume, new Vector2(0.42f, 0.2f));
            VolumePanel.Add(ptPlus, new Vector2(0.55f, 0.33f));
            VolumePanel.Add(ptMinus, new Vector2(0.4f, 0.33f));
            VolumePanel.Add(pePlusMinus, new Vector2(0.4f, 0.38f));
            VolumePanel.Add(peLine, new Vector2(0.1f, 0.55f));
            // X in Range (0.1 - 0.82)
            VolumePanel.Add(peMarker, new Vector2(0.1f, 0.55f));

            peMarker.UpdatePositionX(volumeterPos);

        }

        private void LoadControlsPanel()
        {
            ControlsPanel = new Panel(GameConstants.Content.Load<Texture2D>("Panels/menu/panelMenu"),
                                                new Vector2(0.35f * GameConstants.WINDOWSIZE.X,
                                                            0.1f * GameConstants.WINDOWSIZE.Y));

            ptHeader3 = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/menu/controlscomplete"),true);
            //TODO: mit den einzelnen Elementen umsetzen, hier aus Faulheit ein großes .png für die Controls


            ControlsPanel.Add(ptHeader3, new Vector2(0.1f, 0.1f));
        }

        private void LoadDifficultyPanel()
        {
            DifficultyPanel = new Panel(GameConstants.Content.Load<Texture2D>("Panels/menu/panelMenu"),
                                                new Vector2(0.35f * GameConstants.WINDOWSIZE.X,
                                                            0.1f * GameConstants.WINDOWSIZE.Y));


            ptHeader = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/void"), true);
            ptHeader.AddText(GameConstants.lucidaConsole, "Difficulty", Color.White);
            peEnter = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/menu/Enter"), true);
            peEsc = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/menu/esc"), true);
            ptBack = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/void"), true); // Text: number of Coins
            ptBack.AddText(GameConstants.lucidaConsole, "Back", Color.White);
            ptConfirm = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/void"), true);
            ptConfirm.AddText(GameConstants.lucidaConsole, "Confirm", Color.White);

            ptEasy = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/void"), true);
            ptEasy.AddText(GameConstants.lucidaConsole, "easy", Color.White, 2.2f);
            ptExtreme = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/void"), true);
            ptExtreme.AddText(GameConstants.lucidaConsole, "extreme", Color.Violet, 2.2f);
            ptHard = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/void"), true);
            ptHard.AddText(GameConstants.lucidaConsole, "hard", Color.White, 2.2f);
            ptMedium = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/void"), true);
            ptMedium.AddText(GameConstants.lucidaConsole, "medium", Color.White);
            ptEasy2 = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/void"), false);
            ptEasy2.AddText(GameConstants.lucidaConsole, "easy", Color.Red, 2.2f);
            ptExtreme2 = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/void"), false);
            ptExtreme2.AddText(GameConstants.lucidaConsole, "extreme", Color.Red, 2.2f);
            ptHard2 = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/void"), false);
            ptHard2.AddText(GameConstants.lucidaConsole, "hard", Color.Red, 2.2f);
            ptMedium2 = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/void"), false);
            ptMedium2.AddText(GameConstants.lucidaConsole, "medium", Color.Red);

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

        #endregion
        
        #region Update

        public void Update(GameTime time)
        {

            if (enterMenu)
                switch (State)
                {
                    case SettingState.Difficulty:
                        ptDiff2.isActive = true;
                        ptCon2.isActive = false;
                        ptSound2.isActive = false;
                        UpdateDifficulty();
                        break;
                    case SettingState.Volume:
                        ptDiff2.isActive = false;
                        ptCon2.isActive = false;
                        ptSound2.isActive = true;
                        UpdateVolume();
                        break;
                    case SettingState.Controls:
                        ptDiff2.isActive = false;
                        ptCon2.isActive = true;
                        ptSound2.isActive = false;
                        UpdateControls();
                        break;
                    case SettingState.back:
                        newState = EState.MainMenu;
                        canLeave = true;
                        break;
                }
            else
            {
                ptDiff2.isActive = false;
                ptCon2.isActive = false;
                ptSound2.isActive = false;

                if (!ispressed && Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    State = (SettingState) Math.Min((int)State + 1, 3);
                    UpdateCursor();
                    GameConstants.Switch.Play();
                    ispressed = true;
                }
                if (!ispressed && Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    State = (SettingState) Math.Max((int)State - 1, 0);
                    UpdateCursor();
                    GameConstants.Switch.Play();
                    ispressed = true;
                }

                if (Keyboard.GetState().IsKeyUp(Keys.Down) && Keyboard.GetState().IsKeyUp(Keys.Up) && Keyboard.GetState().IsKeyUp(Keys.Escape))
                    ispressed = false;

                if (!isEnterDown && Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    GameConstants.Select.Play();
                    enterMenu = true;
                    isEnterDown = true;
                }

                if (Keyboard.GetState().IsKeyUp(Keys.Enter))
                    isEnterDown = false;

                if (!ispressed && Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    ispressed = true;
                    newState = EState.MainMenu;
                    canLeave = true;
                }

            }



        }

        private void UpdateCursor()
        {
            switch (State)
            {
                case SettingState.Difficulty:
                    peCursor.UpdatePositionY( 0.31f);
                    break;
                case SettingState.Volume:
                    peCursor.UpdatePositionY( 0.46f);
                    break;
                case SettingState.Controls:
                    peCursor.UpdatePositionY( 0.61f);
                    break;
                case SettingState.back:
                    peCursor.UpdatePositionY( 0.75f);
                    break;
            }
        }

        private void UpdateControls()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                ispressed = true;
                enterMenu = false;
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Escape))
                ispressed = false;

        }

        private void UpdateVolume()
        {
            peMarker.UpdatePositionX(volumeterPos);

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                volumeterPos = Math.Max(0.1f, volumeterPos - 0.01f);
                GameConstants.Volume = GetVolReal(volumeterPos);
                SoundEffect.MasterVolume = GameConstants.Volume;
                ispressed = true;
                GameConstants.Switch.Play();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                volumeterPos = Math.Min(volumeterPos + 0.01f, 0.82f);
                GameConstants.Volume = GetVolReal(volumeterPos);
                SoundEffect.MasterVolume = GameConstants.Volume;
                ispressed = true;
                GameConstants.Switch.Play();
            }


            if (!isEnterDown && Keyboard.GetState().IsKeyDown(Keys.Enter))
            {                
                GameConstants.Volume = GetVolReal(volumeterPos);
                SoundEffect.MasterVolume = GameConstants.Volume;
                isEnterDown = true;
                enterMenu = false;
                GameConstants.Select.Play();
                GameManager.Instance.Save();
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Escape))
                ispressed = false;

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                GameConstants.Volume = priorVolume;
                volumeterPos = GetVolPos(GameConstants.Volume);
                SoundEffect.MasterVolume = GameConstants.Volume;
                enterMenu = false;
                ispressed = true;
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Enter))
                isEnterDown = false;
        }

        private void UpdateDifficulty()
        {
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

            if (!ispressed && Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                DiffMode = (Difficulty) Math.Min((int)DiffMode + 1, 3);
                GameConstants.Switch.Play();
                ispressed = true;
            }
            if (!ispressed && Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                DiffMode = (Difficulty) Math.Max((int)DiffMode - 1, 0);
                GameConstants.Switch.Play();
                ispressed = true;
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Down) && Keyboard.GetState().IsKeyUp(Keys.Up) && Keyboard.GetState().IsKeyUp(Keys.Escape))
                ispressed = false;

            if (!isEnterDown && Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                GameConstants.iDifficulty =  Math.Max(1, (int)DiffMode + 1);
                if (GameConstants.isDebugMode)
                    Console.WriteLine("Difficulty: "+ GameConstants.iDifficulty);
                enterMenu = false;
                isEnterDown = true;
                ptEasy2.isActive = false;
                ptMedium2.isActive = false;
                ptHard2.isActive = false;
                ptExtreme2.isActive = false;
                GameConstants.Select.Play();
                GameManager.Instance.Save();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                DiffMode = (Difficulty) Math.Max(0, GameConstants.iDifficulty - 1);
                if (GameConstants.isDebugMode)
                    Console.WriteLine("Difficulty: " + GameConstants.iDifficulty);
                enterMenu = false;
                ptEasy2.isActive = false;
                ptMedium2.isActive = false;
                ptHard2.isActive = false;
                ptExtreme2.isActive = false;
                ispressed = true;
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Enter))
                isEnterDown = false;

        }

        #endregion

        public void Draw(GameTime time)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(background, new Rectangle(0, 0, (int)GameConstants.WINDOWSIZE.X, (int)GameConstants.WINDOWSIZE.Y), Color.White);

            MainPanel.Draw(spriteBatch);

            switch (State)
            {
                case SettingState.Difficulty:
                    DifficultyPanel.Draw(spriteBatch);
                    break;
                case SettingState.Volume:
                    VolumePanel.Draw(spriteBatch);
                    break;
                case SettingState.Controls:
                    ControlsPanel.Draw(spriteBatch);
                    break;
            }

            spriteBatch.End();
        }

    }
}
