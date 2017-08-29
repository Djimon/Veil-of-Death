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
            fVeilDistance = 100; //TODO: abhängig von GameConstants.iDifficulty machen
            iPhase = 0;
        }

        public void ResetLevel()
        {
            SpikeList.Clear();
            SlowList.Clear();
            CoinList.Clear();
        }

        /********* Session Data **********/

        /// <summary>
        /// Actual Level
        /// <para> 0 - tutorial</para>
        /// <para> 1 - Spike-Trap (Castle)</para>
        /// <para> 2 - Spike + Slow (Castle)</para>
        /// <para> 3 - + Sword (Dessert)</para>
        /// <para> 4 - + Spikeroll (Dessert)</para>
        /// <para>99 - Hidden Bonus-Level</para>
        /// </summary>
        public int Level { get; private set; }
        private int[] Score { get; set; }
        public int score {
            get
            {
                return Score[Level];
            } private set
            { } }

        public Vector3 StartPos { get; set; }
        public Vector3 ZielPos { get; set; }
        public float BestTime { get; internal set; }

        public float fVeilDistance { get; private set; }
        public int iPhase {get; private set;}

        public int[] iCoinScore = new int[GameConstants.iMaxLevel];
        public int[] iTimeBonus = new int[GameConstants.iMaxLevel];
        public float[] fStageCleared = new float[GameConstants.iMaxLevel];

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
            iCoinScore[Level] = 0;
            iTimeBonus[Level] = 0;
            fStageCleared[Level] = 0;
        }
        public void LevelUp()
        {
            Level++;
        }
        public void Save()
        {           
            //TODO: Session-data in dateispeichern
        }

        public void EnterNextPhase()
        {
            iPhase = Math.Min(1+iPhase, 4);

            //LARS: Play Sound: nächste Phase des Schleiers ( z.B. Dröhnen, wie bei Inception?)
            GameConstants.ChangePhase.Play();

            if (GameConstants.isDebugMode)
                Console.WriteLine("Entered Phase "+iPhase);
        }

        public void SetVeilDistance(float x)
        {
            fVeilDistance = x;
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