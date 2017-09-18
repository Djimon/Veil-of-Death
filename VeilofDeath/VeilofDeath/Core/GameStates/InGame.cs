using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Drawing;
using VeilofDeath.Objects;
using Animations;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using VeilofDeath.SpecialFX;
using VeilofDeath.PanelStuff;
using VeilofDeath.Objects.Traps;
using VeilofDeath.Level;
using VeilofDeath.Objects.PlayerStuff;

namespace VeilofDeath.Core.GameStates
{
    class InGame : IGameState
    {
        #region member variablen

        /// <summary>
        /// newState defines in which GameState to go next
        /// Must be initialized to "EState.none"!
        /// </summary>
        public EState newState { get; set; }

        public bool canLeave { get; set; }

        public KeyboardState currentKeyboardState { get; private set; }
        public KeyboardState oldKeyboardState { get; private set; }

        public TrapHandler TrapHandler;

        public SpriteBatch spriteBatch;

        int iLevel;

        Bitmap levelMask;
        public Map testmap;

        Player Player;
        PlayerController PController;
        AnimatedModel m_player;

        private Veil VeilofDeath;

        Vector2 GUI_Pos = new Vector2(1000, 20); 
        Vector2 GUI_Stuff = new Vector2(100, 400); 

        Texture2D txLine, txPlayer;
        Texture2D[] txVeil;

        private Vector3 start;

        float fTimeDelta;

        private Texture2D shadeLayer;

        #endregion


        Spawner objectSpawner;
        private float timesincelastupdate;
        AParticleEnginge VeilParticles;

        private Vector2 playerTexPos = new Vector2(0, 0);
        private Vector2 VeilTexPos = new Vector2(0, 0);
        private float playeTexStart;
        private float veilTexStart;

        private Song music;
        private int MaxCoins;
        private bool isJumpCalculated;
        private float t0;
        private bool isSetTime = false;
        private bool isStarted = false;

        Panel MainPanel;
        PanelElement peRdy, // Read?
                        pePressJump, // press jump to start
                        pePressJump2, // press jump anim state 2
                        ptRun, // description controls
                        peRunA, // Controlls animationstate 1
                        peRunB, // COntrools animationstate 2
                        ptJump, // description jump
                        peJumpA, // Jump anim state 1
                        peJumpB; // jump anim state 2


        public InGame(int Level)
        {
            spriteBatch = GameConstants.SpriteBatch;
            iLevel = Level;

            Initialize();
            LoadContent();
        }

        #region initialize, load content

        public void Initialize()
        {
            newState = EState.none;
            currentKeyboardState = Keyboard.GetState();
            oldKeyboardState = new KeyboardState();
            GameConstants.MainCam.ResetCamera();
            GameConstants.fMovingSpeed = 1.5f + (0.25f * GameConstants.iDifficulty);
            GameConstants.fSpeedTime = 350 - (50 * GameConstants.iDifficulty);

            if (GameManager.Instance.Level < 2)
                GameConstants.levelDictionary = LevelContent.LoadListContent<Model>(GameConstants.Content, "Models/Ebene0");
            else if (GameManager.Instance.Level >= 2 && GameManager.Instance.Level < 4)
                GameConstants.levelDictionary = LevelContent.LoadListContent<Model>(GameConstants.Content, "Models/Ebene1");
            else if (GameManager.Instance.Level >= 4)
                GameConstants.levelDictionary = LevelContent.LoadListContent<Model>(GameConstants.Content, "Models/Ebene2");


            foreach (KeyValuePair<string, Model> SM in GameConstants.levelDictionary)
            {
                if (GameConstants.isDebugMode)
                    Console.WriteLine("Key:" + SM.Key + ", Value: " + SM.Value);
            }

            txVeil = new Texture2D[3];
            txLine = GameConstants.Content.Load<Texture2D>("GUI/line");
            txPlayer = GameConstants.Content.Load<Texture2D>("GUI/playericon");
            txVeil[0] = GameConstants.Content.Load<Texture2D>("GUI/veil_1");
            txVeil[1] = GameConstants.Content.Load<Texture2D>("GUI/veil_2");
            txVeil[2] = GameConstants.Content.Load<Texture2D>("GUI/veil_3");

            CalculateGUIPositions();

            GameConstants.PointLightLayer = GameConstants.Content.Load<Effect>("FX/PointLightLayer");

            shadeLayer = GameConstants.Content.Load<Texture2D>("Textures/shadeLayer");

            GameConstants.renderTarget = new RenderTarget2D(
                GameConstants.Graphics.GraphicsDevice,
                GameConstants.Graphics.GraphicsDevice.PresentationParameters.BackBufferWidth,
                GameConstants.Graphics.GraphicsDevice.PresentationParameters.BackBufferHeight,
                false,
                GameConstants.Graphics.GraphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.Depth24);
        }

