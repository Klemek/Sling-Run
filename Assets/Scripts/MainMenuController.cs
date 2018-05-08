using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SlingRun
{
    public class MainMenuController : MonoBehaviour
    {
        public TextMeshProUGUI HighScoreText;
        public TextMeshProUGUI VersionText;

        private void PlayGame()
        {
            SceneManager.LoadScene(Constants.GAME_SCENE);
        }

        private void QuitGame()
        {
            Application.Quit();
        }

        private void Start()
        {
            HighScoreText.text = string.Format(Constants.HIGHSCORE_TEXT, PlayerData.HighScore);
            VersionText.text = Constants.VERSION;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) QuitGame();
        }
    }
}