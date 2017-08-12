using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using VeilofDeath.Objects;
using VeilofDeath.Objects.Traps;

namespace VeilofDeath
{
    class GameManager
    {
        private static GameManager instance;
        /// <summary>
        /// returns the only instance of the GameManager (singlton)
        /// </summary>
        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameManager();
                }
                return instance;
            }
        }        

        public GameManager()
        {
            Load();
        }

        public void Load()
        {
            //TODO: aus datei laden (if Datei leer, then level = 1)
            this.Level = 0;
            Score = new int[GameConstants.iMaxLevel];
        }

        /********* Session Data **********/
        public int Level { get; private set; }
        private int[] Score { get; set; }
        public int score {
            get
            {
                return Score[Level];
            } private set
            { } }
        public int Difficulty { get; set; }
        public Vector3 StartPos { get; set; }
        public Vector3 ZielPos { get; set; }


        public void UpdateScore(int value)
        {
            Score[Level] = value;
        }

        public void AddtoScore(int value)
        {
            Score[Level] += value;
        }
        public void ResetScore()
        {
            Score[Level] = 0;
        }
        public void LevelUp()
        {
            Level++;
        }
        public void Save()
        {           
            //TODO: Session-data in dateispeichern
        }


        /**************** Ingame Features ********************/
        List<SpikeTrap> SpikeList = new List<SpikeTrap>();
        List<SlowTrap> SlowList = new List<SlowTrap>();
        List<Coin> CoinList = new List<Coin>();

        public List<SlowTrap> getSlowList()
        {
            return SlowList;
        }

        public void AddSlow(SlowTrap slow)
        {
            SlowList.Add(slow);
        }



        public List<SpikeTrap> getSpikeList()
        {
            return SpikeList;
        }

        public void AddSpike(SpikeTrap spike)
        {
            SpikeList.Add(spike);
        }

        public List<Coin> getCoinList()
        {
            return CoinList;
        }

        public void AddCoin(Coin coin)
        {
            CoinList.Add(coin);
        }

        internal void Delete(Coin c)
        {
            GameConstants.CoinCollect.Play();
            CoinList.Remove(c);
        }
    }
}