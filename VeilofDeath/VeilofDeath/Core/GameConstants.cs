using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections;

namespace VeilofDeath
{
    static class GameConstants
    {
        /// <summary>
        /// If on puts some outputs to the console
        /// </summary>
        public static bool isDebugMode = true;
        //mandatory things        
        public static ContentManager Content;

        // Camera and View
        public static Vector2 WINDOWSIZE = new Vector2(1200, 700);
        /// <summary>
        /// fixed aspect ratio of the window
        /// </summary>
        public static float fAspectRatio = WINDOWSIZE.X / WINDOWSIZE.Y;
        public static Camera MainCam;
        public static int CameraAngle = 35;
        /// <summary>
        /// fixed window size width
        /// </summary>
        public static float fCameraHeight = 7f;
        /// <summary>
        /// Belongs to the Camera View Distance
        /// </summary>
        public static float fFarClipPlane = 150;
        /// <summary>
        /// Belongs to the Camera View Distance
        /// </summary>
        public static float fNearClipPlane = 0.1f;
        internal static SpriteBatch SpriteBatch;

        //Map/ level
        /// <summary>
        /// Size of each block in the map (like tiles)
        /// </summary>
        public static int iBlockSize = 4;
        public static float fLaneCenter = 1 + (iBlockSize*6 - 1) /2;  // *3 <= * mapmask.width (actual 6) and :2 (center)

        /// <summary>
        /// Length of the Jump
        /// </summary>
        public static float fJumpWidth = 2* iBlockSize;
        /* to overjump one block: when jumping is pressed in the middle of the before-block, 
         * it takes 2 blocks to land in the middle of the after-block */
        public static float fJumpHeight =  iBlockSize *6/5;
        public static float fJumpSpeed;

        /// <summary>
        /// Dictionary which holds the models for each LevelBlock
        /// </summary>
        public static Dictionary<string, Model> levelDictionary;
        /// <summary>
        /// Z-Axis of the level
        /// </summary>
        public static float fLevelHeight = -1f * iBlockSize +1 ;

        //Gameplay
        /// <summary>
        /// Speed the player moves
        /// </summary>
        public static float fMovingSpeed = 3;
        /// <summary>
        /// number of Traps in the Game
        /// </summary>
        public static int iTrapNumber = 4;
        public static int iMaxLevel = 1;

        public static bool isCollided = false;

        public static Game currentGame;

              
        
    }

}