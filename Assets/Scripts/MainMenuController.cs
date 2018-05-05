using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SlingRun
{
    public class MainMenuController : MonoBehaviour
    {
        public TextMeshProUGUI highScoreText;
        
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
            highScoreText.text = string.Format(Constants.HIGHSCORE_TEXT, PlayerData.HighScore);
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                QuitGame();
            }
        }

        
    }
}

