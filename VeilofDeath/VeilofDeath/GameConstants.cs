using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace VeilofDeath
{
    static class GameConstants
    {
        public static int windowWidth = 800;
        public static int windowHeight = 600;

        public static float fCameraHeight = 100;
        public static float fAspectRatio;
        public static float MovingSpeed = 1;

        public static float FarClipPlane = 200;
        public static float NearClipPlane = 1;

        public static int BlockSize = 90;
        public static Dictionary<string, Model> levelDictionary;
        public static float LevelHeight = 0;

        public static int TrapNumber = 4;
    }

}