        private void CalculateGUIPositions()
        {
            playeTexStart = playerTexPos.Y = GameConstants.WINDOWSIZE.Y - txPlayer.Height;
            veilTexStart = VeilTexPos.Y = GameConstants.WINDOWSIZE.Y - txVeil[0].Height;
        }

        public void LoadContent()
        {
            LoadMapAndPlayer();

            LoadParticleSystem();

            LoadSoundMusic();            

            LoadPanel();

            LoadGUIeffects();
        }

        private void LoadGUIeffects()
        {
            List<Texture2D> temp = new List<Texture2D>();
            temp.Add(GameConstants.Content.Load<Texture2D>("GUI/speedUp"));
            temp.Add(GameConstants.Content.Load<Texture2D>("GUI/slowDown"));
            temp.Add(GameConstants.Content.Load<Texture2D>("GUI/speedUP")); //TODO: ersetze mit richtigem Icon
            GameManager.Instance.GUIFX = new GUIFlash(temp);
            GameManager.Instance.GUIFX.EmitterLocation = new Vector2(GameConstants.WINDOWSIZE.X, GameConstants.WINDOWSIZE.Y);
        }

        private void LoadMapAndPlayer()
        {
            //load Model with Animation and Textures ( UV-Mapping)
            m_player = new AnimatedModel(GameConstants.Content, "AnimatedModels/Playermodell", "AnimatedModels/MAINTEXTURE");

            //load Light-Shader for Ambient,Diffus,Specular Light
            //GameConstants.lightEffect = GameConstants.Content.Load<Effect>("FX/Test");

            String bitmapname = "Content/Maps/" + iLevel.ToString() + ".bmp";
            if (GameConstants.isDebugMode)
                Console.WriteLine("bitmap: " + bitmapname);
            levelMask = new Bitmap(bitmapname);
            testmap = null;
            testmap = new Map(levelMask);

            //after Map generation
            objectSpawner = new Spawner();
            start = GameManager.Instance.StartPos;

            objectSpawner.PlaceCoins(testmap.map);
            MaxCoins = GameManager.Instance.getCoinList().Count;
            Player = new Player(m_player);
            // x_playerModelTransforms = SetupEffectDefaults(m_player);
            Player.Spawn(new Vector3(start.X, start.Y, 0));
            GameConstants.MainCam.SetTarget(Player);
            PController = new PlayerController(Player);
            TrapHandler = new TrapHandler();


            //Loads Textfile where Animation Settings are written
            m_player.LoadAnimationParts("AnimationParts/Animations.txt");
            //searching for the Animation you are looking
            m_player.BlendToAnimationPart("Run");
        }

        private void LoadParticleSystem()
        {
            //particleSystems
            List<Texture2D> texList = new List<Texture2D>();
            texList.Add(GameConstants.Content.Load<Texture2D>("Particles/bubble"));
            //texList.Add(GameConstants.Content.Load<Texture2D>("Particles/cloud1"));
            //texList.Add(GameConstants.Content.Load<Texture2D>("Particles/cloud2"));
            //texList.Add(GameConstants.Content.Load<Texture2D>("Particles/cloud3"));
            VeilParticles = new VeilOfDath(texList, 10, 1f, new Vector2(0.5f, 0.5f));

            VeilofDeath = new Veil(GameConstants.iDifficulty, Player, VeilParticles);
        }

        private void LoadSoundMusic()
        {
            music = GameConstants.Content.Load<Song>("Music/Background");
            MediaPlayer.Play(music);
            MediaPlayer.IsRepeating = true;

            MediaPlayer.Volume = GameConstants.Volume *2/3;
        }

