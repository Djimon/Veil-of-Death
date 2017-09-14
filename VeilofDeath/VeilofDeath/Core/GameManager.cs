using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using VeilofDeath.Objects;
using VeilofDeath.Objects.Traps;
using System.IO;

namespace VeilofDeath.Core
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

        private const string awzmSvNm = "LCA00x88.wza";

        public GameManager()
        {
           
        }
        public void Save()
        {
            /* je Zeile nur eine Information
              - maxLevel
              - Volume
              - difficulty (als integer)
              - aktuelles Level (als integer)
              - Level 0 - (i)score;(i)Coins;(i)Time;(f)Completeness
              - Level 0 - (i)score;(i)Coins;(i)Time;(f)Completeness
              - Level 0 - (i)score;(i)Coins;(i)Time;(f)Completeness
              - Level 0 - (i)score;(i)Coins;(i)Time;(f)Completeness
              - Level 0 - (i)score;(i)Coins;(i)Time;(f)Completeness
              - Level 0 - (i)score;(i)Coins;(i)Time;(f)Completeness
              - Veil Distance (float)
             */
            if (this.Level > 0)
            {
                StreamWriter savewriter = new StreamWriter(awzmSvNm);
                int i = 0;

                savewriter.WriteLine(GameConstants.iMaxLevel.ToString());
                savewriter.WriteLine(GameConstants.Volume.ToString());
                savewriter.WriteLine(GameConstants.iDifficulty.ToString());
                savewriter.WriteLine(this.Level.ToString());
                while (i < GameConstants.iMaxLevel)
                {
                    savewriter.WriteLine(this.Score[i].ToString() + ":"
                                        + this.iCoinScore[i].ToString() + ":"
                                        + this.iTimeBonus[i].ToString() + ":"
                                        + this.fStageCleared[i].ToString());
                    i++;
                }
                savewriter.WriteLine(this.fVeilDistance.ToString());
                savewriter.Close();
                Console.WriteLine("Saved...");
            }
            
        }

        public void Load()
        {
            Score = new int[GameConstants.iMaxLevel];
            
            if (File.Exists(awzmSvNm))
            {
                StreamReader savereader = new StreamReader(awzmSvNm);
                int k = 0;
                int maxL = 0;
                try
                {
                    char[] delimiter = { ':', ';' };
                    maxL = int.Parse(savereader.ReadLine());
                    Console.WriteLine("Loaded internal Savings:");
                    GameConstants.Volume = float.Parse(savereader.ReadLine());
                    Console.WriteLine("Volume = " + GameConstants.Volume);
                    GameConstants.iDifficulty = int.Parse(savereader.ReadLine());
                    Console.WriteLine("Difficulty = " +  GameConstants.iDifficulty);
                    this.Level = int.Parse(savereader.ReadLine());                    
                    while (k < maxL)
                    {
                        string[] words = savereader.ReadLine().Split(delimiter);
                        this.Score[k] = int.Parse(words[0]);
                        this.iCoinScore[k] = int.Parse(words[1]);
                        this.iTimeBonus[k] = int.Parse(words[2]);
                        this.fStageCleared[k] = float.Parse(words[3]);
                        Console.WriteLine("Level " + k + " = " + fStageCleared[k]*100 + "%");

                        k++;
                    }
                    this.fVeilDistance = float.Parse(savereader.ReadLine());

                    savereader.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine("failed to load: {0}", e.ToString());
                    this.Level = 0;
                    fVeilDistance = 100;
                    iPhase = 0;
                    FlushStats();
                } 
            }
            else
            {
                this.Level = 0;                
                fVeilDistance = 100;
                iPhase = 0;
                FlushStats();
            }

             

        }

        /// <summary>
        /// Resets the actual Level
        /// </summary>
        public void ResetLevel()
        {
            SpikeList.Clear();
            SlowList.Clear();
            CoinList.Clear();
            RollList.Clear();
            ResetScore();
            ResetPhase();
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
        public int[] Score { get; private set; }
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

        public int DeathCounter { get; private set; }

        public int DeathUp()
        {
            DeathCounter++;
            return DeathCounter;
        }

        public int[] iCoinScore = new int[GameConstants.iMaxLevel];
        public int[] iTimeBonus = new int[GameConstants.iMaxLevel];
        public float[] fStageCleared = new float[GameConstants.iMaxLevel];


        public void FlushStats()
        {
            for (int i = 0; i < GameConstants.iMaxLevel; i++)
            {
                iCoinScore[i] = 0;
                iTimeBonus[i] = 0;
                fStageCleared[i] = 0;
                Score[i] = 0;
            }
        }

        public void UpdateScore(int value)
        {
            Score[Level] = value;
        }

        public void AddtoScore(int value)
        {
            Score[Level] += value;
        }

        /// <summary>
        /// Resets the Score of the actual Level
        /// </summary>
        public void ResetScore()
        {
            Score[Level] = 0;
            iCoinScore[Level] = 0;
            iTimeBonus[Level] = 0;
            fStageCleared[Level] = 0;
        }

        public void ResetToLevel0()
        {
            Level = 0;
            GameConstants.iWinStatus = 0;
        }

        public void LevelUp()
        {
            Level++;
        }

        public void EnterNextPhase()
        {
            iPhase = Math.Min(1+iPhase, 4);

            //LARS: Play Sound: nächste Phase des Schleiers ( z.B. Dröhnen, wie bei Inception?)
            GameConstants.ChangePhase.Play();

            if (GameConstants.isDebugMode)
                Console.WriteLine("Entered Phase "+iPhase);
        }

        public void ResetPhase()
        {
            iPhase = 0;
        }

        public void SetVeilDistance(float x)
        {
            fVeilDistance = x;
        }


        /**************** Ingame Features ********************/
        List<SpikeTrap> SpikeList = new List<SpikeTrap>();
        List<SlowTrap> SlowList = new List<SlowTrap>();
        List<Coin> CoinList = new List<Coin>();
        List<SpikeRoll> RollList = new List<SpikeRoll>();

        public List<SlowTrap> getSlowList()
        {
            return SlowList;
        }

        public void AddSlow(SlowTrap slow)
        {
            SlowList.Add(slow);
        }

        public void AddRoll(SpikeRoll roll)
        {
            RollList.Add(roll);
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

        internal List<SpikeRoll> getRollList()
        {
            return RollList;
        }
    }
}