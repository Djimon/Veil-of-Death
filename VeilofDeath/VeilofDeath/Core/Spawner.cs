using Microsoft.Xna.Framework;
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
        Random rnd = new Random(4242);
        Vector3 actualPosition;

        private List<Block> freePos = new List<Block>();


        private static Spawner instance;
        /// <summary>
        /// returns the only instance of the camere (singlton)
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

        public Spawner()
        {
            actualPosition = GameManager.Instance.StartPos;
        }


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
                    if (rnd.NextDouble() >= 0.5)
                    {
                        iCoins = CalculateCoins(B);

                        // calculate Coin spawnning

                    }
                }
            }           

            if (rnd.NextDouble() >= 0.67) // go left
            {
                Block tmp = Check("left");
                if (tmp != null)
                {
                    CalculateCoins(tmp);
                }
            }
            else if (rnd.NextDouble() <= 0.33) // go right
            {
                Block tem = Check("left");
                if (tem != null)
                {
                    CalculateCoins(tem);
                }
            }
            else
            {
                //leave
            }
        }

        private int CalculateCoins(Block B)
        {
            int iCoins = rnd.Next(2, 4);
            actualPosition = B.position;
            if (iCoins > 2 && Check("front") == B)
                SpawnCoins(actualPosition, iCoins);
            if (iCoins <= 2)
                SpawnCoins(actualPosition, iCoins);
            return iCoins;
        }


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
                    tmp = freePos.Find(x => x.position == new Vector3(actualPosition.X, actualPosition.Y + GameConstants.iBlockSize, actualPosition.Z));
                    if (tmp != null)
                       return tmp.isFree ? tmp: null ;
                    else return null;
                default:
                    return null;
            }
        }


        private void SpawnCoins(Vector3 pos, int number)
        {            
            switch (number)
            {
                case 1:
                    GameManager.Instance.AddCoin(new Coin(new Vector3(pos.X, pos.Y - GameConstants.iBlockSize / 4, 0)));
                    Console.WriteLine("Placed Coin at " + pos);
                    break;
                case 2:
                    GameManager.Instance.AddCoin(new Coin(new Vector3(pos.X, pos.Y - GameConstants.iBlockSize / 4, 0)));
                    GameManager.Instance.AddCoin(new Coin(new Vector3(pos.X, pos.Y + GameConstants.iBlockSize / 4, 0)));
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
                default:
                    break;
            }    
        }
    }
}
