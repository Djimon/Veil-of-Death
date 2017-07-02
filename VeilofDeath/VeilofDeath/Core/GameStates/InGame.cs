using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Drawing;
using Animations;

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

        public Matrix[] x_playerModelTransforms;
        private Matrix x_projectionMatrix;
        private Matrix x_viewMatrix;

        Vector2 GUI_Pos = new Vector2(100, 50); //TODO get rid of magicConstants
        Vector2 GUI_Stuff = new Vector2(100, 400); //TODO get rid of magicConstants

        private Vector3 start;

        float fTimeDelta;
        private int score;

        #endregion

        public InGame(int Level)
        {
            spriteBatch = GameConstants.SpriteBatch;
            iLevel = Level;

            Initialize();
            LoadContent();
        }

        #region initialize, content methods

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

            
            //TODO: initialize these Matrixes
            //x_projectionMatrix =
            //x_viewMatrix =
        }

        public void LoadContent()
        {
            //load Model with Animation and Textures ( UV-Mapping)
            m_player = new AnimatedModel(GameConstants.Content, "Models/Level1/Playermodell","Textures/MAINTEXTURE");


            levelMask = new Bitmap("Content/Maps/testmap2.bmp"); //TODO: rename to "1" for level 1 and so on "2", "3" load in pendancy of level

            testmap = new Map(levelMask);
            start = GameManager.Instance.StartPos;
            Player = new Player(m_player);
           // x_playerModelTransforms = SetupEffectDefaults(m_player);
            Player.Spawn(new Vector3(start.X, start.Y, 0));
            PController = new PlayerController(Player);

            TrapHandler = new TrapHandler();

            GameConstants.MainCam.SetTarget(Player);




            //Loads Textfile where Animation Settings are written
            m_player.LoadAnimationParts("AnimationParts/Run.txt");


            //searching for the Animation you are looking
            m_player.BlendToAnimationPart("Run");

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
            foreach (ModelMesh mesh in newModel.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    //meshPart.Effect = basicEffect.Clone();
                }
            }
            return newModel;
        }

        public void UnloadContent()
        {
            
        }

        #endregion

        #region update methods

        public void Update(GameTime time)
        {

            TrapHandler.choseTraps(time);

            PController.Update(currentKeyboardState);
            oldKeyboardState = currentKeyboardState;
            fTimeDelta += (float)time.ElapsedGameTime.TotalSeconds;
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

            //update Animation
            m_player.Update(time);
        }

        private void UpdateScore()
        {
            score = (int)(fTimeDelta * 10);
            GameManager.UpdateScore(score);
        }

        #endregion

        #region draw methods

        public void Draw(GameTime time)
        {
            testmap.Draw();

            Player.Draw(time);

            //NewDrawModel(Player);

            DrawGUI();

        }

        /// <summary>
        /// Helper to draw 2-dimensional GUI-Objects using the SpriteBatch
        /// </summary>
        private void DrawGUI()
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
            //Debug - Anzeige
            spriteBatch.DrawString(GameConstants.lucidaConsole, "Pos: " + Player.Position+ " Score: "+GameManager.Score,
                                   GUI_Pos, Microsoft.Xna.Framework.Color.White);

            spriteBatch.End(); ;
        }

        #endregion

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

        private bool reachedFinish()
        {
            if (Player != null)
                return Player.Position.Y >= GameManager.Instance.ZielPos.Y ? true : false;
            else
                return false;
        }

    }
}