        private void LoadPanel()
        {
            MainPanel = new Panel(GameConstants.Content.Load<Texture2D>("Panels/panel"),
                                    new Vector2(0.1f * GameConstants.WINDOWSIZE.X,
                                                0.1f * GameConstants.WINDOWSIZE.Y));

            peRdy = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/Rdy"), true);
            pePressJump = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/pressA"), true);
            pePressJump2 = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/pressB"), false);
            ptJump = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/void"), true);
            ptJump.AddText(GameConstants.lucidaConsole, "press space \n to Jump", Microsoft.Xna.Framework.Color.White);
            peJumpA = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/JumpA"), true);
            peJumpB = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/JumpB"), false);
            ptRun = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/void"), true);
            ptRun.AddText(GameConstants.lucidaConsole, "switch lane \n with arrow buttons", Microsoft.Xna.Framework.Color.White);
            peRunA = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/RunA"), false);
            peRunB = new PanelElement(GameConstants.Content.Load<Texture2D>("Panels/RunB"), true);


            //TODO: FIX THAT ptRUN and ptJump werent shown
            MainPanel.Add( peRdy, new Vector2(0.3f,0.44f));
            MainPanel.Add( pePressJump, new Vector2(0.3f, 0.62f));
            MainPanel.Add( pePressJump2, new Vector2(0.3f, 0.62f));            
            MainPanel.Add(peRunA, new Vector2(0.6f, 0.2f));
            MainPanel.Add(peRunB, new Vector2(0.6f, 0.2f));
            MainPanel.Add(ptRun, new Vector2(0.6f, 0.1f));            
            MainPanel.Add(peJumpA, new Vector2(0.15f, 0.2f));
            MainPanel.Add(peJumpB, new Vector2(0.15f, 0.2f));
            MainPanel.Add(ptJump, new Vector2(0.15f, 0.1f));
        }

        public void UnloadContent()
        {

        }

        #endregion

        #region update methods

        public void Update(GameTime time)
        {
            if (isStarted || GameConstants.isRetryQuickJoinOn)
            {
                UpdateRun(time);
                isStarted = true;
            }
            else
                UpdateWarmUp(time);

        }

        private void UpdateWarmUp(GameTime time)
        {
            // See the map, but with Overblend of the Startscreen (with Blinking Controls)

            HandlePanelStates(time);

            // If Player Presse Enter, count down with 3 big Numbers
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                //TODO: 3,2,1 GO! single Emitting ParticelEngine -> on Player for multiple things and Ingame-Fedback

                isStarted = true;
            }              
            // Go to isStarted = true and enter runing time

        }

        private void HandlePanelStates(GameTime time)
        {
            if (time.TotalGameTime.Milliseconds % 500 == 0)
            {
                peRunA.isActive = !peRunA.isActive;
                peRunB.isActive = !peRunB.isActive;
            }
            if (time.TotalGameTime.Milliseconds % 900 == 0)
            {
                peJumpA.isActive = !peJumpA.isActive;
                peJumpB.isActive = !peJumpB.isActive;
            }
            

            if (time.TotalGameTime.Milliseconds % 200 == 0)
            {
                pePressJump.isActive = !pePressJump.isActive;
                pePressJump2.isActive = !pePressJump2.isActive;
            }
        }

        private void UpdateRun(GameTime time)
        {
            GameConstants.rotation += GameConstants.rotationSpeed;
           
            if (!isSetTime)
            {
                t0 = time.TotalGameTime.Seconds * 1000 + time.TotalGameTime.Milliseconds;
                if (GameConstants.isDebugMode)
                    Console.WriteLine("t0 = " + t0);
                isSetTime = true;
            }

            //TrapHandler.choseTraps(time);
            PController.Update(currentKeyboardState, testmap);
            oldKeyboardState = currentKeyboardState;
            fTimeDelta = (float)time.ElapsedGameTime.Milliseconds;

            UpdateHeartBeat(time);
            UpdateSpikerolls();

            UpdatePlayer(time);

            //update Animation
            m_player.Update(time);

            UpdateScore();

            UpdateCheating();

            UpdateFinishLine(time);

            GameManager.Instance.GUIFX.Update(time);

            VeilofDeath.Update(time);

            if (VeilofDeath.hasReachedPlayer)
            {
                newState = EState.GameOver;
            }

           
        }

        private void UpdateCheating()
        {

            if (Keyboard.GetState().IsKeyDown(Keys.O))
            {
                GameManager.Instance.iCoinScore[GameManager.Instance.Level] = MaxCoins;
                GameManager.Instance.Score[GameManager.Instance.Level] = (int) MaxCoins * GameConstants.ScorePerCoin;
                Player.Position.Y = GameManager.Instance.ZielPos.Y;
            }

        }

        private void UpdateSpikerolls()
        {
            if (GameManager.Instance.getRollList().Count > 0)
                foreach (SpikeRoll SR in GameManager.Instance.getRollList())
                {
                    SR.Update(testmap);
                }
        }

