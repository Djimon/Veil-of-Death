using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Drawing;

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

        public SpriteBatch spriteBatch;
        public SpriteFont lucidaConsole;

        int iLevel;

        Bitmap levelMask;
        public Map testmap;  

        Player Player;
        PlayerController PController;
        Model m_player;

        public Matrix[] x_playerModelTransforms;
        private Matrix x_projectionMatrix;
        private Matrix x_viewMatrix;

        Vector2 GUI_Pos = new Vector2(100, 50); //TODO get rid of magicConstants
        Vector2 GUI_Stuff = new Vector2(100, 400); //TODO get rid of magicConstants

        private bool reachedFinish = false;

        float fTimeDelta;
        private int score;

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
            
            


            lucidaConsole = GameConstants.Content.Load<SpriteFont>("Fonts/Lucida Console");

            GameConstants.levelDictionary = LevelContent.LoadListContent<Model>(GameConstants.Content, "Models/Level1");
            foreach (KeyValuePair<string, Model> SM in GameConstants.levelDictionary)
            {
                Console.WriteLine("Key:" + SM.Key + ", Value: " + SM.Value);
            }

            //TODO: initialize these Matrixes
            //x_projectionMatrix =
            //x_viewMatrix =
    }

        public void LoadContent()
        {
            
            m_player = LoadModel("Models/cube");

            levelMask = new Bitmap("Content/Maps/testmap.bmp");

            testmap = new Map(levelMask);
            Player = new Player(m_player);
            x_playerModelTransforms = SetupEffectDefaults(m_player);
            Player.Spawn(new Vector3(GameConstants.fLaneCenter, 0, 0));
            PController = new PlayerController(Player);

            GameConstants.MainCam.SetTarget(Player);
        }

        public void UnloadContent()
        {
            
        }

        public void Update(GameTime time)
        {
            PController.Update(currentKeyboardState);
            oldKeyboardState = currentKeyboardState;
            Player.Tick();
            fTimeDelta += (float)time.ElapsedGameTime.TotalSeconds;
            //GameConstants.MainCam.Update(time);
            UpdateScore();

            if (reachedFinish)
            {
                newState = EState.Score;
            }
        }

        public void Draw(GameTime time)
        {
            testmap.Draw();

            Player.Draw();

            //NewDrawModel(Player);

            DrawGUI();
            
        }

        private void UpdateScore()
        {
            score = (int)(fTimeDelta * 10);
            GameManager.UpdateScore(score);
        }

        /// <summary>
        /// Helper to draw 2-dimensional GUI-Objects using the SpriteBatch
        /// </summary>
        private void DrawGUI()
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
            //Debug - Anzeige
            spriteBatch.DrawString(lucidaConsole, "Pos: " + Player.Position+ " Score: "+GameManager.Score,
                                   GUI_Pos, Microsoft.Xna.Framework.Color.White);




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
            foreach (ModelMesh mesh in newModel.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    //meshPart.Effect = basicEffect.Clone();
                }
            }
            return newModel;
        }

    }
}
