using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using VeilofDeath.Core;
using VeilofDeath.Core.GameStates;
using Microsoft.Xna.Framework.Audio;

namespace VeilofDeath.Core
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Stack<IGameState> gameStates = new Stack<IGameState>();

        Vector3 lightDirection = new Vector3(3, -2, 5);

        IGameState currentState;

        /// <summary>
        /// Main Game
        /// </summary>
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
      
            Content.RootDirectory = "Content";
            GameConstants.Content = Content;

            GameConstants.fAspectRatio = (float)GraphicsDeviceManager.DefaultBackBufferWidth / GraphicsDeviceManager.DefaultBackBufferHeight;

            GameConstants.currentGame = this;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            GameManager.Instance.Load();
            graphics.PreferredBackBufferWidth = (int)GameConstants.WINDOWSIZE.X;
            graphics.PreferredBackBufferHeight = (int)GameConstants.WINDOWSIZE.Y;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Window.Title = "Veil of Death (1.001)";
            lightDirection.Normalize();

            GameConstants.MainCam = new Camera();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            GameConstants.SpriteBatch = spriteBatch;
            GameConstants.Graphics = graphics;
            GameConstants.lucidaConsole = GameConstants.Content.Load<SpriteFont>("Fonts/Lucida Console");

            gameStates.Clear();
            gameStates.Push(new MainMenu());
            

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            GameConstants.CoinCollect = GameConstants.Content.Load<SoundEffect>("Music/SoundEffects/PickupCoin");
            GameConstants.Landing = GameConstants.Content.Load<SoundEffect>("Music/SoundEffects/LandAfterJump");
            GameConstants.CharactersJump = GameConstants.Content.Load<SoundEffect>("Music/SoundEffects/JumpHupHuman");
            GameConstants.HeartBeat = GameConstants.Content.Load<SoundEffect>("Music/SoundEffects/HBeat");
            GameConstants.ChangePhase = GameConstants.Content.Load<SoundEffect>("Music/SoundEffects/Roaring");
            GameConstants.Winner = GameConstants.Content.Load<SoundEffect>("Music/SoundEffects/Winner");
            GameConstants.TotalWinner = GameConstants.Content.Load<SoundEffect>("Music/SoundEffects/TotalWinner");
            GameConstants.Loser = GameConstants.Content.Load<SoundEffect>("Music/SoundEffects/Loser");
            GameConstants.Select = GameConstants.Content.Load<SoundEffect>("Music/SoundEffects/Select");
            GameConstants.Switch = GameConstants.Content.Load<SoundEffect>("Music/SoundEffects/Switch");
            GameConstants.SpikeRoll = GameConstants.Content.Load<SoundEffect>("Music/SoundEffects/SpikeRoll");
            GameConstants.SpikeTrap = GameConstants.Content.Load<SoundEffect>("Music/SoundEffects/SpikeTrap");
            GameConstants.SpeedUp = GameConstants.Content.Load<SoundEffect>("Music/SoundEffects/SpeedUp");
            GameConstants.SlowTrap = GameConstants.Content.Load<SoundEffect>("Music/SoundEffects/SlowTrap");
            GameConstants.Slide = GameConstants.Content.Load<SoundEffect>("Music/SoundEffects/Slide");
            GameConstants.EvilLaugh = GameConstants.Content.Load<SoundEffect>("Music/SoundEffects/EvilLaugh");
            GameConstants.AtmosphereSound = GameConstants.Content.Load<SoundEffect>("Music/SoundEffects/AtmosphereSound");

            GameConstants.Sounds.Add(GameConstants.CoinCollect);
            GameConstants.Sounds.Add(GameConstants.Landing);
            GameConstants.Sounds.Add(GameConstants.CharactersJump);
            GameConstants.Sounds.Add(GameConstants.HeartBeat);
            GameConstants.Sounds.Add(GameConstants.ChangePhase);
            GameConstants.Sounds.Add(GameConstants.Winner);
            GameConstants.Sounds.Add(GameConstants.TotalWinner);
            GameConstants.Sounds.Add(GameConstants.Loser);
            GameConstants.Sounds.Add(GameConstants.Select);
            GameConstants.Sounds.Add(GameConstants.Switch);
            GameConstants.Sounds.Add(GameConstants.SpikeRoll);
            GameConstants.Sounds.Add(GameConstants.SpikeTrap);
            GameConstants.Sounds.Add(GameConstants.SpeedUp);
            GameConstants.Sounds.Add(GameConstants.SlowTrap);
            GameConstants.Sounds.Add(GameConstants.Slide);
            GameConstants.Sounds.Add(GameConstants.EvilLaugh);
            GameConstants.Sounds.Add(GameConstants.AtmosphereSound);

            SoundEffect.MasterVolume = GameConstants.Volume;

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.F4))
                Exit();

            currentState = gameStates.Peek();

            currentState.Update(gameTime);
            GameConstants.MainCam.Update(gameTime);                           

            EState nextState = currentState.newState;
            if (currentState.canLeave)
                gameStates.Pop();

            if (nextState != EState.none)
            {
                currentState.newState = nextState;
                gameStates.Push(FetchGameState(nextState));
            }
            if (gameStates.Count == 0)
                Exit(); //DoubleCheck is saver
       
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.Black);
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            gameStates.Peek().Draw(gameTime);


            base.Draw(gameTime);
        }

        /// <summary>
        /// Decodes the EState integer value to the correct GameState and returns it.
        /// </summary>
        /// <param name="state">GameState Name(Enum)</param>
        /// <returns>IGameState</returns>
        private IGameState FetchGameState(EState state)
        {
            switch (state)
            {
                case EState.none:
                    Exit(); //Exception
                    return null;
                /*
                case EState.Start: //Boot-Screen
                    break;
                */
                case EState.MainMenu:  //Hauptmenü
                    return new MainMenu();
                case EState.Settings: //Bedienung
                    return new Settings();
                case EState.Statistics: //Stats
                    return new Statistics();             
                case EState.Ingame: //new Level
                    GameManager.Instance.ResetLevel();
                    return new InGame(GameManager.Instance.Level);
                case EState.Story:
                    return new Story(GameManager.Instance.Level);              
                case EState.Score: //Score
                    return new Score();                   
                case EState.GameOver: //Spielende
                    return new GameOver(GameConstants.iWinStatus);                
                case EState.Credits: //Credits
                    return new Credits();                        
                default: return null;
            }
        }
    }

    //TODO: Door Red ist kaputt
    //TODO: Mehr Content einbauen
    //TODO: Eure Collisionsboxen sind sehr merwürdig positioniert, sodass man erst sehr spät springen sollte, dass mindert den Spielspaß
    //TODO: Im Menü kann man den Schwierigkeitsgrad einstellen aber es wird nciht der ausgewählte State gespeichert.
    //TODO: Warum stirbt man in den späteren Level wenn man jeden die Wand läuft? - Ist das gewollt?
    //TODO: Manchmal wird kein Sound im Level abgespielt.


}
