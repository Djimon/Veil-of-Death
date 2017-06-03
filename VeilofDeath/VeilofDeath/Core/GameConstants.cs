﻿using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace VeilofDeath
{
    static class GameConstants
    {

        // Camera and View
        public static Vector2 WINDOWSIZE = new Vector2(1200, 700);
        /// <summary>
        /// fixed aspect ratio of the window
        /// </summary>
        public static float fAspectRatio = WINDOWSIZE.X / WINDOWSIZE.Y;
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

        //Map/ level
        /// <summary>
        /// Size of each block in the map (like tiles)
        /// </summary>
        public static int iBlockSize = 2;
        public static float fLaneCenter = iBlockSize * 3;  // *3 <= * mapmask.width (actual 6) and :2 (center)
        /// <summary>
        /// Dictionary which holds the models for each LevelBlock
        /// </summary>
        public static Dictionary<string, Model> levelDictionary;
        /// <summary>
        /// Z-Axis of the level
        /// </summary>
        public static float fLevelHeight = -1;

        //Gameplay
        /// <summary>
        /// Speed the player moves
        /// </summary>
        public static float fMovingSpeed = 1;
        /// <summary>
        /// number of Traps in the Game
        /// </summary>
        public static int iTrapNumber = 4;
    }

}