using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace VeilofDeath
{
    static class GameConstants
    {
        //mandatory things        
        public static ContentManager Content;

        // Camera and View
        public static Vector2 WINDOWSIZE = new Vector2(1200, 700);
        /// <summary>
        /// fixed aspect ratio of the window
        /// </summary>
        public static float fAspectRatio = WINDOWSIZE.X / WINDOWSIZE.Y;
        public static Camera MainCam;
        public static int CameraAngle = 40;
        /// <summary>
        /// fixed window size width
        /// </summary>
        public static float fCameraHeight = 10f;
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
        public static float fLaneCenter = (iBlockSize*6 -1) /2;  // *3 <= * mapmask.width (actual 6) and :2 (center)
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
        public static float fMovingSpeed = 1;
        /// <summary>
        /// number of Traps in the Game
        /// </summary>
        public static int iTrapNumber = 4;
        public static int iMaxLevel = 1;
        
    }

}