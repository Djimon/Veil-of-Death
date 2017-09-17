using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using VeilofDeath.Core;

namespace VeilofDeath.Level
{
    public class Map
    {

        /// <summary>
        /// Position which triggers the level completion.
        /// </summary>
        public Vector2 Ziel { get; set; }


        /// <summary>
        /// Position where the palyer spawns.
        /// </summary>
        public Vector2 Start { get; private set; }

        /// <summary>
        /// representation of the map.
        /// </summary>
        public Block[,] map;

        int scale;

        /// <summary>
        /// blocksize for all blocks.
        /// </summary>
        public int iBlockSize { get; set; }  

        /* ~~~~ Strings, um Bitmapfarbe einem Blocktyp zuzuordnen ~~~~ */
        public static String sWhite = "ffffffff"; //Weg
        public static String sBlack = "ff000000"; //Mauer Vertikal
        public static String sRed = "ffff0000"; // loch /spike
        public static String sGreen = "ff00ff00"; //Start
        public static String sBlue = "ff0000ff"; //Ziel
        public static String sGrey = "ff424242"; //Mauer Horizontal
        public static String sOrange = "ffff8800"; // Slowtrap
        public static String sCyan = "ff00ffff"; // spikerolll
        public static String sDarkGreen = "ff004000"; // Door
        public static String SlightGrey = "ff909090"; // Zelle

        /// <summary>
        ///  <para>generates a map with random traps and the possibility of fixed traps ("taktische Falle")</para>
        /// 
        ///  <para>Overview of which color decode which feature:       </para>
        ///  <para>Swhite     = "ffffffff" Weg                        true      </para>        
        ///  <para>Sgrey      = "ff414141" mauer                      false     </para>
        ///  <para>Sred       = "ffff0000" loch                       false     </para>
        ///  <para>Sblue      = "ff0000ff" Ziel                       true      </para>
        ///  <para>Sgreen     = "ff00ff00" Start                      true      </para>
        ///  <para>Sblack     = "ff000000" Falle (random)             false     </para>
        ///  <para>Sorange    = "ffff8800" taktische Falle 1          true      </para>
        ///  <para>Scyan      = "ff008080" taktische Falle 2          true      </para>
        ///  <para>Sdarkgreen = "ff004000" door                       true      </para>
        ///  <para>SlightGrey = "ff909090" Zelle                      false     </para>
        ///  
        ///  <para>Sred is default.                                       </para>
        /// </summary>
        /// <param name="mask">Bitmap file (1px descripes 1 Tile)</param>
        public Map(Bitmap mask)
        {
            iBlockSize = GameConstants.iBlockSize;
            scale = 2;
            Ziel = new Vector2(0, 0);

            map = new Block[mask.Width, mask.Height];
            if (GameConstants.isDebugMode)
                Console.WriteLine("Maske: "+mask.Width +":"+ mask.Height);

            for (int row = 0; row < map.GetLength(0); row++)
            {
                for (int col = 0; col < map.GetLength(1); col++)
                {
                    if (GameConstants.isDebugMode)
                        Console.WriteLine("P("+row+":"+col+") color = "+mask.GetPixel(row,col).Name);

                    if (mask.GetPixel(row, col).Name == sBlack)
                    {
                        GetTrap(mask,row,col);                        
                    }
                    else if (mask.GetPixel(row, col).Name == sWhite)
                    {
                        map[row, col] = new Block(0, new Vector2(row * iBlockSize + scale, col * iBlockSize + scale));
                        // weg
                    }
                    else if (mask.GetPixel(row, col).Name == sGrey)
                    {
                        map[row, col] = new Block(1, new Vector2(row * iBlockSize + scale, col * iBlockSize + scale));
                        // mauer
                    }
                    else if (mask.GetPixel(row, col).Name == sRed)
                    {
                        map[row, col] = new Block(2, new Vector2(row * iBlockSize + scale, col * iBlockSize + scale));
                        // spike
                    }
                    else if (mask.GetPixel(row, col).Name == sGreen)
                    {
                        map[row, col] = new Block(3, new Vector2(row * iBlockSize + scale, col * iBlockSize + scale));
                        Start = new Vector2(row * iBlockSize + 1, col * iBlockSize + 1);
                        // start
                    }
                    else if (mask.GetPixel(row, col).Name == sBlue)
                    {
                        map[row, col] = new Block(4, new Vector2(row * iBlockSize + scale, col * iBlockSize + scale));
                        Ziel = new Vector2(row * iBlockSize + 1, col * iBlockSize + 1);
                        // ziel
                    }
                    else if (mask.GetPixel(row, col).Name == sOrange)
                    {
                        map[row, col] = new Block(5, new Vector2(row * iBlockSize + scale, col * iBlockSize + scale));
                        // Slow
                    }
                    else if (mask.GetPixel(row, col).Name == sCyan)
                    {
                        map[row, col] = new Block(6, new Vector2(row * iBlockSize + scale, col * iBlockSize + scale));
                        // SpikeRoll
                    }
                    else if (mask.GetPixel(row, col).Name == sDarkGreen)
                    {
                        map[row, col] = new Block(7, new Vector2(row * iBlockSize + scale, col * iBlockSize + scale));
                        // Door
                    }
                    else if (mask.GetPixel(row, col).Name == SlightGrey)
                    {
                        map[row, col] = new Block(8, new Vector2(row * iBlockSize + scale, col * iBlockSize + scale));
                        // Zelle
                    }
                    else
                    {
                        map[row, col] = new Block(0, new Vector2(row * iBlockSize + scale, col * iBlockSize + scale));
                        //loch
                    }                                        
                }
            }
        }


        /// <summary>
        /// generates a random Trap
        /// </summary>
        /// <param name="bm">Bitmap mask from constructor</param>
        /// <param name="row">actual row (X)</param>
        /// <param name="col">actual column (Y)</param>
        private void GetTrap(Bitmap bm, int row, int col)
        {
            Random n = new Random();
            int temp = n.Next(1,GameConstants.iTrapNumber); // derzeit 4
            switch (temp)
            {
                case 1: map[row, col] = new Block(5, new Vector2(row * iBlockSize + scale, col * iBlockSize + scale)); // Falle 1
                    break;
                case 2: map[row, col] = new Block(6, new Vector2(row * iBlockSize + scale, col * iBlockSize + scale)); // Falle 2
                    break;
                case 3: map[row, col] = new Block(7, new Vector2(row * iBlockSize + scale, col * iBlockSize + scale)); // Falle 3
                    break;
                case 4: map[row, col] = new Block(8, new Vector2(row * iBlockSize + scale, col * iBlockSize + scale)); // Falle 4
                    break;
                //case 5:
                //    break;
                //case 6:
                //    break;
                default: map[row, col] = new Block(2, new Vector2(row * iBlockSize + scale, col * iBlockSize + scale)); //loch

                    break;
            }
             
        }

        /// <summary>
        /// Draws the map, within a given draw Range
        /// </summary>
        /// <param name="min">minimal draw position (should be after the player)</param>
        /// <param name="max">maximal draw positon (shouldn't be larger then camera farplane)</param>
        public void Draw(float min, float max)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++) 
                {
                    if (j> min && j<max) //Culling to increase performance
                        map[i, j].Draw();
                }
            }
        }
    }
}
