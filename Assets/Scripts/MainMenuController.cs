using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    #region Unity Attributes

    public TextMeshProUGUI HighScoreText;
    public TextMeshProUGUI MoneyText;
    public TextMeshProUGUI VersionText;

    #endregion

    #region Unity Methods
    
    private void Start()
    {
        HighScoreText.text = string.Format(Constants.HighscoreText, PlayerData.HighScore);
        VersionText.text = Constants.Version;
        MoneyText.text = PlayerData.Money.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) QuitGame();
    }
    
    #endregion
    
    #region Methods
    
    public void PlayGame()
    {
        SceneManager.LoadScene(Constants.GameScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    #endregion

    
}