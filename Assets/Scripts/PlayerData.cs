using UnityEngine;

namespace SlingRun
{
    public class PlayerData
    {
        public static int HighScore
        {
            get
            {
                return !PlayerPrefs.HasKey(Constants.HIGHSCORE_KEY) ? 0 : PlayerPrefs.GetInt(Constants.HIGHSCORE_KEY);
            }
            set
            {
                PlayerPrefs.SetInt(Constants.HIGHSCORE_KEY, value);
                PlayerPrefs.Save();
            }
        }
    }
}