        /// <summary>
        ///  Allways plyas one heartbeat, depending on the actual phase, frequent and volume are higher
        /// </summary>
        /// <param name="time">Gametime</param>
        private static void UpdateHeartBeat(GameTime time)
        {

            switch (GameManager.Instance.iPhase)
            {
                case 1:
                    if ((time.TotalGameTime.Seconds*1000 + time.TotalGameTime.Milliseconds) % 2500 == 0)
                        GameConstants.HeartBeat.Play();
                    break;
                case 2:
                    if ((time.TotalGameTime.Seconds * 1000 + time.TotalGameTime.Milliseconds) % 1500 == 0)
                        GameConstants.HeartBeat.Play();
                    break;
                case 3:
                    if ((time.TotalGameTime.Seconds * 1000 + time.TotalGameTime.Milliseconds) % 900 == 0)
                        GameConstants.HeartBeat.Play();
                    break;
                case 4:
                    if ((time.TotalGameTime.Seconds * 1000 + time.TotalGameTime.Milliseconds) % 600 == 0)
                        GameConstants.HeartBeat.Play();
                    break;
                default:
                    break;
            }
        }

        private void UpdateFinishLine(GameTime time)
        {
            if (reachedFinish())
            {
                float temp = time.TotalGameTime.Minutes * 60 * 1000 + time.TotalGameTime.Seconds * 1000 + time.TotalGameTime.Milliseconds;
                float fTimeRecord = GameManager.Instance.BestTime / (temp - t0);

                if (fTimeRecord > 0.93f)
                {
                    //TODO: Feedback "+1000" über Spieler (Player-particleEnginge)
                    GameManager.Instance.iTimeBonus[GameManager.Instance.Level] = 1000;
                }
                else if (fTimeRecord > 0.7f)
                {
                    //TODO: Feedback "+500" über Spieler
                    GameManager.Instance.iTimeBonus[GameManager.Instance.Level] = 500;
                }
                else if (fTimeRecord > 0.4f)
                {
                    //TODO: Feedback "+250" über Spieler
                    GameManager.Instance.iTimeBonus[GameManager.Instance.Level] = 250;
                }
                else
                    GameManager.Instance.iTimeBonus[GameManager.Instance.Level] = 0;

                GameManager.Instance.AddtoScore(GameManager.Instance.iTimeBonus[GameManager.Instance.Level]);
                if (GameConstants.isDebugMode)
                {
                    Console.WriteLine("finished in " + (temp - t0));
                    Console.WriteLine("speed: " + fTimeRecord * 100 + "%");
                }
                
                GameManager.Instance.SetVeilDistance(VeilofDeath.fDistance);
                GameManager.Instance.fStageCleared[GameManager.Instance.Level] = (float)GameManager.Instance.iCoinScore[GameManager.Instance.Level] / (float)MaxCoins;
                canLeave = true;
                testmap = null;
                newState = EState.Score;

                //WICHTIG: damit sich die Map etc. löscht          
            }

            //ParticleTest.Update(time);
        }

        private void UpdatePlayer(GameTime time)
        {
            if (Player.isDead)
            {
                if (GameConstants.isDebugMode) 
                    Console.WriteLine("thy dieded!");
                Player = null;
                newState = EState.GameOver;
            }
            else
            {
                Player.Tick(testmap);

                if (Player.Position.Y >= GameConstants.fJumpWidth
                    && !isJumpCalculated)
                {
                    //GameConstants.fJumpSpeed = GameConstants.fJumpWidth / fTimeDelta;
                    GameManager.Instance.BestTime = (float)(GameManager.Instance.ZielPos.Y - GameManager.Instance.StartPos.Y) / (float)((Player.Position.Y - 2) / (time.TotalGameTime.Seconds * 1000 + time.TotalGameTime.Milliseconds - t0));

                    //if (GameConstants.isDebugMode)
                    //    Console.WriteLine("JumpSpeed: " + GameConstants.fJumpSpeed);

                    //GameConstants.fjumpTime = GameConstants.fJumpWidth / GameConstants.fJumpSpeed;
                    //if (GameConstants.isDebugMode)
                    //    Console.WriteLine("JumpTime: " + GameConstants.fjumpTime);

                    if (GameConstants.isDebugMode)
                        Console.WriteLine("best possible time = " + GameManager.Instance.BestTime);

                    isJumpCalculated = true;
                }
            } /*else (if (Player.isDead))*/
        }



        #endregion

        #region draw methods

        public void DrawSceneToTexture(GameTime time)
        {
            // Set the render target
            GameConstants.Graphics.GraphicsDevice.SetRenderTarget(GameConstants.renderTarget);

            GameConstants.Graphics.GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };

