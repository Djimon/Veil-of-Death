using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace VeilofDeath
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GamePadState lastState = GamePad.GetState(PlayerIndex.One);
        KeyboardState oldKeyboardState, currentKeyboardState;

        Vector3 cameraPosition = new Vector3(0.0f, 0.0f, GameConstants.fCameraHeight);
        Matrix x_projectionMatrix;
        Matrix x_viewMatrix;

        Player Player;
        Model m_player;
        Matrix[] x_playerModelTransforms;

        Camera camera;

        Vector3 lightDirection = new Vector3(3, -2, 5);
        SpriteFont lucidaConsole;

        Vector2 GUI_Pos = new Vector2(100, 50); //TODO get rid of magicConstants
        Vector2 GUI_Stuff = new Vector2(100, 400); //TODO get rid of magicConstants

        PlayerController PController;

        //level test variables
        Level dungeon;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            GameConstants.fAspectRatio = (float)GraphicsDeviceManager.DefaultBackBufferWidth / GraphicsDeviceManager.DefaultBackBufferHeight;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = GameConstants.windowWidth;
            graphics.PreferredBackBufferHeight = GameConstants.windowHeight;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Window.Title = "Veil of Death (alpha 0.01a)";

            lightDirection.Normalize();

            // TODO: Add your initialization logic here
            //SetUpCamera();
            camera = new Camera(graphics.GraphicsDevice);

            Player = new Player();

            //Level test initialize
            dungeon = new Level();
            dungeon.Initialize();

            oldKeyboardState = Keyboard.GetState();
            currentKeyboardState = new KeyboardState();
            PController = new PlayerController();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here
            //basicEffect = Content.Load<Effect>("effects");
            lucidaConsole = Content.Load<SpriteFont>("Fonts/Lucida Console");
            m_player = LoadModel("Models/cube");
            //m_player = CM.Load<Model>("Models/cube");
            Player.Initilize(m_player);
            x_playerModelTransforms = SetupEffectDefaults(m_player);
            Player.Spawn(new Vector3(0, 1, 1));

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed 
                || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            float fTimeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // UpdateCamera();
            // TODO: Add your update logic here

            PController.Update(oldKeyboardState, Player);

            oldKeyboardState = currentKeyboardState;

            //letztes Update immer die Camera!
            camera.Update(gameTime, Player);
            base.Update(gameTime);
        }



        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSlateBlue);                   

            dungeon.DrawGround(GraphicsDevice); //my Update with Camera as input propperty


            Player.Draw(camera);

            DrawGUI();
            
            base.Draw(gameTime);
        }

        private void DrawGUI()
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
            spriteBatch.DrawString(lucidaConsole, "Pos: " + Player.Position,
                                   GUI_Pos, Color.White);

            spriteBatch.End(); ;
        }
        /// <summary>
        /// Sets up the view matrix and the projection matrix
        /// </summary>
        private void SetUpCamera()
        {
            x_viewMatrix = Matrix.CreateLookAt(new Vector3(20, 13, -5), new Vector3(8, 0, -7), new Vector3(0, 1, 0));
            x_projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 
                                                                    GameConstants.fAspectRatio, 
                                                                    GameConstants.NearClipPlane, 
                                                                    GameConstants.FarClipPlane);
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
            Model newModel = Content.Load<Model>(assetName);
            foreach (ModelMesh mesh in newModel.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    //meshPart.Effect = basicEffect.Clone();
                }
            }
            return newModel;
        }

        /// <summary>
        /// draws the given model
        /// </summary>
        /// <param name="model">Model</param>
        /// <param name="modelTranslation">Translation matrix of the given model</param>
        /// <param name="absoluteBoneTransforms">bone transformation matrix of given model</param>
        public void DrawModel(Model model, Matrix modelTranslation, Matrix[] absoluteBoneTransforms)
        {

            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            //Draw the model, a model can have multiple meshes, so loop
            foreach (ModelMesh mesh in model.Meshes)
            {
                //This is where the mesh orientation is set
                foreach (BasicEffect effect in mesh.Effects)
                {
                    //effect.World = absoluteBoneTransforms[mesh.ParentBone.Index] * modelTranslation;
                    effect.View = x_viewMatrix;
                    effect.World = Matrix.CreateTranslation(new Vector3(0, 0, 0));
                    effect.Projection = x_projectionMatrix;
                }
                //Draw the mesh, will use the effects set above.
                mesh.Draw();
            }
        }

        //private void UpdateCamera()
        //{
        //    Vector3 campos = new Vector3(0, 0.1f, 0.6f);
        //    campos = Vector3.Transform(campos, Matrix.CreateFromQuaternion(Player.Rotation));
        //    campos += Player.Position;

        //    Vector3 camup = new Vector3(0, 1, 0);
        //    camup = Vector3.Transform(camup, Matrix.CreateFromQuaternion(Player.Rotation));

        //    viewMatrix = Matrix.CreateLookAt(campos, Player.Position, camup);
        //    projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GameConstants.fAspectRatio, 0.2f, 500.0f);
        //}
    }
}
