﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeilofDeath.Objects;

namespace VeilofDeath.Core
{
    class Spawner
    {
        Random rnd = new Random();
        Vector3 actualPosition;

        private List<Block> freePos = new List<Block>();


        private static Spawner instance;
        /// <summary>
        /// returns the only instance of the Spawner (singlton)
        /// </summary>
        public static Spawner Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Spawner();
                }
                return instance;
            }
        }

        /// <summary>
        /// CONSTRUCTOR
        /// </summary>
        public Spawner()
        {
            actualPosition = GameManager.Instance.StartPos;
        }


        /// <summary>
        /// main method to place Coins in the level in dependene of the generated map and free spaces
        /// </summary>
        /// <param name="map">block matrix of the actual map</param>
        public void PlaceCoins(Block[,] map)
        {
            int iCoins = 0;

            foreach (Block B in map)
            {
                if (B.blockType == 0
                    && B.isFree
                    && B.position.Y > 2 * GameConstants.iBlockSize)
                {
                    freePos.Add(B);
                    if (rnd.NextDouble() >= 0.8)
                    {
                        if (rnd.NextDouble() >= 0.67) // go left
                        {
                            Block tmp = Check("left");
                            if (tmp != null)
                            {
                                iCoins += CalculateCoins(tmp);
                            }
                        }
                        else if (rnd.NextDouble() <= 0.33) // go right
                        {
                            Block tem = Check("right");
                            if (tem != null)
                            {
                                iCoins += CalculateCoins(tem);
                            }
                        }
                        else
                        {
                            iCoins += CalculateCoins(B);
                        }

                    } /* if if (rnd.NextDouble() >= 0.85) */
                } /* if (B.blockType == 0 */
            } /* foreach (Block B in map) */

            if (GameConstants.isDebugMode)
                Console.WriteLine("Coins placed: " +iCoins);
        }

        /// <summary>
        /// Randomly sets the among of Coins and place them
        /// </summary>
        /// <param name="B">Startblock on which the coins should be placed</param>
        /// <returns></returns>
        private int CalculateCoins(Block B)
        {
            int iCoins = rnd.Next(2, 6);
            actualPosition = B.position;
            if (iCoins > 2 && Check("front") == B)
                SpawnCoins(actualPosition, iCoins);
            if (iCoins <= 2)
                SpawnCoins(actualPosition, iCoins);
            return iCoins;
        }

        /// <summary>
        /// Check if the next Block is free to place Coins
        /// </summary>
        /// <param name="direction">which diretcion should be gecked</param>
        /// <para name="left"> looks on the left lane one block above</para>
        /// <para name="right"> looks on the right lane one block above</para>
        /// <para name="front"> looks on the same lane one block above</para>
        /// <returns></returns>
        private Block Check(string direction)
        {
            Block tmp;
            switch (direction)
            {
                case "left":
                    tmp = freePos.Find(x => x.position == new Vector3(actualPosition.X - GameConstants.iBlockSize, actualPosition.Y + GameConstants.iBlockSize, actualPosition.Z));
                    if (tmp != null)
                        return tmp.isFree ? tmp : null;
                    else return null;
                case "right":
                    tmp = freePos.Find(x => x.position == new Vector3(actualPosition.X + GameConstants.iBlockSize, actualPosition.Y + GameConstants.iBlockSize, actualPosition.Z));
                    if (tmp != null)
                        return tmp.isFree ? tmp: null;
                    else return null;
                case "front":
                    tmp = freePos.Find(x => x.position == new Vector3(actualPosition.X, actualPosition.Y +  GameConstants.iBlockSize, actualPosition.Z));
                    if (tmp != null)
                       return tmp.isFree ? tmp: null ;
                    else return null;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Helper to Place the Coins to the correct Position
        /// </summary>
        /// <param name="pos">Position where the 1st coin is placed</param>
        /// <param name="number">coins, follow in one row</param>
        private void SpawnCoins(Vector3 pos, int number)
        {            
            switch (number)
            {
                case 1:
                    GameManager.Instance.AddCoin(new Coin(new Vector3(pos.X, pos.Y - GameConstants.iBlockSize /2, 0)));
                    if (GameConstants.isDebugMode)
                        Console.WriteLine("Placed Coin at " + pos);
                    break;
                case 2:
                    GameManager.Instance.AddCoin(new Coin(new Vector3(pos.X, pos.Y - GameConstants.iBlockSize / 2, 0)));
                    if (GameConstants.isDebugMode)
                        Console.WriteLine("Placed Coin at " + pos);
                    GameManager.Instance.AddCoin(new Coin(new Vector3(pos.X, pos.Y + GameConstants.iBlockSize / 2, 0)));
                    if (GameConstants.isDebugMode)
                        Console.WriteLine("Placed Coin at " + pos);
                    break;
                case 3:
                    pos.Y += GameConstants.iBlockSize;
                    SpawnCoins(pos, 1);
                    break;
                case 4:
                    pos.Y += GameConstants.iBlockSize;
                    SpawnCoins(pos, 2);
                    break;
                case 5:
                    pos.Y += GameConstants.iBlockSize;
                    SpawnCoins(pos, 3);
                    break;
                case 6:
                    pos.Y += GameConstants.iBlockSize;
                    SpawnCoins(pos, 4);
                    break;
                default:
                    break;
            }    
        }
    }
}
