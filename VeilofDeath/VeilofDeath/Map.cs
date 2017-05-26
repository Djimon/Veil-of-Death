using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace VeilofDeath
{
    class Map
    {
        /// <summary>
        /// Gibt Position des Ziels an.
        /// </summary>
        public Vector2 Ziel { get; set; }


        /// <summary>
        /// Gibt Startposition des players an.
        /// </summary>
        public Vector2 Start { get; private set; }

        /// <summary>
        /// Block-Array zum Anlegen der Map.
        /// </summary>
        public Block[,] map;

        /// <summary>
        /// Blockgröße, um zentral einzustellen, welchen Wert sie hat.
        /// </summary>
        public int iBlockSize { get; set; }
        // ToDo: eventuell für Höhe und Breite der Blöcke unterscheiden, da der sichtbare Teil der Spielertextur nicht so breit wie hoch ist


        /* ~~~~ Strings, um Bitmapfarbe einem Blocktyp zuzuordnen ~~~~ */
        public static String sWhite = "ffffffff"; //Weg
        public static String sBlack = "ff000000"; //Mauer Vertikal
        public static String sRed = "ffff0000"; // Hintergrund
        public static String sGreen = "ff00ff00"; //Start
        public static String sBlue = "ff0000ff"; //Ziel
        public static String sGrey = "ff424242"; //Mauer Horizontal
        public static String sOrange = "ffff8800"; // Loch im Boden
        public static String sCyan = "ff008080"; // geheimer Weg Vorderansicht
        public static String sDarkGreen = "ff004000"; // geheimer Weg Draufsicht


        /// <summary>
        ///  <para>generates a map with random traps and the possibility of fixed traps ("taktische Falle")</para>
        /// 
        ///  <para>Übersicht der Namen, Werte, Kacheltypen und walkables:       </para>
        ///  <para>Swhite     = "ffffffff" Weg                        true      </para>        
        ///  <para>Sgrey      = "ff414141" mauer                      false     </para>
        ///  <para>Sred       = "ffff0000" loch                       false     </para>
        ///  <para>Sblue      = "ff0000ff" Ziel                       true      </para>
        ///  <para>Sgreen     = "ff00ff00" Start                      true      </para>
        ///  <para>Sblack     = "ff000000" Falle (random)             false     </para>
        ///  <para>Sorange    = "ffff8800" taktische Falle 1          true      </para>
        ///  <para>Scyan      = "ff008080" taktische Falle 2          true      </para>
        ///  <para>Sdarkgreen = "ff004000" taktische Falle 3          true      </para>
        ///  
        ///  <para>Sred ist Standardwert.                                       </para>
        /// </summary>
        /// <param name="mask">Bitmap file (1px descripes 1 Tile)</param>
        public Map(Bitmap mask)
        {
            iBlockSize = GameConstants.BlockSize;
            Ziel = new Vector2(0, 0);

            map = new Block[mask.Width, mask.Height];
            Console.WriteLine("Maske: "+mask.Width +":"+ mask.Height);

            for (int row = 0; row < map.GetLength(0); row++)
            {
                for (int col = 0; col < map.GetLength(1); col++)
                {
                    Console.WriteLine("P("+row+":"+col+") color = "+mask.GetPixel(row,col).Name);

                    if (mask.GetPixel(row, col).Name == sBlack)
                    {
                        GetTrap(mask,row,col);                        
                    }
                    else if (mask.GetPixel(row, col).Name == sWhite)
                    {
                        map[row, col] = new Block(0, new Vector2(row * 90, col * 90));
                        // weg
                    }
                    else if (mask.GetPixel(row, col).Name == sGrey)
                    {
                        map[row, col] = new Block(1, new Vector2(row * 90, col * 90));
                        // mauer
                    }
                    else if (mask.GetPixel(row, col).Name == sRed)
                    {
                        map[row, col] = new Block(2, new Vector2(row * 90, col * 90));
                        // loch
                    }
                    else if (mask.GetPixel(row, col).Name == sGreen)
                    {
                        map[row, col] = new Block(3, new Vector2(row * 90, col * 90));
                        Start = new Vector2(row * 90 + 1, col * 90 + 1);
                        // start
                    }
                    else if (mask.GetPixel(row, col).Name == sBlue)
                    {
                        map[row, col] = new Block(4, new Vector2(row * 90, col * 90));
                        Ziel = new Vector2(row * 90 + 45, col * 90 + 45);
                        // ziel
                    }
                    else if (mask.GetPixel(row, col).Name == sOrange)
                    {
                        map[row, col] = new Block(5, new Vector2(row * 90, col * 90));
                        // Falle 1
                    }
                    else if (mask.GetPixel(row, col).Name == sCyan)
                    {
                        map[row, col] = new Block(6, new Vector2(row * 90, col * 90));
                        // Falle 2
                    }
                    else if (mask.GetPixel(row, col).Name == sDarkGreen)
                    {
                        map[row, col] = new Block(7, new Vector2(row * 90, col * 90));
                        // Falle 3
                    }
                    else
                    {
                        map[row, col] = new Block(0, new Vector2(row * 90, col * 90));
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
            int temp = n.Next(1,GameConstants.TrapNumber); // derzeit 4
            switch (temp)
            {
                case 1: map[row, col] = new Block(5, new Vector2(row * 90, col * 90)); // Falle 1
                    break;
                case 2: map[row, col] = new Block(6, new Vector2(row * 90, col * 90)); // Falle 2
                    break;
                case 3: map[row, col] = new Block(7, new Vector2(row * 90, col * 90)); // Falle 3
                    break;
                case 4: map[row, col] = new Block(8, new Vector2(row * 90, col * 90)); // Falle 4
                    break;
                //case 5:
                //    break;
                //case 6:
                //    break;
                default: map[row, col] = new Block(2, new Vector2(row * 90, col * 90)); //loch
                         
                    break;
            }
             
        }

        /// <summary>
        /// Helper to draw each tile of the map
        /// </summary>
        /// <param name="C">main camera</param>
        public void Draw(Camera C)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j].Draw(C);                    
                }
            }
        }
    }
}