            // Draw the scene
            GameConstants.Graphics.GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.CornflowerBlue);

            //float t0 = time.ElapsedGameTime.Milliseconds;
            int GridY = (int)(Player.Position.Y - (GameConstants.iBlockSize / 2)) / GameConstants.iBlockSize;
            if (testmap != null)
                testmap.Draw(GridY - 6, GridY - 7 + GameConstants.fFarClipPlane);
            //Console.WriteLine("Drawtime: "+ (time.ElapsedGameTime.Milliseconds - t0)+ " seconds");

            Player.Draw(time);

            DrawObject();

            DrawGUI();

            if (!isStarted)
                DrawReadyPanel();

            // Drop the render target
            GameConstants.Graphics.GraphicsDevice.SetRenderTarget(null);
        }


        public void Draw(GameTime time)
        {
            DrawSceneToTexture(time);

            GameConstants.Graphics.GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.Black);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
                SamplerState.LinearWrap, DepthStencilState.Default,
                RasterizerState.CullNone, GameConstants.PointLightLayer);

            GameConstants.PointLightLayer.Parameters["PointLight"].SetValue(shadeLayer);

            spriteBatch.Draw(GameConstants.renderTarget, new Microsoft.Xna.Framework.Rectangle(0, 0, 1200, 700), Microsoft.Xna.Framework.Color.White);

            spriteBatch.End();
        }

        private void DrawReadyPanel()
        {
            spriteBatch.Begin(depthStencilState: GameConstants.Graphics.GraphicsDevice.DepthStencilState,
                rasterizerState: GameConstants.Graphics.GraphicsDevice.RasterizerState);

            MainPanel.Draw(spriteBatch);

            spriteBatch.End();
        }

        private void DrawObject()
        {
            foreach (Coin c in GameManager.Instance.getCoinList())
            {
                c.Draw();
            }

            foreach (SpikeTrap sp in GameManager.Instance.getSpikeList())
            {
                sp.Draw();
            }

            foreach (SpikeRoll sr in GameManager.Instance.getRollList())
            {
                sr.Draw();
            }

            foreach (SpeedTrap st in GameManager.Instance.getSpeedList())
            {
                st.Draw();
            }

        }


        private void UpdateScore()
        {
            timesincelastupdate += fTimeDelta;
            //if (timesincelastupdate > 1000)
            //{
            //    GameManager.Instance.AddtoScore((int)Math.Floor(timesincelastupdate/200));                
            //    timesincelastupdate = timesincelastupdate % 1000;
            //}
            if (timesincelastupdate > 100)
                UdpdateGUIPos();

        }


        private void UdpdateGUIPos()
            {
                if (Player != null)
                    playerTexPos.Y = GameConstants.WINDOWSIZE.Y -
                                     GameConstants.WINDOWSIZE.Y *
                                     Math.Max(0, Player.Position.Y / GameManager.Instance.ZielPos.Y);

                VeilTexPos.Y =    GameConstants.WINDOWSIZE.Y -
                                    GameConstants.WINDOWSIZE.Y * 
                                    Math.Max(0, VeilofDeath.Position.Y / GameManager.Instance.ZielPos.Y);

            }

        /// <summary>
        /// Helper to draw 2-dimensional GUI-Objects using the SpriteBatch
        /// </summary>
        private void DrawGUI()
        {
            //spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
            spriteBatch.Begin(depthStencilState: GameConstants.Graphics.GraphicsDevice.DepthStencilState,
                rasterizerState: GameConstants.Graphics.GraphicsDevice.RasterizerState);


            VeilofDeath.Draw(spriteBatch);


            // Vorlage: // spriteBatch.Draw(texture, position, color)
            spriteBatch.DrawString(GameConstants.lucidaConsole, " Score: " + GameManager.Instance.score,
                GUI_Pos, Microsoft.Xna.Framework.Color.White);

            GameManager.Instance.GUIFX.Draw(spriteBatch);

            // GUI
            spriteBatch.Draw(txLine, new Vector2(1, 1), Microsoft.Xna.Framework.Color.White);
            if (isStarted)
            {
                spriteBatch.Draw(txPlayer, playerTexPos, Microsoft.Xna.Framework.Color.White);
                spriteBatch.Draw(txVeil[0], VeilTexPos, Microsoft.Xna.Framework.Color.White);
            } 


            spriteBatch.End();
        }

        #endregion

        private bool reachedFinish()
            {
                if (Player != null)
                    return Player.Position.Y >= GameManager.Instance.ZielPos.Y ? true : false;
                else
                    return false;
            }

        }
    }
