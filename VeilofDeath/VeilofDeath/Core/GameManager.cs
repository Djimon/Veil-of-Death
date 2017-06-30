using System.Collections.Generic;
using Microsoft.Xna.Framework;
using VeilofDeath.Objects;
using VeilofDeath.Objects.Traps;

namespace VeilofDeath
{
    class GameManager
    {
        private static GameManager instance;
        /// <summary>
        /// returns the only instance of the camere (singlton)
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




        /********* Session Data **********/
        public int Level { get; private set; }
        public static int Score { get; private set; }
        public int Difficulty { get; set; }
        public Vector3 StartPos { get; set; }
        public Vector3 ZielPos { get; set; }

        public static void UpdateScore(int value)
        {
            Score = value;
        }
        public GameManager()
        {
            Load();
        }
        public void LevelUp()
        {
            Level++;
        }
        public void Save()
        {           
            //TODO: Session-data in dateispeichern
        }
        public void Load()
        {
            //TODO: aus datei laden (if Datei leer, then level = 1)
            this.Level = 1;
        }

        /**************** Ingame Features ********************/
        List<SpikeTrap> SpikeList = new List<SpikeTrap>();
        List<SlowTrap> SlowList = new List<SlowTrap>();

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

    }
}