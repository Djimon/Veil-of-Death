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
        public int Score { get; private set; }
        public int Difficulty { get; set; }





        public GameManager()
        {
            Load();
            Level = 0;
        }

        public void LevelUp()
        {
            Level++;
        }

        public void Save()
        {

        }

        public void Load()
        {

        }

    }
}