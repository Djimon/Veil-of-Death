using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeilofDeath
{
    public static class LevelContent
    {
        /// <summary>
        /// Generates lookup table for models with their corresponding string (name)
        /// </summary>
        /// <typeparam name="T">Onject (Model)</typeparam>
        /// <param name="contentManager">MonoGame Content.mgcb</param>
        /// <param name="contentFolder">Path to folder</param>
        /// <returns></returns>
        public static Dictionary<string, T> LoadListContent<T>(this ContentManager contentManager, string contentFolder)
        {
            DirectoryInfo dir = new DirectoryInfo(contentManager.RootDirectory + "/" + contentFolder);
            if (!dir.Exists)
                return null;
            Dictionary<String, T> result = new Dictionary<String, T>();

            FileInfo[] files = dir.GetFiles("*.*"); // TODO: Use as Filter (e.g.: ".fbx")
            foreach (FileInfo file in files)
            {
                string key = Path.GetFileNameWithoutExtension(file.Name);
                Console.WriteLine("...." + key);
                result[key] = contentManager.Load<T>(contentFolder + "/" + key);
            }
            return result;
        }
    }
}
