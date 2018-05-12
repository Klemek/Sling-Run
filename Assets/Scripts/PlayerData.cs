using UnityEngine;

public static class PlayerData
{
    public static int HighScore
    {
        get { return !PlayerPrefs.HasKey(Constants.HighscoreKey) ? 0 : PlayerPrefs.GetInt(Constants.HighscoreKey); }
        set
        {
            PlayerPrefs.SetInt(Constants.HighscoreKey, value);
            PlayerPrefs.Save();
        }
    }

    public static int Money
    {
        get { return !PlayerPrefs.HasKey(Constants.MoneyKey) ? 0 : PlayerPrefs.GetInt(Constants.MoneyKey); }
        set
        {
            PlayerPrefs.SetInt(Constants.MoneyKey, value);
            PlayerPrefs.Save();
        }
    }
}