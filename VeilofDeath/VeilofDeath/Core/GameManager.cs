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
        }

        public void LevelUp()
        {
            Level++;
        }

        public void Save()
        {
            //TODO: aus datei laden
            this.Level = 1;

        }

        public void Load()
        {

        }

    }
}