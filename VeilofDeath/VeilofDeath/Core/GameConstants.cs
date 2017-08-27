using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using VeilofDeath.Objects;

namespace VeilofDeath
{
    static class GameConstants
    {
        /// <summary>
        /// If on puts some outputs to the console
        /// </summary>
        public static bool isDebugMode = false;
        //mandatory things        
        public static ContentManager Content;

        // Camera and View
        public static Vector2 WINDOWSIZE = new Vector2(1200, 700);

        #region camera variables

        /// <summary>
        /// fixed aspect ratio of the window
        /// </summary>
        public static float fAspectRatio = WINDOWSIZE.X / WINDOWSIZE.Y;
        public static Camera MainCam;
        public static int CameraAngle = 35;
        public static float CameraDistance = -20; //TODO: Fix staert of game is behind the Map
        /// <summary>
        /// fixed window size width
        /// </summary>
        public static float fCameraHeight = 7;
        /// <summary>
        /// Belongs to the Camera View Distance
        /// </summary>
        public static float fFarClipPlane = 100;
        /// <summary>
        /// Belongs to the Camera View Distance
        /// </summary>
        public static float fNearClipPlane = 0.1f;

        #endregion

        internal static SpriteBatch SpriteBatch;

        //Map/ level
        /// <summary>
        /// Size of each block in the map (like tiles)
        /// </summary>
        public static int iBlockSize = 4;
        public static float fLaneCenter = (iBlockSize*6) /2;  // *3 <= * mapmask.width (actual 6) and :2 (center)

        #region jump variables

        /// <summary>
        /// Length of the Jump
        /// </summary>
        public static float fJumpWidth = 2.5f * iBlockSize;
        /* to overjump one block: when jumping is pressed in the middle of the before-block, 
         * it takes 2 blocks to land in the middle of the after-block */
        public static float fJumpHeight = iBlockSize * 6 / 5;
        //public static float fJumpSpeed;
        //public static float fjumpTime;

        #endregion

        /// <summary>
        /// Dictionary which holds the models for each LevelBlock
        /// </summary>
        public static Dictionary<string, Model> levelDictionary;
        /// <summary>
        /// Z-Axis of the level
        /// </summary>
        public static float fLevelHeight = -1f * iBlockSize  +0.1f;

        //Gameplay
        /// <summary>
        /// Speed the player moves
        /// </summary>
        public static float fMovingSpeed = 2;
        /// <summary>
        /// number of Traps in the Game
        /// </summary>
        public static int iTrapNumber = 4;
        public static int iMaxLevel = 3;
        /// <summary>
        /// Diificulty can be 1,2,3 or 4
        /// </summary>
        public static int iDifficulty = 1; 

        public static bool isCollided = false;

        public static Game currentGame;
        public static SpriteFont lucidaConsole;
        internal static int iWinStauts = 0;

        public static GraphicsDeviceManager Graphics { get; internal set; }

        //variables for rotating the coins
        public static float rotation = 0f;
        public static float rotationSpeed = 0.05f;

        #region Sound Variables

        public static SoundEffect CoinCollect;
        public static SoundEffect Landing;
        public static SoundEffect CharactersJump;


        #endregion
    }

}