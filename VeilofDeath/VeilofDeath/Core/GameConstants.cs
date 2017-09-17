using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using VeilofDeath.Objects;

namespace VeilofDeath.Core
{
    static class GameConstants
    {
        /// <summary>
        /// If on puts some outputs to the console
        /// </summary>
        public static bool isDebugMode = false;
        
        /// <summary>
        /// Global Content Manager (MonoGame)
        /// </summary>                   
        public static ContentManager Content;

        /// <summary>
        /// Windowsize (width,height)
        /// </summary>
        public static Vector2 WINDOWSIZE = new Vector2(1200, 700);

        #region camera variables

        /// <summary>
        /// fixed aspect ratio of the window
        /// </summary>
        public static float fAspectRatio = WINDOWSIZE.X / WINDOWSIZE.Y;
        /// <summary>
        /// Main Camera Object
        /// </summary>
        public static Camera MainCam;
        public static int CameraAngle = 35;
        public static float CameraDistance = 55; //TODO: Fix staert of game is behind the Map
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
        public static float fMovingSpeed = 1.5f +  (0.25f * iDifficulty) ;
        /// <summary>
        /// number of Traps in the Game
        /// </summary>
        public static int iTrapNumber = 4;
        public const int iMaxLevel = 6;
        /// <summary>
        /// Diificulty can be 1,2,3 or 4
        /// </summary>
        public static int iDifficulty = 1; 

        /// <summary>
        /// Basic Score per Coin.
        /// Will be heigher at harder difficulties
        /// </summary>
        public static int ScorePerCoin = 25;

        public static Game currentGame;
        /// <summary>
        /// Global Font for TextObjects
        /// </summary>
        public static SpriteFont lucidaConsole;
        /// <summary>
        /// Contains the information if the game is fully completed
        /// </summary>
        internal static int iWinStatus = 0;
        internal static bool hasGameWon = false;
        /// <summary>
        /// Triggers the Quick-Rejoin function, when a level has to be redone
        /// </summary>
        internal static bool isRetryQuickJoinOn = false;

        public static GraphicsDeviceManager Graphics { get; internal set; }

        //variables for rotating the coins
        public static float rotation = 0f;
        public static float rotationSpeed = 0.05f;


        #region Sound Variables

        public static SoundEffect CoinCollect;
        public static SoundEffect Landing;
        public static SoundEffect CharactersJump;
        public static SoundEffect Winner;
        public static SoundEffect TotalWinner;
        public static SoundEffect Loser;
        public static SoundEffect ChangePhase;
        public static SoundEffect HeartBeat;
        public static SoundEffect Select;
        public static SoundEffect Switch;

        public static List<SoundEffect> Sounds = new List<SoundEffect>();

        public static float Volume = 0.75f; // 1 = 100%
        internal static float fSpeedTime = 350 - (50 * iDifficulty);
        //public static SoundEffectInstance HBInstance;

        #endregion
    }

}