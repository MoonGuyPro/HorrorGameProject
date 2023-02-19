using System.IO;
using UnityEngine;

namespace Utils
{
    public class HandleSaveFile
    {
        public static void SaveProgress(string lastLevel)
        {
            string path = Application.dataPath + "/save.dat";
            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
            }
            File.WriteAllText(path, lastLevel);
        }
        
        public static string LoadProgress()
        {
            string path = Application.dataPath + "/save.dat";
            if (File.Exists(path))
            {
                StreamReader reader = new StreamReader(path);
                string ret = reader.ReadToEnd();
                reader.Close();
                return ret;
            }
            return "";
        }
    }
}