using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using VeilofDeath.Objects;
using VeilofDeath.Objects.Traps;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Globalization;
using VeilofDeath.SpecialFX;

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

        public GUIFlash GUIFX;

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
            if (this.Level >= 0)
            {
                WriteAndEncryptSaveFile();
                //Console.WriteLine("Game saved");
            }

        }

        public void Load()
        {
            Score = new int[GameConstants.iMaxLevel];
            
            if (File.Exists(awzmSvNm))
            {
                LoadAndDecryptSaveData();
                //Console.WriteLine("Game loaded");
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
            SpeedList.Clear();
            BrettList.Clear();
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
        public bool[] hasStoryRead = new bool[4];

        /// <summary>
        /// Resets all statistics
        /// </summary>
        public void FlushStats()
        {
            this.DeathCounter = 0;
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
        /// <summary>
        /// Resets the whole game to beginning
        /// </summary>
        public void ResetToLevel0()
        {
            Level = 0;
            GameConstants.iWinStatus = 0;
            this.hasStoryRead[0] = false;
            this.hasStoryRead[1] = false;
            this.hasStoryRead[2] = false;
            this.hasStoryRead[3] = false;
        }

        public void LevelUp()
        {
            Level++;

            Random rand = new Random();

            double choice = rand.NextDouble();

            if (choice <= 0.5)
            {
                MediaPlayer.Play(GameConstants.music1);
            }
            else
            {
                MediaPlayer.Play(GameConstants.music2);
            }
            MediaPlayer.IsRepeating = true;

            MediaPlayer.Volume = GameConstants.Volume * 2 / 3;

        }

        /// <summary>
        /// Controlls the phases of the veil
        /// </summary>
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
        List<SpeedTrap> SpeedList = new List<SpeedTrap>();
        List<BrettTrap> BrettList = new List<BrettTrap>();

        public List<BrettTrap> getBrettList()
        {
            return BrettList;
        }

        public void AddBrett(BrettTrap brett)
        {
            BrettList.Add(brett);
        }

        public void DeleteBrett(BrettTrap B)
        {
            BrettList.Remove(B);
        }

        public List<SpeedTrap> getSpeedList()
        {
            return SpeedList;
        }

        public void AddSpeed(SpeedTrap speed)
        {
            SpeedList.Add(speed);
        }

        public void DeleteBuff(SpeedTrap s)
        {
            SpeedList.Remove(s);
        }

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

        #region Verschlüsselung und Entschlüsselung: Save 'n' Load

        private readonly byte[] key = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 };
        private readonly byte[] iv = new byte[] { 65, 110, 68, 26, 69, 178, 200, 219 };

        /// <summary>
        /// Verschlüsselt einen Eingabestring.
        /// </summary>
        /// <param name="input">Der zu verschlüsselnde String.</param>
        /// <returns>Byte-Array mit dem verschlüsselten String.</returns>
        public byte[] MakeUnreadable(string input)
        {
            try
            {
                // MemoryStream Objekt erzeugen
                MemoryStream memoryStream = new MemoryStream();

                // CryptoStream Objekt erzeugen und den Initialisierungs-Vektor
                // sowie den Schlüssel übergeben.
                CryptoStream cryptoStream = new CryptoStream(
                memoryStream, new TripleDESCryptoServiceProvider().CreateEncryptor(this.key, this.iv), CryptoStreamMode.Write);

                // Eingabestring in ein Byte-Array konvertieren
                byte[] toEncrypt = new ASCIIEncoding().GetBytes(input);

                // Byte-Array in den Stream schreiben und flushen.
                cryptoStream.Write(toEncrypt, 0, toEncrypt.Length);
                cryptoStream.FlushFinalBlock();

                // Ein Byte-Array aus dem Memory-Stream auslesen
                byte[] ret = memoryStream.ToArray();

                // Stream schließen.
                cryptoStream.Close();
                memoryStream.Close();

                // Rückgabewert.
                return ret;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(String.Format(CultureInfo.CurrentCulture, "Fehler beim Verschlüsseln: {0}", e.Message));
                return null;
            }
        }

        /// <summary>
        /// Entschlüsselt einen String aus einem Byte-Array.
        /// </summary>
        /// <param name="data">Das verschlüsselte Byte-Array.</param>
        /// <returns>Entschlüsselter String.</returns>
        public string MakeReadable(byte[] data)
        {
            try
            {
                // Ein MemoryStream Objekt erzeugen und das Byte-Array
                // mit den verschlüsselten Daten zuweisen.
                MemoryStream memoryStream = new MemoryStream(data);

                // Ein CryptoStream Objekt erzeugen und den MemoryStream hinzufügen.
                // Den Schlüssel und Initialisierungsvektor zum entschlüsseln verwenden.
                CryptoStream cryptoStream = new CryptoStream(
                memoryStream,
                new TripleDESCryptoServiceProvider().CreateDecryptor(this.key, this.iv), CryptoStreamMode.Read);
                // Buffer erstellen um die entschlüsselten Daten zuzuweisen.
                byte[] fromEncrypt = new byte[data.Length];

                // Read the decrypted data out of the crypto stream
                // and place it into the temporary buffer.
                // Die entschlüsselten Daten aus dem CryptoStream lesen
                // und im temporären Puffer ablegen.
                cryptoStream.Read(fromEncrypt, 0, fromEncrypt.Length);

                // Den Puffer in einen String konvertieren und zurückgeben.
                return new ASCIIEncoding().GetString(fromEncrypt);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(String.Format(CultureInfo.CurrentCulture, "Fehler beim Entschlüsseln: {0}", e.Message));
                return null;
            }
        }



        /// <summary>
        /// converts a byte array to a hex string
        /// </summary>
        /// <param name="ba">byte array</param>
        /// <returns></returns>
        public static string ByteArrayToString(byte[] ba)
        {
            string hex = BitConverter.ToString(ba);
            return hex.Replace("-", "");
        }

        /// <summary>
        /// Converts a hex string to byte array
        /// </summary>
        /// <param name="hex">string in hex</param>
        /// <returns></returns>
        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        /// <summary>
        /// Reads end decrypts the save file to load the game details
        /// </summary>
        private void LoadAndDecryptSaveData()
        {
            StreamReader savereader = new StreamReader(awzmSvNm);
            //Console.WriteLine(savereader.ToString());
            string decoded = MakeReadable(StringToByteArray(savereader.ReadLine()));
            savereader.Close();
            //Console.WriteLine(decoded);
            StreamWriter tmp1 = new StreamWriter("~4426377.dz");
            tmp1.Write(decoded);
            tmp1.Close();
            StreamReader tmp2 = new StreamReader("~4426377.dz");
            int k = 0;
            int maxL = 0;
            try
            {
                char[] delimiter = { ':', ';' };
                maxL = int.Parse(tmp2.ReadLine());
                Console.WriteLine("Loaded Savefile:");
                GameConstants.Volume = float.Parse(tmp2.ReadLine());
                Console.WriteLine("Volume = " + GameConstants.Volume);
                GameConstants.iDifficulty = int.Parse(tmp2.ReadLine());
                Console.WriteLine("Difficulty = " + GameConstants.iDifficulty);
                this.Level = int.Parse(tmp2.ReadLine());
                while (k < maxL)
                {
                    string[] words = tmp2.ReadLine().Split(delimiter);
                    this.Score[k] = int.Parse(words[0]);
                    this.iCoinScore[k] = int.Parse(words[1]);
                    this.iTimeBonus[k] = int.Parse(words[2]);
                    this.fStageCleared[k] = float.Parse(words[3]);
                    Console.WriteLine("Level " + k + " = " + fStageCleared[k] * 100 + "%");

                    k++;
                }
                this.fVeilDistance = float.Parse(tmp2.ReadLine());
                this.DeathCounter = int.Parse(tmp2.ReadLine());

                tmp2.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("failed to load: {0}", e.ToString());
                this.Level = 0;
                fVeilDistance = 100;
                iPhase = 0;
                FlushStats();
            }

            File.Delete("~4426377.dz");
        }

        /// <summary>
        /// Writes all necessary save information of the game into a string and encrypts the save file
        /// </summary>
        private void WriteAndEncryptSaveFile()
        {
            if (File.Exists(awzmSvNm))
                File.Delete(awzmSvNm);

            StreamWriter savewriter = new StreamWriter(awzmSvNm);
            string savestring = "";
            int i = 0;

            savestring += GameConstants.iMaxLevel.ToString() + "\n";
            savestring += GameConstants.Volume.ToString() + "\n";
            savestring += GameConstants.iDifficulty.ToString() + "\n";
            savestring += this.Level.ToString() + "\n";
            while (i < GameConstants.iMaxLevel)
            {
                savestring += (this.Score[i].ToString() + ":"
                                    + this.iCoinScore[i].ToString() + ":"
                                    + this.iTimeBonus[i].ToString() + ":"
                                    + this.fStageCleared[i].ToString()) + "\n";
                i++;
            }
            savestring += (this.fVeilDistance.ToString()) + "\n";
            savestring += (this.DeathCounter.ToString()) + "\n";
            //Console.WriteLine(savestring);
            savewriter.Write(ByteArrayToString(MakeUnreadable(savestring)));
            //Console.WriteLine(savewriter);
            savewriter.Close();
            Console.WriteLine("Game Saved...");
        }

        #endregion


    }
}