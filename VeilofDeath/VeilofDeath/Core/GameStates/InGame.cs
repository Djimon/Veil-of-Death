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

namespace VeilofDeath.Core.GameStates
{
    class InGame : IGameState
    {
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
        Model m_player;

        public Matrix[] x_playerModelTransforms;
        private Matrix x_projectionMatrix;
        private Matrix x_viewMatrix;

        Vector2 GUI_Pos = new Vector2(1000, 20); //TODO get rid of magicConstants
        Vector2 GUI_Stuff = new Vector2(100, 400); //TODO get rid of magicConstants

        Texture2D txLine, txPlayer;
        Texture2D[] txVeil;

        private Vector3 start;

        float fTimeDelta;
        private int score;

        Spawner objectSpawner;
        private float timesincelastupdate;

        private Vector2 playerTexPos = new Vector2(0,0);
        private Vector2 VeilTexPos = new Vector2(0, 0);
        private float playeTexStart;
        private float veilTexStart;
        private float angle;

        public InGame(int Level)
        {
            spriteBatch = GameConstants.SpriteBatch;
            iLevel = Level;

            Initialize();
            LoadContent();
        }



        public void Initialize()
        {
            newState = EState.none;
            currentKeyboardState = Keyboard.GetState();
            oldKeyboardState = new KeyboardState();            

            GameConstants.levelDictionary = LevelContent.LoadListContent<Model>(GameConstants.Content, "Models/Level1");
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
        }

        private void CalculateGUIPositions()
        {
            playeTexStart = playerTexPos.Y = GameConstants.WINDOWSIZE.Y - txPlayer.Height;
            veilTexStart = VeilTexPos.Y = GameConstants.WINDOWSIZE.Y - txVeil[0].Height;
        }

        public void LoadContent()
        {
            
            m_player = LoadModel("Models/cube");
            levelMask = new Bitmap("Content/Maps/testmap2.bmp"); //TODO: rename to "1" for level 1 and so on "2", "3" load in pendancy of level
            testmap = new Map(levelMask);

            //after Map generation
            objectSpawner = new Spawner();
            start = GameManager.Instance.StartPos;


            objectSpawner.PlaceCoins(testmap.map);
            Player = new Player(m_player);
            x_playerModelTransforms = SetupEffectDefaults(m_player);
            Player.Spawn(new Vector3(start.X, start.Y, 0));
            PController = new PlayerController(Player);

            TrapHandler = new TrapHandler();
            

            GameConstants.MainCam.SetTarget(Player);
        }

        public void UnloadContent()
        {
            
        }

        public void Update(GameTime time)
        {

            //TrapHandler.choseTraps(time);

            PController.Update(currentKeyboardState);
            oldKeyboardState = currentKeyboardState;
            fTimeDelta = (float)time.ElapsedGameTime.Milliseconds;
            if (Player.isDead)
            {
                Player = null;
                newState = EState.GameOver;
            }
            else
            {
                Player.Tick();

                if (Player.Position.Y >= GameConstants.fJumpWidth
                    && Player.Position.Y <= GameConstants.fJumpWidth + 0.111f)
                {
                    GameConstants.fJumpSpeed = GameConstants.fJumpWidth / fTimeDelta;
                    if (GameConstants.isDebugMode)
                        Console.WriteLine("JumpSpeed: " + GameConstants.fJumpSpeed);
                    GameConstants.fjumpTime = GameConstants.fJumpWidth / GameConstants.fJumpSpeed;
                    Console.WriteLine("JumpTime: "+ GameConstants.fjumpTime);
                }
            }            
            
            //GameConstants.MainCam.Update(time);
            UpdateScore();

            if (reachedFinish())
            {
                newState = EState.Score;
            }
        }

        public void Draw(GameTime time)
        {
            testmap.Draw();

            Player.Draw();

            DrawObject();

            DrawGUI();
            
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

            // Spikes


            // Drehdinger


        }


        private void UpdateScore()
        {
            timesincelastupdate += fTimeDelta;
            if (timesincelastupdate > 1000)
            {
                GameManager.Instance.AddtoScore((int)Math.Floor(timesincelastupdate/200));                
                timesincelastupdate = timesincelastupdate % 1000;
            }
            if (timesincelastupdate > 100)
                UdpdateGUIPos();

        }


        private void UdpdateGUIPos()
        {
            if (Player != null)
                playerTexPos.Y = GameConstants.WINDOWSIZE.Y - GameConstants.WINDOWSIZE.Y * Math.Max(0, Player.Position.Y / GameManager.Instance.ZielPos.Y);
            //veilTexStart.Y = Math.Max(0, veil.Position.Y / lange);

        }

        /// <summary>
        /// Helper to draw 2-dimensional GUI-Objects using the SpriteBatch
        /// </summary>
        private void DrawGUI()
        {
            //spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
            spriteBatch.Begin(depthStencilState: GameConstants.Graphics.GraphicsDevice.DepthStencilState, rasterizerState: GameConstants.Graphics.GraphicsDevice.RasterizerState);

            // Vorlage: // spriteBatch.Draw(texture, position, color)
            spriteBatch.DrawString(GameConstants.lucidaConsole, " Score: " + GameManager.Score,
                                   GUI_Pos, Microsoft.Xna.Framework.Color.White);


            spriteBatch.Draw(txLine, new Vector2(1, 1), Microsoft.Xna.Framework.Color.White);
            spriteBatch.Draw(txPlayer, playerTexPos, Microsoft.Xna.Framework.Color.White);
            //spriteBatch.Draw(txVeil[0], VeilTexPos, Microsoft.Xna.Framework.Color.White);
            //Vector2 location = new Vector2(400, 240);
            //Microsoft.Xna.Framework.Rectangle sourceRectangle = new Microsoft.Xna.Framework.Rectangle(0, 0, txVeil[1].Width, txVeil[1].Height);
            //Vector2 origin = new Vector2(0, 0);
            //angle += 0.11f;
            //spriteBatch.Draw(txVeil[0], location, sourceRectangle, Microsoft.Xna.Framework.Color.White, angle, origin, 1.0f, SpriteEffects.None, 1);

            spriteBatch.End(); ;
        }

        /// <summary>
        /// Enables the default effect for the given model
        /// </summary>
        /// <param name="myModel">3D Model</param>
        /// <returns>absolute bone transforms as Matrix[]</returns>
        private Matrix[] SetupEffectDefaults(Model myModel)
        {
            Matrix[] absoluteTransforms = new Matrix[myModel.Bones.Count];
            myModel.CopyAbsoluteBoneTransformsTo(absoluteTransforms);

            foreach (ModelMesh mesh in myModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.Projection = x_projectionMatrix;
                    effect.View = x_viewMatrix;
                }
            }
            return absoluteTransforms;
        }

        /// <summary>
        /// Loads a Model from content pipeline via string
        /// and apply a basic effect
        /// </summary>
        /// <param name="assetName">name of model in content pipeline (with path)</param>
        /// <returns>Model</returns>
        private Model LoadModel(string assetName)
        {
            Model newModel = GameConstants.Content.Load<Model>(assetName);

            //foreach (ModelMesh mesh in newModel.Meshes)
            //{
            //    foreach (ModelMeshPart meshPart in mesh.MeshParts)
            //    {
            //        //meshPart.Effect = basicEffect.Clone();
            //    }
            //}

            return newModel;
        }

        private bool reachedFinish()
        {
            if (Player != null)
                return Player.Position.Y >= GameManager.Instance.ZielPos.Y ? true : false;
            else
                return false;
        }

    }
}
